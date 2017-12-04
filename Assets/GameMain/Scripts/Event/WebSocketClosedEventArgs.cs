using GameFramework.Event;
using UnityEngine;

namespace GameMain {
	public class WebSocketClosedEventArgs : GameEventArgs {
		public static readonly int EventId = typeof(WebSocketClosedEventArgs).GetHashCode();

		public override int Id {
			get {
				return EventId;
			}
		}

		public IWebSocketChannel Channel {
			get;
			private set;
		}

		public override void Clear ()
		{
			Channel = default(IWebSocketChannel);
		}

		public WebSocketClosedEventArgs Fill(IWebSocketChannel channel){

			Channel = channel;

			return this;
		}
	}
}
