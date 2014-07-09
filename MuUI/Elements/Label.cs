using UnityEngine;
using System.Collections;

namespace MuUI {
	public class Label : Element, IElement  {
		
		public Label(string value = "") {
			text = value;
		}
		
		public void Init() {
			style.SetColor(UIConfig.textColor);
		}
		
		public string text {
			set; get;	
		}
		
		public void Show() {
			GUI.Label(rect, text, style);
		}
		
    }
 
}
