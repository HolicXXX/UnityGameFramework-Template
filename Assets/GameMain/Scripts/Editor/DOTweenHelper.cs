using UnityEngine;
using UnityEditor;

namespace GameMain.Editor {
	public class DOTweenHelper {
		
		[MenuItem("Tools/Demigiant/DOTween Document", false)]
		public static void OpenDocumentUrl(){
			Application.OpenURL ("http://dotween.demigiant.com/documentation.php");
		}

	}
}
