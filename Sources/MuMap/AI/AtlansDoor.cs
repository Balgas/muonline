
using UnityEngine;
using System.Collections;

namespace MuMap.AI {
	public class AtlansDoor : SMDObject {
		
		/*private int _count = 32;
		private int _current = 0;
		private int _fps = 15;
		private string _textureName = "wt";
		private Texture2D[] _textures;
		private int _lastIndex = -1;
		*/
		void Start () {
			/*InitSMD();
			
			if(SMDrendered == null) enabled = false;
			else {
				_textures = new Texture2D[_count];
				string dir = MapFunctions.ConvertMapObject(Util.Map.Location.Atlans);
				for (int i = 0; i<_count; i++) {
					string file = dir + _textureName + MuGlobal.GlobalMethods.GetStringInt(i);
					Texture2D tex = Util.Storage.LoadTextureFromResources(file);
					if (tex!=null) {
						_textures[i] = tex;	
					}
				}
			}*/
		}
		
		void Update () {
			/*if (SMDrendered!=null) {
				int index = (int)(Time.timeSinceLevelLoad * _fps) % _count;
 
				if(index != _lastIndex) {
					
					SMDrendered.material.SetTexture("_MainTex", _textures[_current]);
					SMDrendered.material.SetColor ("_Color", new Color(1, 0, 0, 1));
					
					_current ++;
					if (_current==_count) _current = 0;
					_lastIndex = index;
				}
			}*/
		}
	}
}
