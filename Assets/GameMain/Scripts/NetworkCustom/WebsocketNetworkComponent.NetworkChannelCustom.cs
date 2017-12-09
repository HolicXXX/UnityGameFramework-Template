using System;
using System.IO;
using System.Net;
using UnityEngine;
using GameFramework;
using GameFramework.Event;
using GameFramework.Network;
using WebSocketSharp;

namespace GameMain {
	/// <summary>
	/// 网络自定义组件，用于WebSocket
	/// </summary>
	public partial class WebsocketNetworkComponent {
		/// <summary>
		/// 自定义网络频道，用于WebSocket
		/// </summary>
		public class WebSocketChannel : IWebSocketChannel, IDisposable{

			private const float DefaultHeartBeatInterval = 30f;

			private readonly string m_Name;
			private readonly IWebSocketChannelHelper m_NetworkChannelHelper;
			private bool m_ResetHeartBeatElapseSecondsWhenReceivePacket;
			private float m_HeartBeatInterval;
			private WebSocket m_Socket;
			private float m_heartbeatTime;
			private int m_missingHeartBeatCount;
			private bool m_Active;
			private bool m_Disposed;

			public GameFrameworkAction<WebSocketChannel, object> NetworkChannelConnected;
			public GameFrameworkAction<WebSocketChannel> NetworkChannelClosed;
			public GameFrameworkAction<WebSocketChannel, int, object> NetworkChannelSended;
			public GameFrameworkAction<WebSocketChannel, int> NetworkChannelMissHeartBeat;
			public GameFrameworkAction<WebSocketChannel, NetworkErrorCode, string> NetworkChannelError;

			/// <summary>
			/// 初始化自定义网络频道的新实例。
			/// </summary>
			/// <param name="name">网络频道名称。</param>
			/// <param name="networkChannelHelper">网络频道辅助器。</param>
			public WebSocketChannel(string name, IWebSocketChannelHelper networkChannelHelper){
				m_Name = name ?? string.Empty;
				m_NetworkChannelHelper = networkChannelHelper;
				m_ResetHeartBeatElapseSecondsWhenReceivePacket = false;
				m_HeartBeatInterval = DefaultHeartBeatInterval;
				m_Socket = null;
				m_heartbeatTime = 0f;
				m_missingHeartBeatCount = 0;
				m_Active = false;
				m_Disposed = false;

				NetworkChannelConnected = null;
				NetworkChannelClosed = null;
				NetworkChannelSended = null;
				NetworkChannelMissHeartBeat = null;
				NetworkChannelError = null;

				networkChannelHelper.Initialize(this);
			}

			#region Implement

			/// <summary>
			/// 获取是否已连接。
			/// </summary>
			public bool Connected {
				get{ 
					if (m_Socket != null) {
						return m_Socket.IsAlive;
					}
					return false;
				}
			}

			/// <summary>
			/// 获取或设置心跳间隔时长，以秒为单位。
			/// </summary>
			public float HeartBeatInterval {
				get{ 
					return m_HeartBeatInterval;
				}
				set{ 
					m_HeartBeatInterval = value;
				}
			}

			/// <summary>
			/// 获取网络频道名称。
			/// </summary>
			public string Name {
				get{ 
					return m_Name;
				}
			}

			/// <summary>
			/// 获取主机地址
			/// </summary>
			public string Host {
				get{
					if (m_Socket == null)
					{
						throw new GameFrameworkException("You must connect first.");
					}

					return m_Socket.Url.Host;
				}
			}

			/// <summary>
			/// 获取远程终结点的地址。
			/// </summary>
			public string RemoteAddress {
				get{ 
					if (m_Socket == null)
					{
						throw new GameFrameworkException("You must connect first.");
					}

					return m_Socket.Url.AbsoluteUri;
				}
			}

			/// <summary>
			/// 获取远程终结点的端口号。
			/// </summary>
			public int RemotePort {
				get{ 
					if (m_Socket == null)
					{
						throw new GameFrameworkException("You must connect first.");
					}

					return m_Socket.Url.Port;
				}
			}

			/// <summary>
			/// 获取或设置当收到消息包时是否重置心跳流逝时间。
			/// </summary>
			public bool ResetHeartBeatElapseSecondsWhenReceivePacket {
				get{ 
					return m_ResetHeartBeatElapseSecondsWhenReceivePacket;
				}
				set{ 
					m_ResetHeartBeatElapseSecondsWhenReceivePacket = value;
				}
			}

