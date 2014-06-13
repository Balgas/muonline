
using UnityEngine;
using System.Collections;

namespace MuMap {
	public class MapLighting : MonoBehaviour {
		
		public void Init(Util.Map.Location map) {
			LightmapData[] lightmapData = new LightmapData[1];
			
			LightmapSettings.lightmapsMode = LightmapsMode.Single;
			for( int i = 0 ; i < 1 ; i++ ) {
				lightmapData[i] = new LightmapData();
				lightmapData[i].lightmapFar = Resources.Load(Util.File.DIRECTORY_LIGHTS+(int)map, typeof(Texture2D)) as Texture2D;
			}
			LightmapSettings.lightmaps = lightmapData;
			
		}
		
		
		
	}
}
