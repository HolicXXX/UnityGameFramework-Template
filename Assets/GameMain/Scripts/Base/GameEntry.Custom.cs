using UnityGameFramework.Runtime;

namespace GameMain {
	public partial class GameEntry {
		//This is where custom component lists
		public static ConfigComponent Config
		{
			get;
			private set;
		}

		public static HPBarComponent HPBar
		{
			get;
			private set;
		}

		private static CoroutineComponent _coroutine;

		public static MEC.Timing Coroutine
		{
			get{ return _coroutine.Instance; }
		}

		public static CharacterComponent Character
		{
			get;
			private set;
		}

		public static WebsocketNetworkComponent WS 
		{
			get;
			private set;
		}

		public static HotFixComponent HotFix 
		{
			get;
			private set;
		}

		private static void InitCustomComponents()
		{
			Config = UnityGameFramework.Runtime.GameEntry.GetComponent<ConfigComponent> ();
			HPBar = UnityGameFramework.Runtime.GameEntry.GetComponent<HPBarComponent> ();
			_coroutine = UnityGameFramework.Runtime.GameEntry.GetComponent<CoroutineComponent> ();
			Character = UnityGameFramework.Runtime.GameEntry.GetComponent<CharacterComponent> ();
			WS = UnityGameFramework.Runtime.GameEntry.GetComponent<WebsocketNetworkComponent> ();
			HotFix = UnityGameFramework.Runtime.GameEntry.GetComponent<HotFixComponent> ();
		}
	}
}
