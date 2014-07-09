
using UnityEngine;
using System.Collections;

namespace MuGlobal {
	
	public class GlobalData : MonoBehaviour {
		
		//текущая карта
		public Util.Map.Location map = Util.Map.Location.Lorencia;
		//настройки пользователя
		public Settings settings = new Settings();
		//языковые строки
		public Hashtable strings = new Hashtable();
		
		
	}
}
