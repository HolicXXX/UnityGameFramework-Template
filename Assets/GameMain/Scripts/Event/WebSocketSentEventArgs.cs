using GameFramework.Event;
using UnityEngine;

namespace GameMain {
	public class WebSocketSentEventArgs : GameEventArgs {
		public static readonly int EventId = typeof(WebSocketSentEventArgs).GetHashCode();

		public override int Id {
			get {
				return EventId;
			}
		}

		public IWebSocketChannel Channel {
			get;
			private set;
		}

		public int BytesSent {
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
			BytesSent = default(int);
			UserData = default(object);
		}

		public WebSocketSentEventArgs Fill(IWebSocketChannel channel, int sentNum, object userData){
			Channel = channel;
			BytesSent = sentNum;
			UserData = userData;

			return this;
		}

	}
}
