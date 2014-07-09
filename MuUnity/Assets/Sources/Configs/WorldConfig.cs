
/* Настройки мира
 * 
 * 
 */

using UnityEngine;
using System;
using MuMap;

public class WorldConfig : MonoBehaviour {
	
	public static string FILE_GROUND	= "EncTerrain.map";
	public static string FILE_HEIGHT	= "TerrainHeight.OZB";
	public static string FILE_ZONE		= "EncTerrain.att";
	public static string FILE_OBJECT	= "Terrain.obj";
	
	//основной повторяющий звук вне города
	public static Sounds.Area GetLoopSoundBattle(Util.Map.Location map) {
		Sounds.Area sound = Sounds.Area.Wind;
		switch (map) {
		case Util.Map.Location.Noria:		sound = Sounds.Area.Forest; break;
		case Util.Map.Location.Dungeun:		sound = Sounds.Area.Dungeon; break;
		case Util.Map.Location.Atlans:		sound = Sounds.Area.Water; break;
		case Util.Map.Location.LostTower:	sound = Sounds.Area.Tower; break;
		case Util.Map.Location.Tarcan:		sound = Sounds.Area.Desert; break;
		}
		return sound;
	}
	
	//основной повторяющий звук
	public static Sounds.Area GetLoopSound(Util.Map.Location map) {
		Sounds.Area sound = Sounds.Area.Wind;
		switch (map) {
		//case Util.Map.Location.Lorencia:	sound = Sounds.Area.Rain; break;
		case Util.Map.Location.Dungeun:		sound = Sounds.Area.Dungeon; break;
		case Util.Map.Location.Atlans:		sound = Sounds.Area.Water; break;
		case Util.Map.Location.LostTower:	sound = Sounds.Area.Tower; break;
		case Util.Map.Location.Tarcan:		sound = Sounds.Area.Desert; break;
		}
		return sound;
	}
	
	//звук ветра
	public static bool GetWindSound(Util.Map.Location map) {
		bool wind = false;
		switch (map) {
		case Util.Map.Location.Lorencia:	
		case Util.Map.Location.Devias:	
		case Util.Map.Location.Noria:	
			wind = true;
			break;
		}
		return wind;
	}
	
	//трава
	public static Grass[] GetDetailsList(Util.Map.Location map) {
		Grass[] textures = new Grass[0];
		switch (map) {
			case Util.Map.Location.Lorencia:	textures	= new Grass[]{ 
				new Grass(){ file = "TileGrass01", dry = Color.yellow, healthy = Color.green, minHeight = 100, maxHeight = 120 } 
			}; break;
			case Util.Map.Location.Devias:	textures	= new Grass[]{ 
				new Grass(){ file = "TileGrass02", dry = Color.white, healthy = Color.white, minHeight = 50, maxHeight = 100 },
				new Grass(){ file = "TileGrass02", dry = Color.white, healthy = Color.white, minHeight = 50, maxHeight = 100 }
			}; break;
			case Util.Map.Location.Noria:		textures	= new Grass[]{ 
				new Grass(){ file = "TileGrass01", dry = Color.yellow, healthy = Color.green, minHeight = 150, maxHeight = 200 }, 
				null, 
				new Grass(){ file = "TileGrass03", dry = Color.yellow, healthy = Color.yellow, minHeight = 200, maxHeight = 300 } 
			}; break;
			case Util.Map.Location.DareDevil:	textures	= new Grass[]{ 
				new Grass(){ file = "TileGrass01", dry = Color.yellow, healthy = Color.yellow, minHeight = 150, maxHeight = 200 }, 
				new Grass(){ file = "TileGrass02", dry = Color.white, healthy = Color.white, minHeight = 150, maxHeight = 200 },
				new Grass(){ file = "TileGrass03", dry = Color.yellow, healthy = Color.yellow, minHeight = 200, maxHeight = 300 } 
			}; break;
			case Util.Map.Location.Stadium:	textures	= new Grass[]{ 
				new Grass(){ file = "TileGrass01", dry = Color.yellow, healthy = Color.yellow, minHeight = 100, maxHeight = 150 }
			}; break;
			case Util.Map.Location.Tarcan:	textures	= new Grass[]{ 
				new Grass(){ file = "TileGrass01", dry = Color.gray, healthy = Color.gray, minHeight = 200, maxHeight = 300 }, 
				null,
				new Grass(){ file = "TileGrass03", dry = Color.yellow, healthy = Color.yellow, minHeight = 200, maxHeight = 300 } 
			}; break;
		}
		return textures;
	}
	
