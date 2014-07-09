
using UnityEngine;
using System.Collections;

namespace MuPlayer {
	public class Character : Player {
		
		public PlayerCamera CurrentCamera; //ссылка на мою камеру
		public PlayerController Controller; //ссылка на контроллер меня
			
		//переопределения инициализации
		public override void Init() {
			
			base.Init();
			
			Data.isCharacter = true;
			
			CurrentCamera	= gameObject.AddComponent<PlayerCamera>();
			Controller		= gameObject.AddComponent<PlayerController>();
			
			GameObject AListener = GameObject.FindWithTag("AListener");
			if (AListener!=null) 
				Util.GO.SetParent ( AListener.transform, gameObject.transform );
			
			CurrentCamera.Init();
			Controller.Init();
			
			EventStartMoveTo	+= PlayerMoveTo;
			EventStartMove		+= PlayerStartMove;
			EventEndMove		+= PlayerEndMove;
			
		}
		
		//переопределения установки координат
		public override void SetCoord(MuCoord coord) {
			base.SetCoord(coord);
			CurrentCamera.Apply();
		}
		
		//клик по земле
		void EventContollerClickTerrain(Vector3 point) {
			
			Loom.RunAsync(()=>{
		        
				MuCoord coord = Util.Map.Vector3ToCoords(point);
				
				MuCoord[] coords = PathFinder.Get(Data.Coord, coord, world.players.zones);
				
				Loom.QueueOnMainThread(()=>{
		        	if (coords.Length>0) { //если путь найден отправляем в главный поток
						//ставим конечную точку
		          		world.effects.CreateMoveTarget(point, coord);
						//указываем игроку путь
						SetPath(coords);
					}
				});
				
		    });
			
		}
		
		//общее начало движения
		void PlayerStartMove(PlayerData data) {
			StopCoroutine("StepByAnimations");
			StartCoroutine("StepByAnimations");
		}
		
		//общий конец движения
		void PlayerEndMove(PlayerData data) {
			StopCoroutine("StepByAnimations");
		}
		
		//начало шага
		void PlayerMoveTo(PlayerData data) {
			CurrentCamera.ChangeCoords(data);
			world.CharacterMove(data, State);
		}
		
		//интервалы между анимациями
		IEnumerator StepByAnimations() {
			
			//если не плывет, то пропускаем N сек перед циклом шагов
			if (!State.isSwim) yield return new WaitForSeconds(0.2f);
			
			while (State.isMove) {
				Sounds.Player sound = Util.Audio.GetTileWalk(State, world.map.terrain.grasses, world.players.tiles[Data.Coord.y, Data.Coord.x]);
				Sound.Play ( sound );
				float time = Config.TimeWalk*Config.AnimationSpeedWorldObjects;
				
				//если короткий звук то делится на 2 части
				if (Sounds.isShortSoundStep(sound)) time /= 2;
				
				yield return new WaitForSeconds(time);
				//если короткий звук то повторяется 2 раза
				if (Sounds.isShortSoundStep(sound)) {
					Sound.Play ( sound );
					yield return new WaitForSeconds(time);	
				}
			}
			
			yield break;
		}
		
		
	}
}