
using UnityEngine;
using System.Collections;

namespace MuMap.AI {
	public class GateDoor : Interaction {
		
		float _distance = 300f;
		float _speed = 10f;
		float _rotateY = 180f;
		Quaternion closedRotate;
		Quaternion need;
		
		IEnumerator Start () {
			closedRotate = need = transform.localRotation;
			
			yield return StartCoroutine("SearchMe");
			
			if (objects!=null && me!=null) 
				me.EventStartMoveTo += StartMoveTo;
			
			
			yield break;
		}
		
		void StartMoveTo(MuPlayer.PlayerData Data) {
			float dist = Distance();
			if (dist<_distance) need = Quaternion.Euler(closedRotate.x, _rotateY, closedRotate.z);
			else need = closedRotate;
		}
		
		void Update () {
			if (transform.localRotation.y!=need.y) {
				transform.localRotation = Quaternion.Slerp(transform.localRotation, 
				need, 
				Time.deltaTime * _speed * Config.AnimationSpeedWorldObjects);
			}
		}
	}
}
