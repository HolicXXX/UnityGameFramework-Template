
namespace GameMain
{
    public class CSPacketHeader : PacketHeaderBase
    {
		public override PacketType PacketType {
			get
			{
				return PacketType.ClientToServer;
			}
        }
    }
}
