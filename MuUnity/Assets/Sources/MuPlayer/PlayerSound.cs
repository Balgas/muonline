using UnityEngine;
using System.Collections;

namespace MuPlayer {
	
	public class PlayerSound : MonoBehaviour {
		
		private AudioSource source;
		private Sounds.Player lastSound = Sounds.Player.WalkSoil;
		
		void Awake() {
			source = gameObject.AddComponent<AudioSource>();
			source.playOnAwake = false;
			source.clip = Util.Audio.Get(lastSound);
		}
		
		
		public void Play(Sounds.Player sound) {
			set = sound;
			source.Play();
		}
		
		public void Play(bool loop) {
			source.loop = loop;
			source.Play();
		}
		
		public void Stop() {
			source.Stop();	
		}
		
		public Sounds.Player set {
			set { if (lastSound!=value) { source.clip = Util.Audio.Get(value); lastSound = value; } }	
		}
			
		public float volume {
			set {
				source.volume = value;
			}
		}
		
	}
	
}
