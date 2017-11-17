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

		private static void InitCustomComponents()
		{
			Config = UnityGameFramework.Runtime.GameEntry.GetComponent<ConfigComponent> ();
			HPBar = UnityGameFramework.Runtime.GameEntry.GetComponent<HPBarComponent> ();
			_coroutine = UnityGameFramework.Runtime.GameEntry.GetComponent<CoroutineComponent> ();
		}
	}
}