			/// <summary>
			/// 网络频道轮询。
			/// </summary>
			/// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
			/// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
			public void Update(float elapseSeconds, float realElapseSeconds)
			{
				if (m_Socket == null || !m_Active)
				{
					return;
				}

				if (m_HeartBeatInterval > 0f)
				{
					bool sendHeartBeat = false;
					int missHeartBeatCount = 0;
					lock (this)
					{
						m_heartbeatTime += realElapseSeconds;
						if (m_heartbeatTime >= m_HeartBeatInterval)
						{
							sendHeartBeat = true;
							missHeartBeatCount = m_missingHeartBeatCount;
							m_heartbeatTime = 0f;
							m_missingHeartBeatCount++;
						}
					}

					if (sendHeartBeat && m_NetworkChannelHelper.SendHeartBeat())
					{
						if (missHeartBeatCount > 0 && NetworkChannelMissHeartBeat != null)
						{
							NetworkChannelMissHeartBeat(this, missHeartBeatCount);
						}
					}
				}
			}

			/// <summary>
			/// 关闭网络频道。
			/// </summary>
			public void Close (){
				if (m_Socket == null || !m_Active)
				{
					return;
				}

				m_Active = false;
				try
				{
					m_Socket.CloseAsync();
				}
				catch
				{
				}
			}

			/// <summary>
			/// 连接到远程主机。
			/// </summary>
			/// <param name="uri">需要连接的Uri</param>
			public void Connect (Uri uri, string userName = null, string passWord = null){
				if (m_Socket != null)
				{
					Close();
					m_Socket = null;
				}

				if (!uri.Scheme.Equals ("ws") && !uri.Scheme.Equals ("wss:")) {
					throw new GameFrameworkException ("Invalid WebSocket Uri: " + uri.AbsoluteUri);
				}

				m_Socket = new WebSocket (uri.ToString());
				if (m_Socket == null)
				{
					string errorMessage = "Initialize network channel failure.";
					if (NetworkChannelError != null)
					{
						NetworkChannelError(this, NetworkErrorCode.SocketError, errorMessage);
						return;
					}

					throw new GameFrameworkException(errorMessage);
				}

				if (!string.IsNullOrEmpty (userName) && !string.IsNullOrEmpty (passWord)) {
					m_Socket.SetCredentials (userName, passWord, true);
				}

				m_Socket.OnOpen += OnOpenCallback;
				m_Socket.OnMessage += OnMessageCallback;
				m_Socket.OnError += OnErrorCallback;
				m_Socket.OnClose += OnCloseCallback;

				m_Socket.ConnectAsync ();
			}

			/// <summary>
			/// 连接到远程主机。
			/// </summary>
			/// <param name="url">Url地址。</param>
			public void Connect (string url, string userName = null, string passWord = null){
				if (string.IsNullOrEmpty (url)) {
					throw new GameFrameworkException ("WebSocket Uri cannot be null or empty.");
				}
				this.Connect (new Uri (url), userName, passWord);
			}

			/// <summary>
			/// 注册网络消息包处理函数。
			/// </summary>
			/// <param name="handler">要注册的网络消息包处理类。</param>
			public void RegisterHandler (IProtoHandler handler){
				if (handler == null)
				{
					throw new GameFrameworkException("Packet handler is invalid.");
				}

				this.RegisterHandler (handler.PacketId, handler.Handler);
			}

			/// <summary>
			/// 注册网络消息包处理函数。
			/// </summary>
			/// <param name="packetId">PacketId.</param>
			/// <param name="handler">Handler.</param>
			public void RegisterHandler(int packetId, EventHandler<GameFramework.Event.GameEventArgs> handler){
				if (packetId == (int)Protos.PacketType.None || packetId < 0) {
					throw new GameFrameworkException("Packet Id is invalid.");
				}

				if (handler == null) {
					throw new GameFrameworkException("Packet handler is invalid.");
				}

				GameEntry.Event.Subscribe (packetId, handler);
			}

			/// <summary>
			/// 取消注册包处理函数
			/// </summary>
			/// <param name="handler">要注册的网络消息包处理类.</param>
			public void UnRegisterHandler (IProtoHandler handler){
				if (handler == null)
				{
					throw new GameFrameworkException("Packet handler is invalid.");
				}

				this.UnRegisterHandler (handler.PacketId, handler.Handler);
			}

