
using UnityEngine;
using System.Collections;
using System;
using MuGlobal;

namespace MuMap {
	
	public class MapDataObjects : MapData {
		
		public ArrayList[,] data = new ArrayList[Config.MapLength, Config.MapLength];
		
		public MapDataObjects (Util.Map.Location map) {
			this.map = map;
			this.file = WorldConfig.FILE_OBJECT;
			
			byte[] bytes = Load();
			if (bytes!=null) Parse(bytes);
		}
		
		void Parse(byte[] bytes) {
			//int count = (bytes.Length-3)/30;
			
			uint j = 0;
			for (int i = 3; i < bytes.Length; i+=30) {
				MapObject obj = new MapObject();
				
				int id = BitConverter.ToUInt16(bytes, i)+1;
				Vector3 position = new Vector3( 
					BitConverter.ToSingle(bytes, i+2),
					BitConverter.ToSingle(bytes, i+10)+Config.TileHeight*Config.HoleHeight+Config.FixHeightObject,
					BitConverter.ToSingle(bytes, i+6)
				);
				Vector3 rotate = new Vector3(
					BitConverter.ToSingle(bytes, i+14),
					180f-BitConverter.ToSingle(bytes, i+22),
					BitConverter.ToSingle(bytes, i+18)
				);
				float scale = BitConverter.ToSingle(bytes, i+26);
				MuCoord coord = Util.Map.Vector3ToCoords(position);
				
				//если объект выходит за пределами карты пропускаем его
				if (coord.x<0 || coord.x>=Config.MapLength || coord.y<0 || coord.y>=Config.MapLength) continue;
				
				obj.id = id;
				obj.position = position;
				obj.rotate = rotate;
				obj.scale = scale;
				obj.isSpecial = isSpecial(obj.id);
				obj.num = j;
				j++;
				
				if (data[coord.x,coord.y]==null) {
					data[coord.x,coord.y] = new ArrayList();	
				}
				
				data[coord.x,coord.y].Add(obj);
				
			}
			
		}
		
		bool isSpecial(int id) {
			bool b = false;
			int[] objects = WorldConfig.GetSpecialObjects(map);
			for (int i = 0; i < objects.Length; ++i) {
				if (objects[i]==id) {
					b = true;
					break;
				}
			}
			return b;
		}
		
	}
	
}
