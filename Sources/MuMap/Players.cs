
/* 
 * - создание игроков
 * 
 * WorldLoaded() - мир инициализирован
 * 
 */

using UnityEngine;
using System.Collections;
using MuPlayer;

namespace MuMap {
	
	public class Players : WorldScene {
	
		public Character me; //ссылка на меня
		public RuntimeAnimatorController Animations; //ссылка на анимацию
		public byte[,] zones; //ссылка на зоны
		public MuMap.MapDataGround.Tile[,] tiles; //ссылка на землю
		public GameObject go; //контейнер Players в мире
		
		public IEnumerator Init() {
			
			this.InitGlobal();
			this.InitUI();
			this.InitWorld();
			
			Animations = (RuntimeAnimatorController)Resources.Load("Player/PlayerController", typeof(RuntimeAnimatorController));
			if (Animations==null) ui.error("ErrorCreatePlayer", "PlayerController");
			
			go = Util.GO.Create ( "Players", scene.transform, Util.GO.Layer.Players );
			
			yield break;
		}
		
		public void CreateCharacter(MuCoord coord) {
			GameObject player = Util.Player.CreatePlayersGameObject("Me", go.transform);
			Util.GO.SetTag ( player, Util.GO.Tag.Character );
			me = player.AddComponent<Character>();
			me.Init(); //обязательно инициализация
			me.State.map = global.map; //определяем зону для плавания
			me.SetClass(Util.Player.Class.DL); //указываем обязательно класс
			me.SetCoord(coord);
		}
		
	}
	
}