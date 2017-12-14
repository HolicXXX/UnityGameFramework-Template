using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;

namespace GameMain
{
	[XLua.LuaCallCSharp]
	public class HotFixComponent : GameFrameworkComponent
	{
		
		IHotFixHelper m_hotfixHelper;
		public IHotFixHelper HotFixHelper 
		{
			get {
				return m_hotfixHelper;
			}
		}

		public bool Enable
		{
			get {
				return m_hotfixHelper != null && m_hotfixHelper.Enable;
			}
		}

		protected override void Awake ()
		{
			base.Awake ();

			m_hotfixHelper = null;
		}

		public void SetHelper (IHotFixHelper helper)
		{
			if (helper == null) {
				Log.Error ("Can not set HotFixHelper with null");
				return;
			}

			if (m_hotfixHelper != null) {
				m_hotfixHelper.ShutDown ();
			}
			m_hotfixHelper = helper;
			m_hotfixHelper.Initialize ();
		}

		public void ShutDown () 
		{
			if (m_hotfixHelper != null) {
				m_hotfixHelper.ShutDown ();
			}
			m_hotfixHelper = null;
		}

		void Start ()
		{
			////TestCode
			//SetHelper (new XLuaHelper ());
			//XLua.LuaEnv env = (m_hotfixHelper as XLuaHelper).Main;
			//env.DoString ("CS.UnityEngine.Debug.Log('WTF')");
		}


		void Update ()
		{
			if (Enable) {
				m_hotfixHelper.Update (Time.deltaTime, Time.unscaledDeltaTime);
			}
		}

		void OnDestroy ()
		{
			ShutDown ();
		}
	}
}
