using GameFramework.Event;
using UnityEngine;

namespace GameMain {
	public class WebSocketConnectedEventArgs : GameEventArgs {
		public static readonly int EventId = typeof(WebSocketConnectedEventArgs).GetHashCode();

		public override int Id {
			get {
				return EventId;
			}
		}

		public IWebSocketChannel Channel {
			get;
			private set;
		}

		public object UserData {
			get;
			private set;
		}

		public override void Clear ()
		{
			Channel = default(IWebSocketChannel);
			UserData = default(object);
		}

		public WebSocketConnectedEventArgs Fill(IWebSocketChannel channel, object userData){
			Channel = channel;
			UserData = userData;

			return this;
		}
	}
}
