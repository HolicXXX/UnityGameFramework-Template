using GameFramework;
using GameFramework.Fsm;
using System;

namespace GameMain {
	
	/// <summary>
	/// 角色移动状态管理
	/// </summary>
	public partial class CharacterMotion : ICharacterManager {
		
		private IFsmManager m_FsmManager;
		private IFsm<ICharacterManager> m_StateFsm;

		public CharacterMotion(){
			m_FsmManager = null;
			m_StateFsm = null;
		}

		#region Implement

		/// <summary>
		/// 获取或设置该状态机的宿主
		/// </summary>
		public CharacterControllerBase Owner {
			get;
			private set;
		}

		/// <summary>
		/// 获取当前流程
		/// </summary>
		public CharacterStateBase CurrentState{
			get
			{
				if (m_StateFsm == null) {
					throw new GameFrameworkException ("You must initialize states first");
				}

				return (CharacterStateBase)m_StateFsm.CurrentState;
			}
		}

		/// <summary>
		/// 获取当前状态持续时间
		/// </summary>
		public float CurrentStateTime{
			get
			{
				if (m_StateFsm == null) {
					throw new GameFrameworkException ("You must initialize states first");
				}

				return m_StateFsm.CurrentStateTime;
			}
		}

		/// <summary>
		/// 获取或设置当前状态机是否可用
		/// </summary>
		public bool Enable {
			get;
			set;
		}

		/// <summary>
		/// 状态管理器轮询
		/// </summary>
		/// <param name="elapseSeconds">逻辑流逝时间，单位秒.</param>
		/// <param name="realElapseSeconds">实际流逝时间，单位秒.</param>
		public void Update(float elapseSeconds, float realElapseSeconds) {
			
		}

		/// <summary>
		/// 关闭并清理状态管理器
		/// </summary>
		public void ShutDown() {
			if (m_FsmManager != null) {
				if (m_StateFsm != null) {
					m_FsmManager.DestroyFsm (m_StateFsm);
					m_StateFsm = null;
				}

				m_FsmManager = null;
			}
		}

		/// <summary>
		/// 初始化状态管理器
		/// </summary>
		/// <param name="fsmManager">有限状态机管理器.</param>
		/// <param name="states">状态管理器所包含的状态.</param>
		public void Initialize(IFsmManager fsmManager,CharacterControllerBase owner, params CharacterStateBase[] states){
			if (fsmManager == null) {
				throw new GameFrameworkException("FSM manager is invalid.");
			}

			if (owner == null) {
				throw new GameFrameworkException("FSM Owner is invalid.");
			}

			this.Owner = owner;

			m_FsmManager = fsmManager;
			m_StateFsm = m_FsmManager.CreateFsm(this, states);
		}

		/// <summary>
		/// 开始状态
		/// </summary>
		/// <typeparam name="T">要开始的状态类型.</typeparam>
		public void StartState<T>() where T : CharacterStateBase{
			if (m_StateFsm == null) {
				throw new GameFrameworkException("You must initialize states first.");
			}

			m_StateFsm.Start<T> ();
		}

		/// <summary>
		/// 开始状态
		/// </summary>
		/// <param name="stateType">要开始的状态类型.</param>
		public void StartState(Type stateType){
			if (m_StateFsm == null) {
				throw new GameFrameworkException("You must initialize states first.");
			}

			m_StateFsm.Start (stateType);
		}

		/// <summary>
		/// 是否存在状态
		/// </summary>
		/// <typeparam name="T">要检查的状态类型.</typeparam>
		public bool HasState<T>() where T : CharacterStateBase{
			if (m_StateFsm == null) {
				throw new GameFrameworkException("You must initialize states first.");
			}

			return m_StateFsm.HasState<T> ();
		}

		/// <summary>
		/// 是否存在状态
		/// </summary>
		/// <param name="stateType">State type.</param>
		public bool HasState(Type stateType){
			if (m_StateFsm == null) {
				throw new GameFrameworkException("You must initialize states first.");
			}

			return m_StateFsm.HasState (stateType);
		}

		/// <summary>
		/// 获取状态
		/// </summary>
		/// <returns>获取的状态.</returns>
		/// <typeparam name="T">要获取的状态类型.</typeparam>
		public CharacterStateBase GetState<T>() where T : CharacterStateBase{
			if (m_StateFsm == null) {
				throw new GameFrameworkException("You must initialize states first.");
			}

			return m_StateFsm.GetState<T> ();
		}

		/// <summary>
		/// 获取状态
		/// </summary>
		/// <returns>获取的状态.</returns>
		/// <param name="stateType">要获取的状态类型.</param>
		public CharacterStateBase GetState(Type stateType){
			if (m_StateFsm == null) {
				throw new GameFrameworkException("You must initialize states first.");
			}

			return (CharacterStateBase)m_StateFsm.GetState (stateType);
		}

		#endregion


		#region Custom


		#endregion
	}
}
