using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;

namespace GameMain {

	[DisallowMultipleComponent]
	public partial class WebsocketNetworkComponent : GameFrameworkComponent {

		private Dictionary<string, WebSocketChannel> m_WebSocketChannels;

		protected override void Awake ()
		{
			base.Awake ();
			m_WebSocketChannels = new Dictionary<string, WebSocketChannel> ();
		}

		private void Start(){
//			//Test code
//			var channel = CreateWebSocketChannel ("test", new WebSocketChannelHelper ());
//			channel.Connect ("ws://localhost:3000", "user", "pass");
//			GameEntry.Event.Subscribe (WebSocketConnectedEventArgs.EventId, (sender, args) => {
//				var cargs = args as WebSocketConnectedEventArgs;
//				if(cargs.Channel.Name == "test"){
//					channel.Send (new Protos.HeartBeat_Result (), (int)Protos.PacketType.HeartBeat);
//					GameEntry.Event.Subscribe ((int)Protos.PacketType.HeartBeat, (s, a) => {
//						var packet = a as WebSocketReceivedPacketEventArgs;
//						Log.Info("received id: {0} , channel {1}, packet: {2}", packet.Id, packet.Channel, packet.Packet);
//					});
//				}
//			});
		}

		private void Update(){
			foreach (var pair in m_WebSocketChannels) {
				pair.Value.Update (Time.deltaTime, Time.unscaledDeltaTime);
			}
		}

		public int WebSocketChannelCount {
			get{ 
				return m_WebSocketChannels.Count;
			}
		}

		/// <summary>
		/// 检查是否存在网络频道。
		/// </summary>
		/// <param name="name">网络频道名称。</param>
		/// <returns>是否存在网络频道。</returns>
		public bool HasWebSocketChannel(string name){
			return m_WebSocketChannels.ContainsKey (name);
		}

		/// <summary>
		/// 获取网络频道。
		/// </summary>
		/// <param name="name">网络频道名称。</param>
		/// <returns>要获取的网络频道。</returns>
		public IWebSocketChannel GetWebsocketChannel(string name){
			WebSocketChannel ret = null;
			m_WebSocketChannels.TryGetValue (name, out ret);
			return ret;
		}

		/// <summary>
		/// 获取所有网络频道。
		/// </summary>
		/// <returns>所有网络频道。</returns>
		public IWebSocketChannel[] GetAllWebSocketChannels(){
			WebSocketChannel[] ret = new WebSocketChannel[m_WebSocketChannels.Count];
			m_WebSocketChannels.Values.CopyTo (ret, 0);
			return ret;
		}

		/// <summary>
		/// 创建网络频道。
		/// </summary>
		/// <param name="name">网络频道名称。</param>
		/// <param name="networkChannelHelper">网络频道辅助器。</param>
		/// <returns>要创建的网络频道。</returns>
		public IWebSocketChannel CreateWebSocketChannel(string name, IWebSocketChannelHelper networkChannelHelper){
			if (string.IsNullOrEmpty (name)) {
				throw new GameFrameworkException ("Invalid Channel Name");
			}

			if (networkChannelHelper == null) {
				throw new GameFrameworkException ("ChannelHelper Can not be null");
			}

			if (HasWebSocketChannel (name)) {
				throw new GameFrameworkException ("Already Exists Channel " + name);
			}

			WebSocketChannel ret = new WebSocketChannel (name, networkChannelHelper);

			ret.NetworkChannelConnected += OnWebSocketChannelConnected;
			ret.NetworkChannelSended += OnWebSocketChannelSended;
			ret.NetworkChannelClosed += OnWebSocketChannelClosed;
			ret.NetworkChannelMissHeartBeat += OnWebSocketChannelMissHeartBeat;
			ret.NetworkChannelError += OnWebSocketChannelError;

			m_WebSocketChannels.Add (name, ret);
			return ret;
		}

		/// <summary>
		/// 销毁网络频道。
		/// </summary>
		/// <param name="name">网络频道名称。</param>
		/// <returns>是否销毁网络频道成功。</returns>
		public bool DestroyWebSocketChannel(string name){
			WebSocketChannel channel = null;
			if (m_WebSocketChannels.TryGetValue (name, out channel)) {

				channel.NetworkChannelConnected -= OnWebSocketChannelConnected;
				channel.NetworkChannelSended -= OnWebSocketChannelSended;
				channel.NetworkChannelClosed -= OnWebSocketChannelClosed;
				channel.NetworkChannelMissHeartBeat -= OnWebSocketChannelMissHeartBeat;
				channel.NetworkChannelError -= OnWebSocketChannelError;

				channel.Shutdown ();
				m_WebSocketChannels.Remove (name);
			}

			return false;
		}

		private void OnWebSocketChannelConnected(WebSocketChannel networkChannel, object userData)
		{
			var args = ReferencePool.Acquire<WebSocketConnectedEventArgs> ().Fill (networkChannel, userData);
			GameEntry.Event.Fire (this, args);
		}

		private void OnWebSocketChannelClosed(WebSocketChannel networkChannel)
		{
			var args = ReferencePool.Acquire<WebSocketClosedEventArgs> ().Fill (networkChannel);
			GameEntry.Event.Fire (this, args);
		}

		private void OnWebSocketChannelSended(WebSocketChannel networkChannel, int bytesSent, object userData)
		{
			var args = ReferencePool.Acquire<WebSocketSentEventArgs> ().Fill (networkChannel, bytesSent, userData);
			GameEntry.Event.Fire (this, args);
		}

		private void OnWebSocketChannelMissHeartBeat(WebSocketChannel networkChannel, int missHeartBeatCount)
		{
			var args = ReferencePool.Acquire<WebSocketMissHeartBeatEventArgs> ().Fill (networkChannel, missHeartBeatCount);
			GameEntry.Event.Fire (this, args);
		}

		private void OnWebSocketChannelError(WebSocketChannel networkChannel, GameFramework.Network.NetworkErrorCode errorCode, string errorMessage)
		{
			var args = ReferencePool.Acquire<WebSocketErrorEventArgs> ().Fill (networkChannel, errorCode, errorMessage);
			GameEntry.Event.Fire (this, args);
		}
	}
}
