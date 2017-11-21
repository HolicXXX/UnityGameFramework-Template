using GameFramework.Event;
using UnityEngine;

namespace GameMain {
	public class CharacterMotionEventArgs : GameEventArgs {

		public static readonly int EventId = typeof(CharacterMotionEventArgs).GetHashCode();

		public override int Id {
			get {
				return EventId;
			}
		}

		/// <summary>
		/// 移动的方向
		/// </summary>
		public Vector3 Direction {
			get;
			private set;
		}

		/// <summary>
		/// 面朝的方向
		/// </summary>
		public Vector3 LookDirection {
			get;
			private set;
		}

		/// <summary>
		/// 移动的速度
		/// </summary>
		public float Speed {
			get;
			private set;
		}

		public override void Clear ()
		{
			Direction = default(Vector3);
			LookDirection = default(Vector3);
			Speed = default(float);
		}

		/// <summary>
		/// 填充角色移动事件
		/// </summary>
		/// <param name="direction">移动的方向.</param>
		/// <param name="lookDirection">移动时面朝的方向.</param>
		/// <param name="speed">移动的速度.</param>
		public CharacterMotionEventArgs Fill(Vector3 direction, Vector3 lookDirection, float speed){

			Direction = direction;
			LookDirection = lookDirection;
			Speed = speed;

			return this;
		}
	}
}
