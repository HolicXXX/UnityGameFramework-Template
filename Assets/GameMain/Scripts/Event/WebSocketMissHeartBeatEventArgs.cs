using GameFramework.Event;
using UnityEngine;

namespace GameMain {
	public class WebSocketMissHeartBeatEventArgs : GameEventArgs {
		public static readonly int EventId = typeof(WebSocketMissHeartBeatEventArgs).GetHashCode();

		public override int Id {
			get {
				return EventId;
			}
		}

		public IWebSocketChannel Channel {
			get;
			private set;
		}

		public int MissCount {
			get;
			private set;
		}

		public override void Clear ()
		{
			Channel = default(IWebSocketChannel);
			MissCount = default(int);
		}

		public WebSocketMissHeartBeatEventArgs Fill(IWebSocketChannel channel, int count){
			Channel = channel;
			MissCount = count;

			return this;
		}
	}
}
