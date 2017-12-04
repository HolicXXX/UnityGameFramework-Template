using GameFramework;
using GameFramework.Event;

namespace GameMain {
	public class HeartBeatHandler : ProtoHandlerBase {
		public override int PacketId {
			get {
				return (int)Protos.PacketType.HeartBeat;
			}
		}

		public override void Handler (object sender, GameEventArgs args)
		{
			WebSocketReceivedPacketEventArgs wsargs = args as WebSocketReceivedPacketEventArgs;
			Protos.HeartBeat_Result packet = wsargs.Packet as Protos.HeartBeat_Result;

			Log.Info ("HeartBeat Received. {0}", packet);
		}
	}
}
