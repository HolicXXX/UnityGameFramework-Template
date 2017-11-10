using GameFramework.Network;
using ProtoBuf;

namespace GameMain
{
    [ProtoContract]
    public class SCPacketHeader : PacketHeaderBase, IPacketHeader
    {
        public SCPacketHeader(int packetId)
            : base(PacketType.ServerToClient, packetId)
        {

        }

        public int PacketLength
        {
            get;
            set;
        }
    }
}
