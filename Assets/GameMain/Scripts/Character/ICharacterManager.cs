using GameFramework.Fsm;
using System;

namespace GameMain {
	public interface ICharacterManager {

		/// <summary>
		/// 获取或设置该状态机的宿主
		/// </summary>
		CharacterControllerBase Owner
		{
			get;
		}

		/// <summary>
		/// 获取当前流程
		/// </summary>
		CharacterStateBase CurrentState
		{
			get;
		}

		/// <summary>
		/// 获取当前状态持续时间
		/// </summary>
		float CurrentStateTime
		{
			get;
		}

		/// <summary>
		/// 获取或设置当前状态机是否可用
		/// </summary>
		bool Enable
		{
			get;
			set;
		}

		/// <summary>
		/// 状态管理器轮询
		/// </summary>
		/// <param name="elapseSeconds">逻辑流逝时间，单位秒.</param>
		/// <param name="realElapseSeconds">实际流逝时间，单位秒.</param>
		void Update(float elapseSeconds, float realElapseSeconds);

		/// <summary>
		/// 关闭并清理状态管理器
		/// </summary>
		void ShutDown();

		/// <summary>
		/// 初始化状态管理器
		/// </summary>
		/// <param name="fsmManager">有限状态机管理器.</param>
		/// <param name="states">状态管理器所包含的状态.</param>
		void Initialize(IFsmManager fsmManager, CharacterControllerBase owner, params CharacterStateBase[] states);

		/// <summary>
		/// 开始状态
		/// </summary>
		/// <typeparam name="T">要开始的状态类型.</typeparam>
		void StartState<T> () where T : CharacterStateBase;

		/// <summary>
		/// 开始状态
		/// </summary>
		/// <param name="stateType">要开始的状态类型.</param>
		void StartState(Type stateType);

		/// <summary>
		/// 是否存在状态
		/// </summary>
		/// <typeparam name="T">要检查的状态类型.</typeparam>
		bool HasState<T>() where T : CharacterStateBase;

		/// <summary>
		/// 是否存在状态
		/// </summary>
		/// <param name="stateType"要检查的状态类型</param>
		bool HasState(Type stateType);

		/// <summary>
		/// 获取状态
		/// </summary>
		/// <returns>获取的状态.</returns>
		/// <typeparam name="T">要获取的状态类型.</typeparam>
		CharacterStateBase GetState<T>() where T : CharacterStateBase;

		/// <summary>
		/// 获取状态
		/// </summary>
		/// <returns>获取的状态.</returns>
		/// <param name="stateType">要获取的状态类型.</param>
		CharacterStateBase GetState(Type stateType);
	}
}
