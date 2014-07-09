
using UnityEngine;
using System.Collections;

namespace MuUI {
	public static class UIStyle {
	
		//цвет
		public static void SetColor(this GUIStyle style, Color color) {
			style.normal.textColor = color;
		}
		
		//выравнивание
		public static void SetAlign(this GUIStyle style, TextAnchor align) {
			style.alignment = TextAnchor.UpperCenter;
		}
		
		//перенос строк
		public static void SetWrap(this GUIStyle style, bool wrap) {
			style.wordWrap = wrap;
		}
		
		public static void NewFont(this GUIStyle style) {
			Font font = (MuGlobal.Global.instance!=null ? MuGlobal.Global.instance.font : new Font("ErrorMuFont"));
			style.font = font;
		}
		
	}
}
