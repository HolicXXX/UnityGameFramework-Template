using GameFramework.Event;
using UnityEngine;

namespace GameMain {
	public class WebSocketReceivedPacketEventArgs : GameEventArgs {

		private int _id = (int)Protos.PacketType.None;

		public override int Id {
			get{ 
				return _id;
			}
		}

		public IWebSocketChannel Channel {
			get;
			private set;
		}

		public object Packet {
			get;
			private set;
		}

		public override void Clear ()
		{
			_id = (int)Protos.PacketType.None;
			Channel = default(IWebSocketChannel);
			Packet = default(object);
		}

		public WebSocketReceivedPacketEventArgs Fill(IWebSocketChannel channel, int id, object packet){
			Channel = channel;
			_id = id;
			Packet = packet;
			return this;
		}

	}
}
