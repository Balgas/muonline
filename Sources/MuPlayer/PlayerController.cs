
using UnityEngine;
using System.Collections;
using MuGlobal;

namespace MuPlayer {
	
	public class PlayerController : WorldScene {
		
		float _lastClickTerrain;
		bool UIhover = false;
		
		void Start () {
			_lastClickTerrain = Time.time;
		}
		
		public void Init () {
			
			this.InitUI();
			this.InitWorld();
			
			#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || UNITY_BLACKBERRY
				ui.add ("JoystickUI");
			#endif
		}
		
		
		void Update () {
			
			if (Input.GetMouseButtonDown(0)) {
				UIhover = ui.isHover();
				ClickTerrain();
			} else if (Input.GetMouseButton(0)) {
				if(Time.time-_lastClickTerrain>Config.TerrainClickBetween) {
					ClickTerrain();
				}
			}
			
		}
		
		
		
		void ClickTerrain() {
			
			if (UIhover) return;
			
			_lastClickTerrain = Time.time;
			
			Vector3 v = Input.mousePosition;
			Ray ray = cam.ScreenPointToRay(v);
			RaycastHit hit;
			if(Physics.Raycast(ray.origin,ray.direction, out hit)){
				SendMessage("EventContollerClickTerrain", hit.point);
			}
			
			
		}
		
		
		
		
		
	}
	
}
