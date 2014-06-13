using UnityEngine;
using System.Collections;

namespace MuMap {
	
	public class Map : WorldScene {
		
		public MapTerrain terrain;
		public MapObjects objects;
		public MapLighting lighting;
		public MapSound sound;
		public MapTest test;
		
		public delegate void DelegateDataZones(byte[,] data);
		public event DelegateDataZones EventDataZones;
		
		public delegate void DelegateDataTiles(MapDataGround.Tile[,] data);
		public event DelegateDataTiles EventDataTiles;
		
		public IEnumerator Init () {
			
			//инициализация глобальных компонентов
			InitGlobal();
			InitWorld();
			InitUI();
			
			terrain		= scene.AddComponent<MapTerrain>();
			lighting	= scene.AddComponent<MapLighting>();
			objects		= scene.AddComponent<MapObjects>();
			sound		= scene.AddComponent<MapSound>();
			test		= scene.AddComponent<MapTest>();
			
			//компоненты загрузки данных
			MapDataObjects	DataObjects	= new MapDataObjects	(global.map);
			MapDataZones	DataZones	= new MapDataZones		(global.map);
			MapDataGround	DataGround	= new MapDataGround		(global.map);
			MapDataHeight	DataHeight	= new MapDataHeight		(global.map);
			
			DataObjects.EventErrorFile	+= NotFoundDataFiles;
			DataZones.EventErrorFile	+= NotFoundDataFiles;
			DataGround.EventErrorFile	+= NotFoundDataFiles;
			DataHeight.EventErrorFile	+= NotFoundDataFiles;
			
			//инициализация ландшафта
			terrain.	Init(Terrain.activeTerrain, DataGround.data, DataHeight.data, DataZones.data);
			//инициализация объектов
			objects.	Init(DataObjects.data);
			//инициализация маппинга
			lighting.	Init(global.map);
			//инициализация маппинга
			sound.		Init(global.map);
			//передача данных в мир
			if (EventDataZones!=null)
				EventDataZones ( DataZones.data );
			if (EventDataTiles!=null)
				EventDataTiles ( DataGround.data );
			
			//чистим данные
			DataObjects		= null;
			DataZones		= null;
			DataGround		= null;
			DataHeight		= null;
			System.GC.Collect();
			
			yield break;
		}
		
		void NotFoundDataFiles (string file) {
			ui.error("NotFoundDataFiles", file);
		}
		
	}
	
}
