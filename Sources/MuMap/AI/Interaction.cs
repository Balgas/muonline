
using UnityEngine;
using System.Collections;

namespace MuMap.AI {
	public class Interaction : SMDObject {
		
		protected MuPlayer.Character me;
		protected GameObject world;
		protected GameObject objects;
		
		
		protected void InitWorld() {
			world = Util.GO.GetTag ( Util.GO.Tag.World );
		}
		
		protected void InitObjects() {
			if (world!=null) {
				objects = world.GetComponent<MapObjects>().go;	
			}
		}
		
		protected IEnumerator SearchMe() {
			GameObject meGO = Util.GO.GetTag ( Util.GO.Tag.Character );
			if (meGO!=null)
				me = meGO.GetComponent<MuPlayer.Character>();
			yield break;
		}
		
		protected float Distance() {
			return (me==null) ? 0f : Util.Map.Distance2(gameObject.transform.localPosition, me.transform.localPosition);	
		}
	}
}