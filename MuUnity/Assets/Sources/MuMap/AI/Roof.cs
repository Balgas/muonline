
using UnityEngine;
using System.Collections;
using MuMap;

namespace MuMap.AI {
	public class Roof : Interaction {
		
		float _distance = 250f;
		public bool check = false;
		
		MapObjects mobject = null;
		Roof[] roofs;
		
		IEnumerator Start () {
			
			InitWorld();
			InitObjects();
			InitSMD();
			
			yield return StartCoroutine("SearchMe");
			
			if (world!=null) {
				mobject = world.GetComponent<MapObjects>();
				mobject.EventUpdateLOD += UpdateLOD;
			}
				
			if (objects!=null && me!=null) 
				me.EventStartMoveTo += StartMoveTo;
			
			UpdateLOD();
			Check();
			
			yield break;
		}
		
		void UpdateLOD() {
			roofs = objects.GetComponentsInChildren<Roof>();
		}
		
		void Check() {
			float dist = Distance();
			check = (dist<=_distance);
			if (check && SMD.gameObject.activeSelf) {
				HiddenAll();
			} else {
				CheckAll();	
			}
		}
		
		void StartMoveTo(MuPlayer.PlayerData Data) {
			Check();
		}
		
		void HiddenAll() {
			if (roofs==null) return;
			foreach (Roof roof in roofs) {
				if (roof.SMD!=null && roof.SMD.gameObject.activeSelf==true)
					roof.SMD.gameObject.SetActive(false);	
			}
			MapMute(true);
		}
		
		void ShowAll() {
			if (roofs==null) return;
			foreach (Roof roof in roofs) {
				if (roof.SMD!=null && roof.SMD.gameObject.activeSelf==false)
					roof.SMD.gameObject.SetActive(true);	
			}
			MapMute(false);
		}
		
		void MapMute(bool mute) {
			if (world!=null) {
				MapSound sound = world.GetComponent<MapSound>();	
				if (sound!=null) {
					sound.mute = mute;
				}
			}
		}
		
		void CheckAll() {
			if (roofs==null) return;
			bool b = false;
			foreach (Roof roof in roofs) {
				if (roof.check) {
					b = true;
					break;
				}
			}
			
			if (!b && !SMD.gameObject.activeSelf) {
				ShowAll();
			}
		}
		
		
	}
}
