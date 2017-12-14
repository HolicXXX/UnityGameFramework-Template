using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace GameMain {
	public class XLuaHelper : IHotFixHelper
	{
		public LuaEnv Main {
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
			Main = new LuaEnv ();
			//TODO

			_enable = true;
		}

		public void Update (float elapsedTime, float unscaledElapsedTime)
		{
			Main.Tick ();
		}

		public void ShutDown ()
		{
			_enable = false;
			Main.Dispose (true);
			Main = null;
		}
		#endregion

	}
}