			/// <summary>
			/// 取消注册包处理函数
			/// </summary>
			/// <param name="packetId">PacketId.</param>
			/// <param name="handler">Handler.</param>
			public void UnRegisterHandler (int packetId, EventHandler<GameFramework.Event.GameEventArgs> handler){
				if (packetId == (int)Protos.PacketType.None || packetId < 0) {
					throw new GameFrameworkException("Packet Id is invalid.");
				}

				if (handler == null) {
					throw new GameFrameworkException("Packet handler is invalid.");
				}

				GameEntry.Event.Unsubscribe (packetId, handler);
			}

			public void Send(object packet, int packetId, Action<bool> onComplete = null){
				if (packet == null) {
					throw new GameFrameworkException ("Invalid Packet");
				}

				if (packetId == (int)Protos.PacketType.None) {
					throw new GameFrameworkException ("Invalid Packet Type");
				}
				try{
					
				}
				catch(Exception e){
					m_Active = false;
					if (NetworkChannelError != null)
					{
						NetworkChannelError(this, NetworkErrorCode.SerializeError, e.ToString());
						return;
					}
				}
				byte[] buffer = m_NetworkChannelHelper.Serialize (packet, packetId);

				Send (buffer, onComplete);
			}

			public void Send (byte[] buffer, Action<bool> onComplete = null){
				if (m_Socket == null)
				{
					throw new GameFrameworkException("You must connect first.");
				}

				if (buffer == null || buffer.Length == 0) {
					throw new GameFrameworkException ("Send Data Can't be null or empty.");
				}

				onComplete += b => {
					if (NetworkChannelSended != null) {
						NetworkChannelSended (this, buffer.Length, null);
					}
				};

				m_Socket.SendAsync (buffer, onComplete);
			}

			/// <summary>
			/// 释放资源。
			/// </summary>
			public void Dispose()
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}

			/// <summary>
			/// 释放资源。
			/// </summary>
			/// <param name="disposing">释放资源标记。</param>
			private void Dispose(bool disposing)
			{
				if (m_Disposed)
				{
					return;
				}

				if (disposing)
				{
					Close();
				}

				m_Disposed = true;
			}

			#endregion

			#region Event Callback

			private void OnOpenCallback(object sender, EventArgs e){
				if(this.NetworkChannelConnected != null){
					this.NetworkChannelConnected(this, e);
				}
			}

			private void OnMessageCallback(object sender, WebSocketSharp.MessageEventArgs e){
				this.ProcessReceivedData (e.RawData, e.Type);
			}

			private void OnErrorCallback(object sender, WebSocketSharp.ErrorEventArgs e){
				if(this.NetworkChannelError != null){
					this.NetworkChannelError(this, NetworkErrorCode.SocketError, e.Message);
				}
			}

			private void OnCloseCallback(object sender, WebSocketSharp.CloseEventArgs e){
				m_Socket.OnOpen -= OnOpenCallback;
				m_Socket.OnMessage -= OnMessageCallback;
				m_Socket.OnError -= OnErrorCallback;
				m_Socket.OnClose -= OnCloseCallback;

				if(this.NetworkChannelClosed != null){
					this.NetworkChannelClosed(this);
				}

				m_Socket = null;
			}

			#endregion

			private void ProcessReceivedData(byte[] data, WebSocketSharp.Opcode messageType) {

				lock (this) {
					this.m_heartbeatTime = 0f;
					this.m_missingHeartBeatCount = 0;
				}

				if (messageType == WebSocketSharp.Opcode.Close) {
					this.Close ();
					return;
				}

				if (messageType == WebSocketSharp.Opcode.Ping || messageType == WebSocketSharp.Opcode.Pong) {
					return;
				}

				MemoryStream stream = new MemoryStream (data);
				try{
					stream.Position = 0;
					int packetId;
					object packet = m_NetworkChannelHelper.DeserializePacket(stream, out packetId);//true to test
					if(packet != null){
						GameEntry.Event.Fire(this, ReferencePool.Acquire<WebSocketReceivedPacketEventArgs>().Fill(this, packetId, packet));	
					}
				}
				catch(Exception exception){
					m_Active = false;
					if (NetworkChannelError != null)
					{
						NetworkChannelError(this, NetworkErrorCode.DeserializePacketError, exception.ToString());
						return;
					}

					throw;
				}
				finally{
					stream.Dispose ();
				}
			}

			/// <summary>
			/// 关闭网络频道。
			/// </summary>
			public void Shutdown()
			{
				Close();
				m_NetworkChannelHelper.Shutdown();
			}
		}
	}
}
