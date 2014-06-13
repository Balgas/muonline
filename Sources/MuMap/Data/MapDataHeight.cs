
using System.Collections;
using System;

namespace MuMap {
	
	public class MapDataHeight : MapData {
		
		public float[,] data = new float[Config.MapLength,Config.MapLength];
		
		public MapDataHeight (Util.Map.Location map) {
			this.map = map;
			this.file = WorldConfig.FILE_HEIGHT;
			
			byte[] bytes = Load();
			if (bytes!=null) Parse(bytes);
		}
		
		void Parse(byte[] bytes) {
			int pos = 1082;
			
			for (int x = 0; x < Config.MapLength; ++x) {
				pos+=2;
					for (int y = 0; y < Config.MapLength-2; ++y) {
					data[x,y] = (float)(((float)bytes[pos]/255)*(1f-Config.HoleHeight)+Config.HoleHeight);
					
					pos++;
				}
			}
		}
		
		
	}
	
}
