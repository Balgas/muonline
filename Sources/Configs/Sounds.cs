using UnityEngine;
	
public static class Sounds {
	
	
	public enum Area:int {
		Wind,
		Rain,
		Water,
		Dungeon,
		Tower,
		Desert,
		Forest,
		Door
	}
	
	public enum Player:int {
		Swim,
		WalkGrass,
		WalkSnow,
		WalkSoil,
		Energy
	}
	
	//проверяет короткий ли шаг звука
	public static bool isShortSoundStep(Player sound) {
		return sound != Player.Swim;
	}
	
}
