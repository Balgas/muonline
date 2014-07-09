/* 
 * Мир
 * 
 * 
 * 
 */

using UnityEngine;
using System.Collections;
using MuMap;

public class World : WorldScene {
	
	public Map map;
	public Players players;
	public MapEffects effects;
	public WorldController controller;
	
	IEnumerator Start () {
		
		InitGlobal();
		InitWorld();
		InitUI();
		
		//проверка все ли есть на сцене
		if (Terrain.activeTerrain==null) { ui.error("ErrorCreateMap", "Terrain"); yield break; }
		else if (cam==null) { ui.error("ErrorCreateMap", "Camera"); yield break; }
		else if (scene==null) { ui.error("ErrorCreateMap", "Scene"); yield break; }
		
		
		//инициализация компонентов мира
		map			= scene.AddComponent<Map>();
		players		= scene.AddComponent<Players>();
		effects		= scene.AddComponent<MapEffects>();
		controller	= scene.AddComponent<WorldController>();
		
		//передача в players tiles
		map.EventDataTiles += delegate(MapDataGround.Tile[,] data) {
			players.tiles = data;
		};
		
		//передача в players zones
		map.EventDataZones += delegate(byte[,] data) {
			players.zones = data;
		};
		
		//подпрограммы инциализаций компонентов
		yield return StartCoroutine(map.Init());
		yield return StartCoroutine(players.Init());
		yield return StartCoroutine(effects.Init());
		yield return StartCoroutine(controller.Init());
		
		CreateMe();
		
		
		yield break;
	}
	
	public void CreateMe() {
		players.CreateCharacter(Util.Map.TestMyCoord(global.map));
	}
	
	private void SetLogo() {
		//добавляем логотип карты
		ui.add("MapLogo");
		ui.send("MapLogo", "Load", global.map.ToString());
	}
	
	public void CharacterMove(MuPlayer.PlayerData data, MuPlayer.PlayerState state) {
		//координаты применяются в окружающих объектах
		map.objects.ChangeCoord(data.Coord);
		//координаты применяются в изменени звука
		map.sound.isSafe = state.isSafe;
		
		map.test.coord = data.Coord;
	}
	
}