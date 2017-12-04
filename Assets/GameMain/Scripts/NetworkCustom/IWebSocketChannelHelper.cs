using System;
using System.IO;

namespace GameMain {
	public interface IWebSocketChannelHelper {
		
		int PacketHeaderLength {
			get;
		}

		byte[] Serialize (object packet, int packetId);

		object DeserializePacket (Stream source, out int packetId, bool request = false);

		void Initialize (IWebSocketChannel networkChannel);

		void Shutdown ();

		bool SendHeartBeat();
	}
}
