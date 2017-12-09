using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using ProtoBuf;
using ProtoBuf.Meta;
using Force.Crc32;

namespace GameMain {
	public class WebSocketChannelHelper : IWebSocketChannelHelper {

		private IWebSocketChannel m_networkChannel;

		private Dictionary<string, Type> m_packetTypeDict;
		private Dictionary<Protos.PacketType, string> m_packetNamePrefixDict;
		private List<ProtoHandlerBase> m_packetHandlerList;

		public WebSocketChannelHelper(){
			m_packetTypeDict = new Dictionary<string, Type> ();
			m_packetNamePrefixDict = new Dictionary<Protos.PacketType, string> ();
			m_packetHandlerList = new List<ProtoHandlerBase> ();
		}

		#region Implement

		public int PacketHeaderLength {
			get{ 
				return Constant.C_WebSocket.WebSocketHeaderLength;
			}
		}
			
		public void Initialize (IWebSocketChannel networkChannel){
			
			m_networkChannel = networkChannel;

			Type handlerBaseType = typeof(ProtoHandlerBase);

			Assembly assembly = Assembly.GetExecutingAssembly();
			Type[] types = assembly.GetTypes ();
			foreach (var type in types) {
				if (type.Namespace == "GameMain.Protos") {

					if (type.IsEnum && type.Name == "PacketType") {
						var arr = Enum.GetValues (type);
						foreach (var e in arr) {
							m_packetNamePrefixDict.Add ((Protos.PacketType)e, e.ToString ());
						}
					}

					if (type.IsClass) {
						m_packetTypeDict.Add (type.Name, type);
					}

				} else if(type.Namespace == "GameMain" && type.IsClass && !type.IsAbstract 
					&& type.BaseType == handlerBaseType) {

					ProtoHandlerBase handler = (ProtoHandlerBase)Activator.CreateInstance (type);
					m_packetHandlerList.Add (handler);
					GameEntry.Event.Subscribe (handler.PacketId, handler.Handler);

				}
			}

			GameEntry.Event.Subscribe(WebSocketConnectedEventArgs.EventId, OnNetworkConnected);
			GameEntry.Event.Subscribe(WebSocketClosedEventArgs.EventId, OnNetworkClosed);
			GameEntry.Event.Subscribe(WebSocketSentEventArgs.EventId, OnNetworkSendPacket);
			GameEntry.Event.Subscribe(WebSocketMissHeartBeatEventArgs.EventId, OnNetworkMissHeartBeat);
			GameEntry.Event.Subscribe(WebSocketErrorEventArgs.EventId, OnNetworkError);
		}

		public byte[] Serialize (object packet, int packetId){

			if (packet == null) {
				throw new GameFrameworkException ("Invalid Packet to Serialize.");
			}

			if (packetId == (int)Protos.PacketType.None) {
				throw new GameFrameworkException ("Invalid Packet Type to Serialize");
			}

			MemoryStream stream = new MemoryStream ();
			int length = 0;
			using (MemoryStream pStream = new MemoryStream ()) {
				pStream.Write (BitConverter.GetBytes (packetId), 0, 4);
				RuntimeTypeModel.Default.Serialize (pStream, packet);
				uint crc = Crc32Algorithm.Compute (pStream.GetBuffer (), 0, (int)pStream.Length);
				pStream.Write (BitConverter.GetBytes (crc), 0, 4);
				length = (int)pStream.Length;
				stream.Write (BitConverter.GetBytes (length), 0, PacketHeaderLength);
				stream.Write (pStream.ToArray (), 0, length);
			}

			return stream.ToArray ();
		}

		public object DeserializePacket (Stream source, out int packetId, bool request = false){
			byte[] data = new byte[PacketHeaderLength];
			int readCount = source.Read (data, 0, PacketHeaderLength);
			if (readCount != PacketHeaderLength) {
				throw new GameFrameworkException ("Invalid Packet Header Length");
			}
			int length = BitConverter.ToInt32 (data, 0);
			byte[] packetData = new byte[length];
			readCount = source.Read (packetData, 0, length);
			if (readCount != length) {
				throw new GameFrameworkException ("Invalid Packet Length");
			}
			object ret = null;
			using (MemoryStream stream = new MemoryStream (packetData)) {
				int idLength = stream.Read (data, 0, PacketHeaderLength);
				if (idLength != PacketHeaderLength) {
					throw new GameFrameworkException ("Invalid Packet Id Length");
				}
				packetId = BitConverter.ToInt32 (data, 0);
				Type packetType = GetPacketType ((Protos.PacketType)packetId, request);
				if (packetType == null) {
					throw new GameFrameworkException ("Invalid Packet Id: " + packetId.ToString ());
				}

				if (!Crc32Algorithm.IsValidWithCrcAtEnd (packetData, 0, length)) {
					throw new GameFrameworkException ("Invalid PacketData with Crc32");
				}

				stream.SetLength (length - 4);
				ret = RuntimeTypeModel.Default.Deserialize (stream, null, packetType);
			}

