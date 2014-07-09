
/*
 * Отвечает за:
 * - создание поверхности
 * - изменени высот
 * - загрузку ресурсов на поверхность
 * - загрузка травы (отключена из-за бага)
 * 
 */

using UnityEngine;
using System.Collections;

namespace MuMap {
	
	public class MapTerrain : WorldScene {
		
		Terrain terrain;
		
		public Grass[] grasses;
		
		public void Init(Terrain _terrain, MapDataGround.Tile[,] tiles, float[,] height, byte[,] zones) {
			InitGlobal();
			InitWorld();
			
			terrain = _terrain;
			
			InitTerrain(tiles, height, zones);
			
		}
		
		void InitTerrain(MapDataGround.Tile[,] tiles, float[,] height, byte[,] zones) {
			int size = Config.TileSize;
			//получаем активный террейн
			terrain.transform.parent = this.transform;
			
			TerrainData data = terrain.terrainData;
			
			//устанавливаем видимость
			terrain.basemapDistance = terrain.detailObjectDistance = cam.farClipPlane;
			terrain.heightmapPixelError = Config.HeightmapPixelError;
			//устанавливаем абсолютную ширину террейна
			data.heightmapResolution = 257;
			//устанавливаем физическую ширину
			data.size = new Vector3(Config.MapLength*size, Config.TileHeight+Config.HoleHeight*Config.TileHeight, Config.MapLength*size);
			//получаем текстуры
			SplatPrototype[] prototypes = GetPrototypes();
			//загружаем текстуры
			data.splatPrototypes = prototypes;
			//устанавливаем ямы
			ApplyHole(ref tiles, ref height, zones, (byte)prototypes.Length);
			//устанавливаем высоты
			data.SetHeights(0, 0, height);
			if (prototypes.Length>0) {
				//формируем alphamaps
				float[,,] alphamaps = GetAlphamaps(tiles, prototypes.Length);
				//загружаем alphamaps
				data.SetAlphamaps(0, 0, alphamaps);
			}
			
			//получаем текстуры деталей
			DetailPrototype[] details = GetDetails();
			
			data.detailPrototypes = details;
			//устанавливаем разрешение травы
			data.SetDetailResolution(Config.MapLength, Config.GrassPerPatch);
			for (int i = 0; i<details.Length; ++i) {
				//загружаем слои трав
				data.SetDetailLayer(0, 0, i, GetDetailsMap(tiles, i));
			}
			
			
			
			//переопределенице цвета травы
			data.wavingGrassTint = Color.white;
			
			//устанавливаем разрешение для удаленных объектов
			data.baseMapResolution = 64;
			
			//устанавливаем запеченный свет
			terrain.lightmapIndex = 0;
			//применяем все параметры
			terrain.Flush();
			
			//очищаем переменные
			zones = null;
			height = null;
			prototypes = null;
			System.GC.Collect();
			
		}
		
		void OnApplicationQuit() {
			if (terrain!=null && terrain.terrainData!=null) {
				terrain.terrainData.splatPrototypes = null;
				//terrain.terrainData.detailPrototypes = null;
			}
		}
		
		
		//применяются ямы
		void ApplyHole(ref MapDataGround.Tile[,] tiles, ref float[,] height, byte[,] zones, byte count) {
			for (int y = 0; y < Config.MapLength; ++y) {
				for (int x = 0; x < Config.MapLength; ++x) {
					if (Util.Map.isHoleZone(zones[y,x])) {
						//устанавливаем цвет ямы по дефолту заливкой
						tiles[x,y].id1 = (byte)(count-1);
						tiles[x,y].id2 = 255;
						tiles[x,y].alpha = 1;
						//если левве или ниже не яма, тогда пропускаем ребро
						if ((y>0 && !Util.Map.isHoleZone(zones[y-1,x])) ||
							(x>0 && !Util.Map.isHoleZone(zones[y,x-1]))) {
							
							continue;
						}
						height[x,y] = 0;
					}
				}
			}
		}
		
		//ставит текстуры в координаты
		float[,,] GetAlphamaps(MapDataGround.Tile[,] tiles, int count) {
			float[,,] alphamaps = new float[Config.MapLength,Config.MapLength,count];
			for (int x = 0; x<Config.MapLength; ++x) {
				for (int y =0; y<Config.MapLength; ++y) {
					
					MapDataGround.Tile tile = tiles[x, y];
					
					bool f = false;
					if (tile.id1 < count && tile.alpha!=0.0f) {
						alphamaps[x, y, tile.id1] = tile.alpha; f = true;
					}
					
					if (tile.id2 < count && 1-tile.alpha!=0.0f) {
						alphamaps[x, y, tile.id2] = (f) ? 1 : 1-tile.alpha;
					}
					
				}
			}
			return alphamaps;
		}
		
