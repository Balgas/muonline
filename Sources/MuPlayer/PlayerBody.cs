
using UnityEngine;
using System.Collections;

namespace MuPlayer {
	
	public class PlayerBody : WorldScene {
		
		public GameObject body;
		
		protected void InitBody () {
			body = Util.GO.Create ( "Body", gameObject.transform );
			body.transform.localPosition = - Config.PlayerCenter;
		}
		
		protected void CreateBody ( Util.Player.Class Class ) {
			string[] consist = Util.Player.BodyConsist;
			
			for (int i = 0; i<consist.Length; i++) {
				AttachDetail(consist[i], Util.File.BodyStorageDir(Class, consist[i]));
			}
		}
		
		GameObject AttachDetail(string path, string file) {
			GameObject prefab = Util.Storage.LoadPrefab(file);
			if (prefab==null) return null;
			
			GameObject go = (GameObject)Instantiate(prefab);
			Util.GO.SetParent ( go.transform, body.transform );
			go.name = path;
			
			PlayerDetail detail = go.AddComponent<PlayerDetail>();
			detail.Init ( world.players.Animations );
			return go;
		}
		
		
	}
	
}