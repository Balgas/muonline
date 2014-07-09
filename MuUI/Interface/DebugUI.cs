
using UnityEngine;
using System.Collections;

namespace MuUI {
	
	public class DebugUI : UIComponent {
		
		ArrayList logs = new ArrayList();
		
		public void Log(string text) {
			logs.Add(text);
		}
		
		
		void OnGUI() {
			GUILayout.BeginVertical();
			foreach (string val in logs) {
				GUILayout.Label(val);
			}
			GUILayout.EndVertical();
		}
		
		
	}

}
