
using UnityEngine;
using System.Collections;


public class Structurs : MonoBehaviour {
	
	
	
	public enum Direction:byte {
		Down = 0x01,
		DownRight = 0x02,
		Right = 0x03,
		TopRight = 0x04,
		Top = 0x05,
		TopLeft = 0x06,
		Left = 0x07,
		DownLeft = 0x08,
		Center = 0x09
	}
	
	public enum TileType:byte {
		NotWalk = 4, //вне города, нельзя ходить
		Walk = 0, //вне города, можно ходить
		Safe = 1, //город, можно ходить
		Wall = 5, //в городе, нельзя ходить
		Hole = 12, //пропасть, нельзя ходить, прозрачные тили
		Hole2 = 8, //пропасть, нельзя ходить, прозрачные тили
		Hole3 = 13,	
		Unknown = 2,
		BCBridgeStart = 36,
		BCBridgeMiddle = 32,
		BCBridgeFinish = 40
		
	}

	public enum ItemType:byte {
		Swords = 0,
		Axes = 1,
		Maces = 2,
		Spears = 3,
		Bows = 4,
		Staffs = 5,
		Shields = 6,
		Helms = 7,
		Armors = 8,
		Pants = 9,
		Gloves = 10,
		Boots = 11,
		Items = 12,
		MagicItems = 13,
		SimpleItems = 14,
		Scrolls = 15
	}


}
