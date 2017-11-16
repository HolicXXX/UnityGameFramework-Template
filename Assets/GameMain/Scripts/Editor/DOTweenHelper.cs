using UnityEngine;
using UnityEditor;

namespace GameMain.Editor {
	public class DOTweenHelper {
		
		[MenuItem("Game Main/DOTween Document", false, 10)]
		public static void OpenDocumentUrl(){
			Application.OpenURL ("http://dotween.demigiant.com/documentation.php");
		}

	}
}
