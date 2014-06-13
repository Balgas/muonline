
using UnityEngine;
using System.Collections;

namespace MuMap.AI {
	public class Crater : SMDObject {
	
		void Update () {
			
			transform.Rotate(Vector3.down * Time.deltaTime * 100 * Config.AnimationSpeedWorldObjects);
			
		}
	}
}