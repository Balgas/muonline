using UnityEngine;
using System.Collections;
using System;

namespace Util {
	public class Map : MonoBehaviour {
		
		public enum Location:int {
		 	Lorencia = 1,
			Dungeun = 2,
			Devias = 3,
			Noria = 4,
			LostTower = 5,
			DareDevil = 6,
			Stadium = 7,
			Atlans = 8,
			Tarcan = 9,
			DevilSquare = 10,
			Icarus = 11,
			BloodCastle = 12
		}
		
		//длина диагонали
		public static float TileDiagonal = Mathf.Sqrt(Mathf.Pow(Config.TileSize, 2)*2);
		
		//растояние между двумя точками
		public static float DistanceBetweenTwoPoints(MuCoord coord1, MuCoord coord2) {
			return Vector2.Distance(coord1.ToVector(), coord2.ToVector());
		}
		
		//Vector3 в Vector2
		public static Vector2 Vector3toVector2(Vector3 v) {
			return new Vector2(v.x, v.z);	
		}
		
		//Vector3 -> Mu Vector3
		public static Vector3 Vector3ToVector3(Vector3 coords) {
			MuCoord c = Vector3ToCoords(coords);
			Vector3 v = CoordsToVector3(c);
			return v;
		}
		
		//Vector3 -> MuCoord
		public static MuCoord Vector3ToCoords(Vector3 v) {
			MuCoord coord = new MuCoord();
			coord.x = (int)Mathf.Floor(v.x/Config.TileSize);
			coord.y = (int)Mathf.Floor(v.z/Config.TileSize);
			return coord;
		}
		
		//MuCoord -> Vector3
		public static Vector3 CoordsToVector3(MuCoord c) {
			Vector3 v = Vector3.zero;
			v.x = c.x*Config.TileSize+Config.TileSize/2;
			v.z = c.y*Config.TileSize+Config.TileSize/2;
			return v;
		}
		
		public static float Distance2 (Vector3 first, Vector3 two) {
			return Vector2.Distance(
				Vector3toVector2(first), 
				Vector3toVector2(two));
		}
		
		//возвращает тестовые стартовые координаты
		public static MuCoord TestMyCoord (Util.Map.Location map) {
			MuCoord coord = new MuCoord();
			switch (map) {
				case Util.Map.Location.Lorencia:
					coord = new MuCoord(){ x = 135, y = 128 };
					break;
				case Util.Map.Location.Devias:
					coord = new MuCoord(){ x = 218, y = 34 };
					break;
				case Util.Map.Location.Noria:
					coord = new MuCoord(){ x = 173, y = 110 };
					break;
				case Util.Map.Location.Dungeun:
					coord = new MuCoord(){ x = 109, y = 247 };
					break;
				case Util.Map.Location.Atlans:
					coord = new MuCoord(){ x = 21, y = 14 };
					break;
				case Util.Map.Location.LostTower:
					coord = new MuCoord(){ x = 165, y = 168 };
					break;
				case Util.Map.Location.Stadium:
					coord = new MuCoord(){ x = 60, y = 108 };
					break;
				case Util.Map.Location.Tarcan:
					coord = new MuCoord(){ x = 130, y = 50 };
					break;
				case Util.Map.Location.BloodCastle:
					coord = new MuCoord(){ x = 15, y = 15 };
					break;
				case Util.Map.Location.DevilSquare:
					coord = new MuCoord(){ x = 70, y = 100 };
					break;
			}
			return coord;
		}
		
		//функция возвращает координаты вокруг точки
		public static MuCoord[] CoordsAround ( MuCoord coord, int step = 0 ) {
			if (step==0) { MuCoord[] _coords = new MuCoord[1]; _coords[0] = coord; return _coords; }
			int length = step*2+1;
			int c = length*4-4;
			MuCoord[] coords = new MuCoord[c];
			int x, y, i = 0;
			//top
			if (CorrectXY(coord.y-step)==coord.y-step) {
				for (x=CorrectXY(coord.x-step); x<CorrectXY(coord.x+step); x++) {
					coords[i] = new MuCoord(){ x = x, y = coord.y-step };
					i++;
				}
			}
			//right
			if (CorrectXY(coord.x+step)==coord.x+step) {
				for (y=CorrectXY(coord.y-step); y<CorrectXY(coord.y+step); y++) {
					coords[i] = new MuCoord(){ x = coord.x+step, y = y };
					i++;
				}
			}
			//bottom
			if (CorrectXY(coord.y+step)==coord.y+step) {
				for (x=CorrectXY(coord.x+step-1); x>=CorrectXY(coord.x-step); x--) {
					coords[i] = new MuCoord(){ x = x, y = coord.y+step };
					i++;
				}
			}
			//left
			if (CorrectXY(coord.x-step)==coord.x-step) {
				for (y=CorrectXY(coord.y+step-1); y>=CorrectXY(coord.y-step); y--) {
					coords[i] = new MuCoord(){ x = coord.x-step, y = y };
					i++;
				}
			}
			Array.Resize(ref coords, i);
			return coords;
		}
		
		//корректирует X, Y от мин до макс
		public static int CorrectXY(int i) {
			if (i<0) i = 0;
			else if (i>=Config.MapLength) i = Config.MapLength-1;
			return i;
		}
		
		//проверяет диагональ по 2м координатам
		public static bool isDiagonal(MuCoord s, MuCoord e) {
			return (Mathf.Abs(e.x-s.x) == 1 && Mathf.Abs(e.y-s.y) == 1);	
		}
		
		//возвращает угол по 2м координатам
		public static float GetDirectionFloat(MuCoord s, MuCoord e) {
			if (s.x==e.x && s.y==e.y) return Config.DefaultPlayerDirection;
			float f = 0f;
			if (e.y>s.y) {
				if (e.x>s.x) f = 45f;
				else if (e.x<s.x) f = 315f;
				else f = 0f;
			} else if (e.y<s.y) {
				if (e.x>s.x) f = 135f;
				else if (e.x<s.x) f = 225f;
				else f = 180f;
			} else if (e.y==s.y) {
				if (e.x>s.x) f = 90f;
				else if (e.x<s.x) f = 270f;
			}
			return f;
		}
		
		//проверяет зону на плавательную
		public static bool isSwimZone(Util.Map.Location map) {
			return map == Util.Map.Location.Atlans;	
		}
		
		//это яма?
		public static bool isHoleZone(byte zone) {
			return (zone==(byte)Structurs.TileType.Hole || zone==(byte)Structurs.TileType.Hole2);	
		}
		
		//здесь трава?
		public static bool isGrass(MuMap.Grass[] grasses, byte tile) {
			return (tile<grasses.Length && grasses[tile]!=null);	
		}

		public static bool isSafeZone(byte zone) {
			return zone==(byte)Structurs.TileType.Safe;
		}
		
		//там где можно ходить
		public static bool isWalkZone(byte zone) {
			return (zone==(byte)Structurs.TileType.Walk || 
				zone==(byte)Structurs.TileType.Safe ||
				zone==(byte)Structurs.TileType.BCBridgeStart ||
				zone==(byte)Structurs.TileType.BCBridgeMiddle);	
		}
		
		
		
	}
}
