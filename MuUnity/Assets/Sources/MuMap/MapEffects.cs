using UnityEngine;
using System.Collections;

namespace MuMap {
	
	public class MapEffects : WorldScene {
		
		private GameObject go;
		
		public IEnumerator Init() {
			InitGlobal();
			InitWorld();
			
			go = new GameObject("Effects");
			Util.GO.SetParent ( go.transform, gameObject.transform );
			
			yield break;
		}
		
		public void CreateMoveTarget ( Vector3 point, MuCoord coord ) {
			if (!global.settings.ShowMoveTarget) return;
			
			Transform move = go.transform.Find ( "MoveTarget" );
			if (move!=null) Destroy(move.gameObject);	
			
			Vector3 terrainHeight = world.map.terrain.GetHeight(coord);
			terrainHeight.y += 3;
			if (Config.AccurateMoveTarget) {
				terrainHeight.x = point.x;
				terrainHeight.z = point.z;
			}
			
			GameObject target = Util.GO.CreateAdvancedObject ( "MoveTarget", go.transform, "Effect/MoveTargetPosEffect", "Effect/Animations/MoveTargetPosController", terrainHeight, Vector3.zero, Util.GO.Layer.Effects, 0.7f );
			target.AddComponent<MuEffects.MoveTarget>();
		}
		
	}
	
}