	//формирование списка текстур земли
	public static string[] GetTilesList(Util.Map.Location map) {
		string[] textures = new string[0];
		switch (map) {
			case Util.Map.Location.Lorencia:	textures	= new string[]{ "TileGrass01", "TileGrass02", "TileGround01", "TileGround02", "TileGround03", "TileWater01", "TileWood01", "TileRock01", "TileRock02" }; break;
			case Util.Map.Location.Dungeun:		textures	= new string[]{ "TileGrass01", "TileGrass02", "TileGround01", "TileGround02", "TileGround03", "TileWater01", "TileWood01", "TileRock01", "TileRock02" }; break;
			case Util.Map.Location.Devias:		textures	= new string[]{ "TileGrass01", "TileGrass02", "TileGround01", "TileGround02", "TileGround03", "TileWater01", "TileWood01", "TileRock01", "TileRock02", "TileRock03", "TileRock04", "TileRock05", "TileRock06", "TileRock07" }; break;
			case Util.Map.Location.Noria:		textures	= new string[]{ "TileGrass01", "TileGrass01", "TileGround01", "TileGround01", "TileGround03", "TileWater01", "TileWood01", "TileRock01", "TileRock02", "TileRock03", "TileRock04" }; break;
			case Util.Map.Location.LostTower:	textures	= new string[]{ "TileGrass01", "TileGrass02", "TileGround01", "TileGround02", "TileGround03", "TileWater01", "TileWood01", "TileRock01", "TileRock02", "TileRock03", "TileRock04" }; break;
			case Util.Map.Location.DareDevil:	textures	= new string[]{ "TileGrass01", "TileGrass02", "TileGround01", "TileGround02", "TileGround03", "TileWater01", "TileWood01", "TileRock01", "TileRock02", "TileRock03", "TileRock04", "TileRock05", "TileRock06", "TileRock07" }; break;
			case Util.Map.Location.Stadium:		textures	= new string[]{ "TileGrass01", "TileGrass02", "TileGround01", "TileGround02", "TileGround03", "TileWater01", "TileWood01", "TileRock01", "TileRock02", "TileRock03", "TileRock04" }; break;
			case Util.Map.Location.Atlans:		textures	= new string[]{ "TileGrass01", "TileGrass02", "TileGround01", "TileGround01", "TileGround03", "TileGrass01", "TileWood01", "TileRock01", "TileRock02", "TileRock03", "TileRock04" }; break;
			case Util.Map.Location.Tarcan:		textures	= new string[]{ "TileGrass01", "TileGrass02", "TileGround01", "TileGround02", "TileGround03", "TileWater01", "TileWood01", "TileRock01", "TileRock02", "TileRock03", "TileRock04" }; break;
			case Util.Map.Location.DevilSquare:	textures	= new string[]{ "TileGrass01", "TileGrass02", "TileGround01", "TileGround01", "TileGround03", "TileGrass01", "TileWood01", "TileRock01", "TileRock02", "TileRock03", "TileRock04" }; break;
			case Util.Map.Location.BloodCastle:	textures	= new string[]{ "TileGrass01", "TileGrass02", "TileGround02", "TileGround02", "TileGround03", "TileWater01", "TileWood01", "TileRock01", "TileRock02", "TileRock03", "TileRock04" }; break;
			case Util.Map.Location.Icarus:		textures	= new string[]{ "TileGrass01", "TileGrass01", "TileGrass01", "TileGrass01", "TileGrass01", "TileGrass01", "TileGrass01", "TileGrass01", "TileGrass01", "TileGrass01", "TileRock04" }; break;
		}
		return textures;
	}
	
	//список спец предметов
	public static int[] GetSpecialObjects(Util.Map.Location map) {
		int[] objects = new int[0];
		switch (map) {
			case Util.Map.Location.Lorencia:	objects	= new int[]{ 1, 61, 91 }; break;
			case Util.Map.Location.Dungeun:		objects	= new int[]{ 61 }; break;
			case Util.Map.Location.LostTower: 	objects	= new int[]{ 41 }; break;
			case Util.Map.Location.Stadium:		objects	= new int[]{ 65022 }; break;
		}
		return objects;
	}
	
	//привязка компонентов к предметам
	public static Type GetAIObjects ( Util.Map.Location map, int id ) {
		Type component = null;
		switch (map) {
			case Util.Map.Location.Atlans:
				switch (id) {
					case 24: component = typeof(MuMap.AI.AtlansDoor); break;
				}
			break;
			
			case Util.Map.Location.Devias:
				switch (id) {
					case 21: case 89: component = typeof(MuMap.AI.SimpleDoor); break;
					case 87: component = typeof(MuMap.AI.GateDoor); break;
					case 82: case 83: case 99: case 100: component = typeof(MuMap.AI.Roof); break;
				}
			break;
			case Util.Map.Location.Lorencia:
				switch (id) {
					case 126: case 127: component = typeof(MuMap.AI.Roof); break;
				}
			break;
		}
		
		return component;
	}
	
	public static Color GetBackground ( Util.Map.Location map ) {
		Color color = Color.black;
		switch (map) {
			case Util.Map.Location.Devias: color = Color.white; break;
			case Util.Map.Location.Icarus: color = new Color(0.4f, 0.6f, 0.9f, 1f); break;
		}
		return color;
	}
	
	
}
