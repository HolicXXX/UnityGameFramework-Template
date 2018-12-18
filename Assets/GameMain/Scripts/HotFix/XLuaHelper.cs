using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain {
	public class XLuaHelper : IHotFixHelper
	{
		public Object Main {
			get;
			private set;
		}

		public XLuaHelper () 
		{
			_enable = false;
			Main = null;
		}

		#region Implement
		bool _enable = false;
		public bool Enable {
			get {
				return _enable && Main != null;
			}
		}

		public void Initialize ()
		{
			//TODO
			//Main = new LuaEnv ();

			_enable = true;
		}

		public void Update (float elapsedTime, float unscaledElapsedTime)
		{
			//Main.Tick ();
		}

		public void ShutDown ()
		{
			_enable = false;
			//Main.Dispose (true);
			Main = null;
		}
		#endregion

	}
}
