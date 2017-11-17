using GameFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using MEC;

namespace GameMain {
	public class CoroutineComponent : GameFrameworkComponent {

		[SerializeField]
		private Timing _instance;
		public Timing Instance
		{
			get{ return this._instance; }
		}

		protected override void Awake ()
		{
			base.Awake ();
		}

		private void Start(){
			
		}

		private void OnDestroy(){
		
		}

		private void Update(){
			
		}
	}
}
