using ProtoBuf;
using System;

namespace GameMain
{
    [Serializable, ProtoContract(Name = @"CSHeartBeat")]
    public partial class CSHeartBeat : CSPacketBase
    {
		public static readonly int EventId = typeof(CSHeartBeat).GetHashCode();

        public CSHeartBeat()
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
