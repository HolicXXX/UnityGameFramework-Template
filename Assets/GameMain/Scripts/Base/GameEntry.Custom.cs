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

		private static void InitCustomComponents()
		{
			Config = UnityGameFramework.Runtime.GameEntry.GetComponent<ConfigComponent>();
			HPBar = UnityGameFramework.Runtime.GameEntry.GetComponent<HPBarComponent>();
		}
	}
}
