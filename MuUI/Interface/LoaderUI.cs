
using UnityEngine;
using System.Collections;

namespace MuUI {
	
	public class LoaderUI : UIComponent {
		
		public int MaxParts = 1;
		public int CurrentPart = 0;
		public float Progress = (float)0;
		
		public void SetProgress(float progress) {
			Progress = progress;
		}
		
		public void SetMaxParts(int parts) {
			MaxParts = parts;
		}
		
		public void SetPart(int part) {
			CurrentPart = part; 
		}
		
		void OnGUI() { 
			GUI.BeginGroup (UIRect.ScreenCenterMiddle(150, 80));
			GUI.Box (new Rect(0, 0, 150, 40), Lang.str("Loading"));
			GUI.Box (new Rect(0, 40, 150, 40), GetPercent().ToString()+'%');
			GUI.EndGroup ();
		}
		
		float GetPercent() {
			float percentParts = (float)CurrentPart/MaxParts;
			float percentProgress = (float)Progress/1;
			float oneParts = (float)1/MaxParts; 
			return Mathf.Round((percentParts + (percentProgress*oneParts))*100);
		}
		
	}

}
