
using UnityEngine;
using System.Collections;

namespace MuMap {
	
	public class MapDataZones : MapData {
		
		public byte[,] data = new byte[Config.MapLength,Config.MapLength];
		
		
		public MapDataZones (Util.Map.Location map) {
			this.map = map;
			this.file = WorldConfig.FILE_ZONE;
			
			byte[] bytes = Load();
			if (bytes!=null) Parse(bytes);
		}
		
		
		void Parse(byte[] bytes) {
			ParseZones(ref bytes);
			
			int pos = 4;
			
			for (int x = 0; x < Config.MapLength; ++x) {
				for (int y = 0; y < Config.MapLength; ++y) {
					data[y,x] = bytes[pos];
					pos++;
				}
			}
			
		}
		
	}
	
}
