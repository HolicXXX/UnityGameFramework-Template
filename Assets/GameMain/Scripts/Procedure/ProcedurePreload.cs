using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain {
	public class ProcedurePreload : ProcedureBase {
		public override bool UseNativeDialog {
			get {
				return true;
			}
		}

		protected override void OnInit (ProcedureOwner procedureOwner)
		{
			base.OnInit (procedureOwner);
		}

		protected override void OnEnter (ProcedureOwner procedureOwner)
		{
			base.OnEnter (procedureOwner);

			//Preload DataTables, Dictionary and Fonts
		}

		protected override void OnUpdate (ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate (procedureOwner, elapseSeconds, realElapseSeconds);

			ChangeState<ProcedureChangeScene> (procedureOwner);
		}

		protected override void OnLeave (ProcedureOwner procedureOwner, bool isShutdown)
		{
			base.OnLeave (procedureOwner, isShutdown);
		}

		protected override void OnDestroy (ProcedureOwner procedureOwner)
		{
			base.OnDestroy (procedureOwner);
		}
	}
}
