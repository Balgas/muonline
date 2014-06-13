
using UnityEngine;
using System.Collections;

namespace MuMap {
	
	public class MapDataGround : MapData {
		
		public struct Tile {
			public byte id1;
			public byte id2;
			public float alpha;
		}
		
		public Tile[,] data = new Tile[Config.MapLength,Config.MapLength];
		
		public MapDataGround (Util.Map.Location map) {
			this.map = map;
			this.file = WorldConfig.FILE_GROUND;
			
			byte[] bytes = Load();
			if (bytes!=null) Parse(bytes);
		}
		
		
		void Parse(byte[] bytes) {
			ParseGround(ref bytes);
			
			int pos = 2;
			for (int x = 0; x < Config.MapLength; ++x) {
				for (int y = 0; y < Config.MapLength; ++y) {
					data[x,y].id1 = bytes[pos];
					pos++;
				}
			}
			
			for (int x = 0; x < Config.MapLength; ++x) {
				for (int y = 0; y < Config.MapLength; ++y) {
					data[x,y].id2 = bytes[pos];
					pos++;
				}
			}
			
			for (int x = 0; x < Config.MapLength; ++x) {
				for (int y = 0; y < Config.MapLength; ++y) {
					data[x,y].alpha = 1-(float)bytes[pos]/255;
					pos++;
				}
			}
		}
		
	}
	
}
