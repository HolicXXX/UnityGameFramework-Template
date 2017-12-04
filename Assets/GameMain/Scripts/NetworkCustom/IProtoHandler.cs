using GameFramework.Event;

namespace GameMain {
	public interface IProtoHandler {
		int PacketId{ get;}
		void Handler (object sender, GameEventArgs packet);
	}
}
