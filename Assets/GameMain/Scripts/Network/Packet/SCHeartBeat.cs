using ProtoBuf;
using System;

namespace GameMain
{
    [Serializable, ProtoContract(Name = @"SCHeartBeat")]
    public partial class SCHeartBeat : ServerToClientPacketBase
    {
        public SCHeartBeat()
        {

        }

        public override int PacketId
        {
            get
            {
                return 1;
            }
        }

        public override void Clear()
        {

        }
    }
}
