
namespace GameMain
{
    public class SCPacketHeader : PacketHeaderBase
    {
		public override PacketType PacketType {
			get {
				return PacketType.ServerToClient;
			}
        }
    }
}
