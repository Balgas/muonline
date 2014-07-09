
using UnityEngine;
using System.Collections;

namespace MuUI {
	public class MapLogo : UIComponent {
		
		public void Load(string res) {
			ignoreRaycast = false;
			rect = UIRect.ScreenCenterMiddle(128f, 69f);
			Add ("logo", new Image(Config.DIRECTORY_INTERFACE+Config.DIRECTORY_MAPSLOGO+res));
			StartCoroutine("Timeout");
		}
		
		IEnumerator Timeout() {
			alpha = 0f;
			yield return StartCoroutine(FadeIn(0.03f));
			yield return new WaitForSeconds(Config.WaitLogoMap);
			yield return StartCoroutine(FadeOut(0.03f));
			remove();
	    }
		
		
	}
}
