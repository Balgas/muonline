using UnityEngine;
using System.Collections;

namespace MuMap {
	public class MapSound : MonoBehaviour {
		
		private AudioSource wind;
		private AudioSource area;
		
		//-1 не установлено ранее, 1 safe, 0 battle;
		private int _isSafe = -1;
		private Util.Map.Location map;
		//последний звук, чтобы не обновлять его, если он не обновился
		private Sounds.Area lastArea = Sounds.Area.Wind;
		
		public void Init (Util.Map.Location map) {
			this.map = map;
			
			area = gameObject.AddComponent<AudioSource>();
			area.loop = true;
			
			if (WorldConfig.GetWindSound(map)) {
				wind = Util.Audio.GetLoopSource (gameObject, Sounds.Area.Wind);
			}
			
		}
		
		//проверка на изменение сейфзоны
		public bool isSafe {
			set {
				if (value && _isSafe!=1) {
					_isSafe = 1;
					PlaySafe();
				} else if (!value && _isSafe!=0) {
					_isSafe = 0;
					PlayBattle();
				} else if (_isSafe==-1) {
					_isSafe = value ? 1 : 0;
					if (_isSafe==1) PlaySafe();
					else if (_isSafe==0) PlayBattle();
				}
			}
		}
		
		//проигрование звука вне города
		private void PlayBattle() {
			SetClipAndPlay(WorldConfig.GetLoopSoundBattle(map));
		}
		
		//проигрование звука в городе
		private void PlaySafe() {
			SetClipAndPlay(WorldConfig.GetLoopSound(map));
		}
		
		//устанавливает клип и проигрывает его
		private void SetClipAndPlay(Sounds.Area loop) {
			if (loop!=Sounds.Area.Wind) {
				if (loop==lastArea) return;
				lastArea = loop;
				AudioClip clip = Util.Audio.Get(loop);
				if (clip!=null && clip.isReadyToPlay) {
					area.clip = clip;
					area.Play();
				} else area.clip = null;
			} else area.clip = null;
		}
		
		public bool mute {
			set {
				if (area!=null) area.mute = value;
				if (wind!=null) wind.mute = value;
			}
		}
		
		public float volume {
			set {
				if (area!=null) area.volume = value*Config.RatioMapSound;
				if (wind!=null) wind.volume = value*Config.RatioMapSound;
			}
		}
		
		
	}
	
}
