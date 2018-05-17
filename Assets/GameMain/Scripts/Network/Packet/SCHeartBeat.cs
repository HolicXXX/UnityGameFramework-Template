using ProtoBuf;
using System;

namespace GameMain
{
    [Serializable, ProtoContract(Name = @"SCHeartBeat")]
    public partial class SCHeartBeat : SCPacketBase
    {
		public static readonly int EventId = typeof(SCHeartBeat).GetHashCode();

        public SCHeartBeat()
        {

        }

        public override int Id
        {
            get
            {
				return EventId;
            }
        }

        public override void Clear()
        {

        }
    }
}
