
using UnityEngine;
using System.Collections;

namespace MuPlayer {
	public class PlayerDetail : MonoBehaviour {
		
		protected Animator animator;
		
		public void Init(RuntimeAnimatorController controller) {
			gameObject.layer = (int)Util.GO.Layer.Players;
			gameObject.transform.localPosition = Config.PlayerCenter;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Config.ScaleObject;
			
			animator = gameObject.GetComponent<Animator>();
			
			if (animator!=null && controller!=null) {
				animator.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(controller);
				animator.speed = Config.AnimationSpeedWorldObjects;
				animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
				animator.applyRootMotion = false;
			}
		}
		
		public void Play(string state, float speed = 1f) {
			animator.speed = Config.AnimationSpeedWorldObjects*speed;
			animator.Play (state, 0, 1f);
		}

		
		
	}
	
}
