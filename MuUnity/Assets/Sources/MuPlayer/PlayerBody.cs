/*
 * 
 * Тело игрока, объекты на нем
 * 
 */

using UnityEngine;
using System.Collections;

namespace MuPlayer {
	
	public class PlayerBody : WorldScene {
		
		public GameObject body;
		public GameObject weapon;
		
		protected void InitBody () {
			body = CreateContainer ("Body");
			weapon = CreateContainer ("Weapon");
		}
		
		protected void CreateBody ( Util.Player.Class Class ) {
			string[] consist = Util.Player.BodyConsist;
			
			for (int i = 0; i<consist.Length; i++) {
				AttachDetail(body, consist[i], Util.File.BodyStorageDir(Class, consist[i]));
			}

			CreateWeapon ();
		}

		protected void CreateWeapon (  ) {
			Util.GO.DestroyChildrens (weapon);
			//AttachDetail (weapon, "", "");
		}

		private GameObject CreateContainer ( string name ) {
			GameObject go = Util.GO.Create ( name, gameObject.transform );
			go.transform.localPosition = - Config.PlayerCenter;
			return go;
		}

		/*
		 * parent - родитель
		 * path - наименование части
		 * file - адрес файла
		 */
		private GameObject AttachDetail(GameObject parent, string path, string file) {
			GameObject prefab = Util.Storage.LoadPrefab(file);
			if (prefab==null) return null;
			
			GameObject go = (GameObject)Instantiate(prefab);
			Util.GO.SetParent ( go.transform, parent.transform );
			go.name = path;
			
			PlayerDetail detail = go.AddComponent<PlayerDetail>();
			detail.Init ( world.players.Animations );
			return go;
		}
		
		
	}
	
}