
/* Слежение камеры за игроком
 * 
 * 
 */

using UnityEngine;
using System.Collections;

namespace MuPlayer {
	
	public class PlayerCamera : MonoBehaviour {
	
		Camera MainCamera;
		
		public void Init() {
			MainCamera	= Camera.main;
		}
		
		public void Apply() {
			
			MainCamera.transform.parent = gameObject.transform;
			MainCamera.transform.localPosition = Config.CameraDistancePlayer;
			MainCamera.transform.LookAt(MainCamera.transform.parent);
			
		}
		
		public void ChangeCoords(PlayerData Data) {
			
		}
		
	}

}