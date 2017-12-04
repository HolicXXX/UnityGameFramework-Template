using GameFramework.Event;

namespace GameMain {
	public abstract class ProtoHandlerBase : IProtoHandler {
		
		public virtual int PacketId {
			get{ 
				return (int)Protos.PacketType.None;
			}
		}

		public virtual void Handler(object sender, GameEventArgs packet){
			
		}
	}
}
