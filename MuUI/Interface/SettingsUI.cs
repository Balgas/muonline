using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MuUI {
	public class SettingsUI : UIComponent {
		
		public void SetSettings (Dictionary<string, string> settings) {
			rect = UIRect.ScreenCenterMiddle(190f, 300f);
			
			Add ( "back", new Image (Config.DIRECTORY_INTERFACE+"Backgrounds/settings") );
			Add ( "info", new Label() { left = 20, right = 20, top = 230 } );
			
			int Y = 40;
			foreach (KeyValuePair<string, string> setting in settings) {
				if (setting.Value=="checkbox") {
					string name = setting.Key;
					Label label = new Label(Lang.str ("Settings"+name)){ left = 20, top = Y };
					Checkbox box = new Checkbox(Config.DIRECTORY_INTERFACE+"Elements/Checkbox"){ left = 155, top = Y };
					box.texture.countY = 2;
					Add ("label_"+name, label);
					Add (name, box);
					Y+=25;
				}
			}
			
			info.style.SetAlign	(TextAnchor.UpperCenter);
			info.style.SetWrap	(true);
			
		}
		
		public void SetInfo(string code) {
			info.text = Lang.str(code);
		}
		
		Label info {
			get { return (Label)GetElement("info") as Label; }
		}
		
		
	}
}
