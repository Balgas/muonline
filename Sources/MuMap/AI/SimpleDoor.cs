
using UnityEngine;
using System.Collections;

namespace MuMap.AI {
	public class SimpleDoor : Interaction {
		
		public enum Action {
			Closed,
			Opened,
			Animated
		}
		
		float _distance = 300f;
		float _speed = 0.5f;
		float _rotateY = 180f;
		Action _action = Action.Closed;
		
		Quaternion closedRotate;
		AudioSource source;
		
		IEnumerator Start () {
			source = gameObject.AddComponent<AudioSource>();
			
			AudioClip clip = Util.Audio.Get( Sounds.Area.Door );
			if (clip!=null && clip.isReadyToPlay) {
				source.clip = clip;
				source.playOnAwake = false;
				source.loop = false;
			}
			
			closedRotate = transform.localRotation;
			
			yield return StartCoroutine("SearchMe");
			
			if (me!=null) 
				me.EventStartMoveTo += StartMoveTo;
			
			yield break;
		}
		
		void StartMoveTo(MuPlayer.PlayerData Data) {
			float dist = Distance();
			
			if (dist<=_distance && _action==Action.Closed) 
				StartCoroutine ( Open () );
			else if (dist>_distance && _action==Action.Opened) 
				StartCoroutine ( Close () );
			
		}
		
		IEnumerator Close() {
			_action = Action.Animated;
			yield return StartCoroutine ( Animate ( closedRotate ) );
			_action = Action.Closed;
			yield break;
		}
		
		IEnumerator Open() {
			_action = Action.Animated;
			source.Play();
			yield return StartCoroutine ( Animate ( Quaternion.Euler(closedRotate.x, _rotateY, closedRotate.z) ) );
			_action = Action.Opened;
			yield break;
		}
		
		IEnumerator Animate(Quaternion end) {
			Quaternion start = transform.localRotation;
			float time = _speed;
			while ( time > 0.0f ) {
				time -= Time.deltaTime;
				
				transform.localRotation = Quaternion.Slerp ( start, end, 
					1-(time / _speed));
				yield return null;
			}
			
			yield break;	
		}
		
	}
}