			return ret;
		}

		public void Shutdown (){
			GameEntry.Event.Unsubscribe(WebSocketConnectedEventArgs.EventId, OnNetworkConnected);
			GameEntry.Event.Unsubscribe(WebSocketClosedEventArgs.EventId, OnNetworkClosed);
			GameEntry.Event.Unsubscribe(WebSocketSentEventArgs.EventId, OnNetworkSendPacket);
			GameEntry.Event.Unsubscribe(WebSocketMissHeartBeatEventArgs.EventId, OnNetworkMissHeartBeat);
			GameEntry.Event.Unsubscribe(WebSocketErrorEventArgs.EventId, OnNetworkError);

			m_networkChannel = null;
		}

		public bool SendHeartBeat(){
			Protos.HeartBeat_Request hb = new GameMain.Protos.HeartBeat_Request ();
			m_networkChannel.Send (hb, (int)Protos.PacketType.HeartBeat);
			return true;
		}

		#endregion

		public Type GetPacketType(Protos.PacketType type, bool request = false){
			Type ret = null;
			if (type == GameMain.Protos.PacketType.None) {
				Log.Error ("Invalide Packet Type: {0}", type);
				return ret;
			}

			string prefix = null;
			if (!m_packetNamePrefixDict.TryGetValue (type, out prefix)) {
				Log.Error ("Packet Type not registed: {0}", type);
				return ret;
			}

			prefix = string.Format ("{0}_{1}", prefix, request ? "Request" : "Result");

			if (!m_packetTypeDict.TryGetValue (prefix, out ret)) {
				Log.Error ("Packet not registed: {0}", prefix);
				return ret;
			}

			return ret;
		}

		public Protos.PacketType GetPacketId(Type type, bool request = true){
			if (type == null || type.Namespace != "GameMain.Protos") {
				Log.Error ("Invalid Packet Type");
				return Protos.PacketType.None;
			}

			string prefix = type.Name.Replace (request ? "_Request" : "_Result", "");
			return (Protos.PacketType)Enum.Parse (typeof(Protos.PacketType), prefix);
		}

		#region Event Callback

		private void OnNetworkConnected(object sender, GameEventArgs e)
		{
			WebSocketConnectedEventArgs ne = (WebSocketConnectedEventArgs)e;
			if (ne.Channel != m_networkChannel)
			{
				return;
			}

			Log.Info("Network channel '{0}' connected, Host '{1}', remote address '{2}', remote prot '{3}'.", ne.Channel.Name, ne.Channel.Host, ne.Channel.RemoteAddress, ne.Channel.RemotePort);
		}

		private void OnNetworkClosed(object sender, GameEventArgs e)
		{
			WebSocketClosedEventArgs ne = (WebSocketClosedEventArgs)e;
			if (ne.Channel != m_networkChannel)
			{
				return;
			}

			Log.Info("Network channel '{0}' closed.", ne.Channel.Name);
		}

		private void OnNetworkSendPacket(object sender, GameEventArgs e)
		{
			WebSocketSentEventArgs ne = (WebSocketSentEventArgs)e;
			if (ne.Channel != m_networkChannel)
			{
				return;
			}

			Log.Info("Network channel '{0}' Send Data Length '{1}'.", ne.Channel.Name, ne.BytesSent);
		}

		private void OnNetworkMissHeartBeat(object sender, GameEventArgs e)
		{
			WebSocketMissHeartBeatEventArgs ne = (WebSocketMissHeartBeatEventArgs)e;
			if (ne.Channel != m_networkChannel)
			{
				return;
			}

			Log.Info("Network channel '{0}' miss heart beat '{1}' times.", ne.Channel.Name, ne.MissCount.ToString());

			if (ne.MissCount < 2)
			{
				return;
			}

			ne.Channel.Close();
		}

		private void OnNetworkError(object sender, GameEventArgs e)
		{
			WebSocketErrorEventArgs ne = (WebSocketErrorEventArgs)e;
			if (ne.Channel != m_networkChannel)
			{
				return;
			}

			Log.Info("Network channel '{0}' error, error code is '{1}', error message is '{2}'.", ne.Channel.Name, ne.ErrorCode.ToString(), ne.ErrorMessage);

			ne.Channel.Close();
		}

		#endregion

	}
}
