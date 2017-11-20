using GameFramework;
using GameFramework.Fsm;
using StateOwner = GameFramework.Fsm.IFsm<GameMain.ICharacterManager>;
using UnityEngine;

namespace GameMain {
	public partial class CharacterMotion {

		public class IdleState : CharacterStateBase{

			#region Implement

			/// <summary>
			/// 状态初始化时调用。
			/// </summary>
			/// <param name="procedureOwner">状态持有者。</param>
			protected override void OnInit(StateOwner stateOwner)
			{
				base.OnInit(stateOwner);
			}

			/// <summary>
			/// 进入状态时调用。
			/// </summary>
			/// <param name="procedureOwner">状态持有者。</param>
			protected override void OnEnter(StateOwner stateOwner)
			{
				base.OnEnter(stateOwner);
			}

			/// <summary>
			/// 状态轮询时调用。
			/// </summary>
			/// <param name="procedureOwner">状态持有者。</param>
			/// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
			/// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
			protected override void OnUpdate(StateOwner stateOwner, float elapseSeconds, float realElapseSeconds)
			{
				base.OnUpdate(stateOwner, elapseSeconds, realElapseSeconds);
			}

			/// <summary>
			/// 离开状态时调用。
			/// </summary>
			/// <param name="procedureOwner">状态持有者。</param>
			/// <param name="isShutdown">是否是关闭状态机时触发。</param>
			protected override void OnLeave(StateOwner stateOwner, bool isShutdown)
			{
				base.OnLeave(stateOwner, isShutdown);
			}

			/// <summary>
			/// 状态销毁时调用。
			/// </summary>
			/// <param name="procedureOwner">状态持有者。</param>
			protected override void OnDestroy(StateOwner stateOwner)
			{
				base.OnDestroy(stateOwner);
			}

			#endregion
		}

	}
}