		//устсанавливает на координаты траву
		int[,] GetDetailsMap(MapDataGround.Tile[,] tiles, int layer) {
			int[,] maps = new int[Config.MapLength,Config.MapLength];
			if (!global.settings.RenderGrass) return maps;
			for (int x = 0; x<Config.MapLength; ++x) {
				for (int y =0; y<Config.MapLength; ++y) {
					
					MapDataGround.Tile tile = tiles[x, y];
					
					float alpha = tile.id1==layer ? tile.alpha : 1f-tile.alpha;
					if ((tile.id1==layer || tile.id2==layer) && alpha>=0.5f) {
						maps[x,y] = Mathf.RoundToInt(alpha*Config.GrassPerPatch);
					}
				}
			}
			
			return maps;
		}
		
		DetailPrototype[] GetDetails() {
			grasses = WorldConfig.GetDetailsList(global.map);
			
			if (grasses.Length==0) return new DetailPrototype[0];
			
			string dir = Util.File.ObjectsStorageDir(global.map);
			
			DetailPrototype[] array = new DetailPrototype[grasses.Length];
			for (int i = 0; i<grasses.Length; ++i) {
				array[i] = GetDetail(dir, grasses[i]);
			}
			return array;
		}
		
		SplatPrototype[] GetPrototypes() {
			Util.Map.Location map = global.map;
			
			string[] textures = WorldConfig.GetTilesList(map);
			
			if (textures.Length==0) return new SplatPrototype[0];
			
			string dir = Util.File.WorldStorageDir(map);
			
			SplatPrototype[] array = new SplatPrototype[textures.Length+1];
			for (int i = 0; i<textures.Length; ++i) {
				if (textures[i]==null) array[i] = new SplatPrototype();
				else array[i] = GetSplat ( Util.File.DIRECTORY_DATA + dir + "/" + textures[i]+".jpg");
			}
			
			//simple texture from settings
			array[textures.Length] = new SplatPrototype();
			array[textures.Length].texture = Util.Data.CreateFillTexture(WorldConfig.GetBackground(map), 256, 256);
			array[textures.Length].tileSize = new Vector2(Config.TileSize, Config.TileSize);
			return array;
		}
		
		
		
		DetailPrototype GetDetail(string dir, Grass grass) {
			DetailPrototype detail = new DetailPrototype();
			if (grass==null) return detail;
			
			detail.maxHeight = grass.maxHeight;
			detail.minHeight = grass.minHeight;
			detail.minWidth = Config.GrassWidth;
			detail.maxWidth = Config.GrassWidth;
			detail.prototypeTexture = Util.Storage.LoadTextureFromResources(dir + grass.file);
			detail.noiseSpread = 0.1f;
			detail.dryColor = grass.dry;
			detail.healthyColor = grass.healthy;
			detail.renderMode = DetailRenderMode.GrassBillboard;
			return detail;
		}
		
		SplatPrototype GetSplat(string filename) {
			SplatPrototype splat = new SplatPrototype();
			Texture2D tex = Util.Storage.LoadTextureFromStorage(filename);
			splat.texture = tex;
			splat.tileSize = new Vector2(Config.TileSize, Config.TileSize);
			return splat;
		}
		
		//устанавливает/выключает скорость ветра
		public void SetGrassSpeed(bool status) {
			if (status) {
				terrain.terrainData.wavingGrassStrength = Config.GrassStrength;
				terrain.terrainData.wavingGrassSpeed	= Config.GrassSpeed;
				terrain.terrainData.wavingGrassAmount	= Config.GrassAmount;
			} else {
				terrain.terrainData.wavingGrassStrength =
					terrain.terrainData.wavingGrassSpeed = 
						terrain.terrainData.wavingGrassAmount = 0f;
			}
		}
		
		//возвращает высоту в точке
		public Vector3 GetHeight(MuCoord coord) {
			Vector3 v = Util.Map.CoordsToVector3(coord);
			return GetHeight(v);
		}
		
		//возвращает высоту в точке по вектору
		public Vector3 GetHeight(Vector3 v) {
			v.y = terrain.SampleHeight(v)+8;
			return v;
		}
		
	}
	
	
	public class Grass {
		public string file;
		public Color healthy;
		public Color dry;
		public int minHeight;
		public int maxHeight;
	}
	
}