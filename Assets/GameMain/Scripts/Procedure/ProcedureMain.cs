using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain {
	public class ProcedureMain : ProcedureBase {
		public override bool UseNativeDialog {
			get {
				return false;
			}
		}

		protected override void OnInit (ProcedureOwner procedureOwner)
		{
			base.OnInit (procedureOwner);
		}

		protected override void OnEnter (ProcedureOwner procedureOwner)
		{
			base.OnEnter (procedureOwner);
		}

		protected override void OnUpdate (ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate (procedureOwner, elapseSeconds, realElapseSeconds);
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
