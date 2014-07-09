using UnityEngine;
using System.Collections;

namespace Util {
	
	public class Audio {
		
		//новый аудио поток площади
		public static AudioSource GetLoopSource(GameObject target, Sounds.Area sound) {
			AudioSource source = target.AddComponent<AudioSource>();
			AudioClip clip = Util.Audio.Get(sound);
			if (clip!=null && clip.isReadyToPlay) {
				source.clip = clip;
				source.loop = true;
				source.Play();
			}
			return source;
		}
		
		//ауди клип площади
		public static AudioClip Get(Sounds.Area sound) {
			return Util.Storage.LoadAudioFromResources ( Util.File.DIRECTORY_SOUND_AREA + sound.ToString() );	
		}
		
		//ауди клип игрока
		public static AudioClip Get(Sounds.Player sound) {
			return Util.Storage.LoadAudioFromResources ( Util.File.DIRECTORY_SOUND_PLAYER + sound.ToString());	
		}
		
		//аудио клип хотьбы в определенной точки
		public static Sounds.Player GetTileWalk(MuPlayer.PlayerState state, MuMap.Grass[] grasses, MuMap.MapDataGround.Tile tile) {
			Sounds.Player sound = Sounds.Player.WalkSoil;
			byte mainTile = tile.alpha>0.5f ? tile.id1 : tile.id2;
			if (state.isSwim) {
				sound = Sounds.Player.Swim;
			} else if (Util.Map.isGrass(grasses, mainTile)) {
				if (state.map==Util.Map.Location.Devias)
					sound = Sounds.Player.WalkSnow;
				else
					sound = Sounds.Player.WalkGrass;	
			}
			return sound;
		}
		
	}
	
}
