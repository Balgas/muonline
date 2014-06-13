
using UnityEngine;
using System.Collections;

namespace Effect {
	
	public class MoveTarget : MonoBehaviour {
		
		Transform Circle;
		Transform Arrow;
		Transform Arrow2;
		Transform SMD;
		
		IEnumerator Start () {
			gameObject.isStatic = true;
			
			
			SMD = (Transform)gameObject.transform.Find("SMDImport");
			
			GameObject obj = Util.Storage.LoadPrefab("Effect/MoveTargetCircle");
			
			if (obj!=null) {
				GameObject SubObject = (GameObject)Instantiate(obj, Vector3.zero, Quaternion.identity);
				Util.GO.SetParent ( SubObject.transform, gameObject.transform );
				SubObject.name = "SubObject";
				
				Circle = (Transform)SubObject.transform.Find("Circle");
				Arrow = (Transform)SubObject.transform.Find("Arrow");
				Arrow2 = (Transform)SubObject.transform.Find("Arrow2");
				
			}
			yield return StartCoroutine("FadeAll");
			Destroy(gameObject);
			
		}
		
		void Update () {
			if (Circle!=null)
				Circle.Rotate(Vector3.back * Time.deltaTime * 50f);
			
			if (Arrow!=null) {
				float scale = 0.5f+Mathf.PingPong(Time.time*3f, 0.55f);
				Arrow.localScale = new Vector3(scale, scale, scale);
			}
			
			if (Arrow2!=null) {
				float scale2 = 0.5f+Mathf.PingPong(Time.time*2f, 0.55f);
				Arrow2.localScale = new Vector3(scale2, scale2, scale2);
			}
		}
		
		IEnumerator FadeAll() {
			yield return new WaitForSeconds(1.1f);
			yield return StartCoroutine("FadeOut", Circle);
			Destroy(Circle.gameObject);
			
			yield return new WaitForSeconds(0.2f);
			yield return StartCoroutine("FadeOut", Arrow);
			Destroy(Arrow.gameObject);
			
			yield return new WaitForSeconds(0.1f);
			yield return StartCoroutine("FadeOut", Arrow2);
			Destroy(Arrow2.gameObject);
			
			yield return new WaitForSeconds(0.1f);
			yield return StartCoroutine("FadeOut", SMD);
			yield break;
		}
		
		IEnumerator FadeOut(Transform obj) {
			Color c = obj.renderer.material.color;
			for (float a = 1f; a>=0f; a-=0.1f) {
				c.a = a;
				obj.renderer.material.color = c;
				yield return 1;
			}
			c.a = 0f;
			obj.renderer.material.color = c;
			yield break;
		}
		
	}
	
}
