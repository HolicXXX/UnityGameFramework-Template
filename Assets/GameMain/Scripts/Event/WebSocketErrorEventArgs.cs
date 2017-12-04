using GameFramework.Event;
using GameFramework.Network;
using UnityEngine;

namespace GameMain {
	public class WebSocketErrorEventArgs : GameEventArgs {

		public static readonly int EventId = typeof(WebSocketErrorEventArgs).GetHashCode();

		public override int Id {
			get {
				return EventId;
			}
		}

		public IWebSocketChannel Channel {
			get;
			private set;
		}

		public NetworkErrorCode ErrorCode {
			get;
			private set;
		}

		public string ErrorMessage {
			get;
			private set;
		}

		public override void Clear ()
		{
			Channel = default(IWebSocketChannel);
			ErrorCode = default(NetworkErrorCode);
			ErrorMessage = default(string);
		}

		public WebSocketErrorEventArgs Fill(IWebSocketChannel channel, NetworkErrorCode code, string msg){

			Channel = channel;
			ErrorCode = code;
			ErrorMessage = msg;

			return this;
		}

	}
}
