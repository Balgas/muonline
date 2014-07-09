
using UnityEngine;
using System.Collections;


namespace MuGlobal {

	 public class Global : GlobalData {
	
	 	public static Global instance;
	
		void Awake () {
	 		instance = this;
			
	 		//не разрушаем этот объект
	 		DontDestroyOnLoad (gameObject);
			
			//добавляем глобальный компонент системных настроек
			gameObject.AddComponent<SystemSettings>();
	 	}
		
		public void Init () {
			//загружаем строки
			Util.Storage.LoadLang(ref strings, Util.File.Localization());
		}
		
		public void WWWSend ( string url ) {
			WWW www = new WWW ( url );
			StartCoroutine ( LoadWWWCoroutine ( www ) );
		}
		
		IEnumerator LoadWWWCoroutine ( WWW www ) {
			while (!www.isDone) {
				yield return www;	
			}
			yield break;
		}
		

	}
	
}
