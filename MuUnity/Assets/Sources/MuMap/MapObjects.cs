
/* - Загрузка объектов на сцену
 * - Загрузка анимации и привязка к объекту
 * - LOD система обновления объектов
 */

using UnityEngine;
using System.Collections;

namespace MuMap {
	
	public struct MapObject {
		public uint num;
		public int id;
		public Vector3 position;
		public Vector3 rotate;
		public float scale;
		public bool isSpecial;
	}
	
	public class MapObjects : WorldScene {
		
		public delegate void DelegateUpdateLOD();
		public event DelegateUpdateLOD EventUpdateLOD;
		
		public GameObject go;
		
		protected ArrayList nums = new ArrayList(); 	//список текущих объектов
		protected int bufferLength = 0;					//длина буффера из глобалс
		protected MuCoord lastUpdatedPoint;				//последняя точка обновления
		protected ArrayList[,] data;					//массив объектов
		
		public void Init ( ArrayList[,] _data ) {
			
			this.InitGlobal();
			this.InitWorld();
			
			bufferLength = global.settings.BufferObjectsLength;
			data = _data;
			
			go = Util.GO.Create ( "Objects", scene.transform, Util.GO.Layer.WorldObjects );
			
		}
		
		//изменение координат игрока
		public void ChangeCoord(MuCoord coord) {
			if ((object)lastUpdatedPoint==null) {
				//если последний точки небыло, значит карта еще не создана
				lastUpdatedPoint = coord;
				RuntimeAround(coord);
			} else if (NeedUpdate(coord)) {
				RuntimeAround(coord);
			}
		}
		
		//проверка на обновление и запись последней точки обновления
		private bool NeedUpdate(MuCoord coord) {
			bool status = false;
			
			int dist = (int)Mathf.Ceil(Util.Map.DistanceBetweenTwoPoints(lastUpdatedPoint, coord));
			if (dist>=bufferLength-Config.MinRadiusViewTiles)
				status = true;
			
			if (status) //обновляем точку
				lastUpdatedPoint = coord;
			return status;	
		}
		
		//отправляет событие обновления
		private void SendUpdatedLOD() {
			if (EventUpdateLOD!=null) {
				EventUpdateLOD();
			}
		}
		
		//создает объекты вокруг точки
		private void RuntimeAround(MuCoord coord) {
			ArrayList added = new ArrayList();
			ArrayList removed = new ArrayList();
			
			Loom.RunAsync(()=>{
		        
				// НАЧАЛО НОВОГО ПОТОКА
				
				//создание
				for (int x = coord.x-bufferLength; x<=coord.x+bufferLength; ++x) {
					for (int y = coord.y-bufferLength; y<=coord.y+bufferLength; ++y) {
						if (x<0 || y<0 || x>=Config.MapLength || y>=Config.MapLength) continue;
						else {
							if (data[x,y]!=null) {
								
								string xy = GetXY(x, y);
								
								if (!nums.Contains(xy)) {
									nums.Add(xy);
								}
								added.Add(xy);
								
							}
						}
					}
				}
				//удаление
				ArrayList list = (ArrayList)nums.Clone();
				foreach (string n in list) {
					if (!added.Contains(n)) {
						//удаляем из списка видимых
						nums.Remove(n);
						removed.Add(n);
					}
				}
				list = null;
				
				// КОНЕЦ ГЛАВНОГО ПОТОКА
				
				Loom.QueueOnMainThread(()=>{
					
					StartCoroutine ( UpdateLOD(added, removed) );
					
					added = null;
					removed = null;
					
				});
				
		    });
			
		}
		
		IEnumerator UpdateLOD(ArrayList added, ArrayList removed) {
			foreach (string add_id in added) {
				yield return StartCoroutine ( SwitchActiveObject(add_id, true) );
			}
			foreach (string remove_id in removed) {
				yield return StartCoroutine ( SwitchActiveObject(remove_id, false) );
			}
			
			SendUpdatedLOD();
			
			yield break;
		}
		
		IEnumerator SwitchActiveObject(string xy, bool isActive) {
			Transform find = go.transform.Find(xy);
			if (find!=null) {
				find.gameObject.SetActive(isActive);	
			} else if (isActive) {
				MuCoord coord = GetCoordXY ( xy );
				yield return StartCoroutine ( CreateXY ( coord ) );
			}
			yield break;
		}
		
		
		private IEnumerator CreateXY ( MuCoord coord ) {
			int x = coord.x;
			int y = coord.y;
			
			string x_y = GetXY(x, y);
			GameObject xy = new GameObject(x_y);
			xy.transform.parent = go.transform;
			
			foreach (MapObject obj in data[x,y]) {
				yield return StartCoroutine ( CreateWorldObject(xy, obj) );
			}
			
			if (xy.transform.childCount==0)
				//если объектов 0, то он нам не нужен
				Destroy(xy);
			
			yield break;
		}
		
		IEnumerator CreateWorldObject(GameObject xy, MapObject mapObject) {
			
			if (mapObject.isSpecial) yield break; //временно не добавляем спецобъекты
			Util.Map.Location map = global.map;
			
			string dir = Util.File.ObjectsStorageDir(map);
			string file = dir + "Object"+mapObject.id.ToString();
			string contr = dir + "Controller"+mapObject.id.ToString();
			GameObject obj = Util.Storage.LoadPrefab(file);
			GameObject muObject = null;
			
			if (obj!=null) {
				
				muObject = (GameObject)Instantiate(obj,Vector3.zero, Quaternion.identity);
				muObject.name = "Obj"+mapObject.id.ToString();
				
				Transform SMD = muObject.transform.FindChild("SMDImport");
				if (SMD!=null) {
					SkinnedMeshRenderer mesh = SMD.gameObject.GetComponent<SkinnedMeshRenderer>();
					if (mesh!=null) {
						mesh.castShadows = mesh.receiveShadows = false;
					}
				}
				
				if (global.settings.AnimatedWorldObjects) 
					Util.GO.SetAnimator ( muObject, contr );
				
				
				Util.GO.SetParent (muObject.transform, xy.transform);
				Util.GO.SetLayer (muObject, Util.GO.Layer.WorldObjects);
				
				muObject.transform.position = mapObject.position;
				muObject.transform.localScale = new Vector3(mapObject.scale*Config.ScaleObject.x, mapObject.scale*Config.ScaleObject.y, mapObject.scale*Config.ScaleObject.z);
				muObject.transform.localRotation = Quaternion.Euler(mapObject.rotate);
				
				//добавляем AI на объект (если есть)
				System.Type type = WorldConfig.GetAIObjects(map, mapObject.id);
				if (type!=null) {
					muObject.AddComponent(type);
				}
				
				muObject.isStatic = true;
				
			} else {
				if (Config.ErrorWorldObjectNotFound)
					Debug.Log(Lang.str("ErrorCreateObject", file));
			}
			
			yield break;
		}
		
		//имя объекта по x и y
		private string GetXY(int x, int y) {
			return x.ToString()+"_"+y.ToString();	
		}
		
		//координата по x_y
		private MuCoord GetCoordXY(string xy) {
			string[] split = xy.Split('_');
			return new MuCoord(){ x = int.Parse(split[0]), y = int.Parse(split[1]) };	
		}
		
		
	}
	
}
