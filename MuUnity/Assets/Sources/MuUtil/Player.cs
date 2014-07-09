using UnityEngine;
using System.Collections;

namespace Util {
	
	public static class Player {
		
		public enum Class:byte {
			MELF = 0x50,
			ELF = 0x40,
			BK = 0x30,
			DK = 0x20,
			SM = 0x10,
			DW = 0x00,
			DL = 0x80,
			MG = 0x60
		}
		
		//из чего состоит тело
		public static string[] BodyConsist = new string[]{ "Armor", "Boot", "Glove", "Helm", "Pant" };
		
		//преобразование класса в инт
		public static int GetClassInt( Class _class ) {
			int c = 0;
			switch (_class) {
			case Class.DW: case Class.SM: c = 1; break;
			case Class.DK: case Class.BK: c = 2; break;
			case Class.ELF: case Class.MELF: c = 3; break;
			case Class.MG: c = 4; break;
			case Class.DL: c = 5; break;
			}
			return c;
		}
		
		//проверка является ли класс сабом
		public static bool IsSub ( Class _class ) {
			bool b = false;
			switch (_class) {
			case Class.SM: b = true; break;
			case Class.BK: b = true; break;
			case Class.MELF: b = true; break;
			}
			return b;
		}
		
		public static GameObject CreatePlayersGameObject ( string name, Transform parent ) {
			GameObject playerGO = new GameObject ( name );
			Util.GO.SetParent ( playerGO.transform, parent );
			Util.GO.SetLayer ( playerGO, Util.GO.Layer.Players );
			return playerGO;
		}
		
	}
	
}
