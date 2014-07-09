using UnityEngine;
using System.Collections;

namespace MuUI {
	
	public class UIEffect : MonoBehaviour {
		
		protected float alpha = 1f;
		protected Color color = new Color(1f, 1f, 1f, 1f);
		
		
		protected IEnumerator FadeIn(float speed) {
			while (alpha<1) {
				alpha += speed;
				yield return 1;
			}
			yield break;
		}
		
		protected IEnumerator FadeOut(float speed) {
			while (alpha>0) {
				alpha -= speed;
				yield return 1;
			}
			yield break;
		}
		
		protected void ApplyColor() {
			color.a = alpha;
			GUI.color = color;	
		}
		
		
		
	}
	
}
