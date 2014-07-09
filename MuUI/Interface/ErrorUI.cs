
using UnityEngine;
using System.Collections;

namespace MuUI {
	
	public class ErrorUI : UIComponent {
		
		private Label	label	= new Label() { top = 25f, left = 20f, right = 20f };
		private Button	close	= new Button(MuGlobal.GlobalMethods.ButtonPath("button_close")) { top = 75f, left = 150f };
		
		void Awake () {
			rect = UIRect.ScreenCenterMiddle(352f, 113f);
			
			close.texture.countY = 3;
			
			Add ( "back", new Image(Config.DIRECTORY_INTERFACE+"Backgrounds/message_back") );
			Add ( "label", label );
			Add ( "close", close );
			
			close.Click += remove;
			
			label.style.SetWrap		(true);
			label.style.SetAlign	(TextAnchor.UpperCenter);
		}
		
		public void SetText(string text) {
			label.text = text;
		}
		
		
	}

}
