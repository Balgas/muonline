using UnityEngine;
using System.IO;
using System.Collections;
using System.Xml;

namespace Util {
	
	public class Storage {
		
		public static byte[] Load(string file) {
			string filePath;
			byte[] result = new byte[0];
		
			filePath = Application.streamingAssetsPath+'/'+file;
			
			if (filePath.Contains("://") || Application.isWebPlayer) {
				WWW www = new WWW(filePath);
				while (!www.isDone) { }
	            result = www.bytes;
	    	} else {
				#if !UNITY_WEBPLAYER
				try {
					result = System.IO.File.ReadAllBytes(filePath);
				} catch {
					
				}
				#endif
			}
			return result;
	    }
		
		public static void LoadLang (ref Hashtable strings, string file) {
			TextAsset data = Util.Storage.LoadTextFromResources(file);
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(data.ToString());
			for(int i = 0; i<doc.ChildNodes.Count; i++) {
				XmlNode node = doc.ChildNodes.Item(i);
				if(node.Name=="resources") {
					for(int n = 0; n<node.ChildNodes.Count; n++) {
						XmlNode str = node.ChildNodes.Item(n);
						strings.Add(str.Attributes["name"].Value, str.InnerText);
					}
				}
			}
			
		}
		
		
		public static Texture2D LoadTextureFromStorage(string file) {
			byte[] img = Load(file);
			if (img.Length==0) return null;
			Texture2D t = new Texture2D(1, 1);
			t.LoadImage(img);
			img = null;
			return t;
		}
		
		public static Material LoadMaterialFromResources(string file) {
			Material obj = (Material)Resources.Load(file, typeof(Material));
			return obj;
		}
		
		public static Texture2D LoadTextureFromResources(string file) {
			Texture2D obj = (Texture2D)Resources.Load(file, typeof(Texture2D));
			return obj;
		}
		
		public static TextAsset LoadTextFromResources(string file) {
			TextAsset obj = (TextAsset)Resources.Load(file, typeof(TextAsset));
			return obj;
		}
		
		public static AudioClip LoadAudioFromResources(string file) {
			AudioClip obj = (AudioClip)Resources.Load(file, typeof(AudioClip));
			return obj;
		}
		
		public static GameObject LoadPrefab(string path) {
			GameObject obj = (GameObject)Resources.Load(path);
			return obj;
		}
		
		public static string UrlEncode(string instring) {
	        StringReader strRdr = new StringReader(instring);
	        StringWriter strWtr = new StringWriter();
	        int charValue = strRdr.Read();
	        while (charValue != -1)
	        {
	            if (((charValue >= 48) && (charValue <= 57)) // 0-9
	            || ((charValue >= 65)  && (charValue <= 90)) // A-Z
	            || ((charValue >= 97)  && (charValue <= 122))) // a-z
	            {
	                strWtr.Write((char) charValue);
	            }
	            else if (charValue == 32) // Space
	            {
	                strWtr.Write("+");
	            }
	            else
	            {
	                strWtr.Write("%{0:x2}", charValue);
	            }
	            charValue = strRdr.Read();
	        }
	        return strWtr.ToString();
	    }
		
		public static void WWWSend ( string url ) {
			MuGlobal.Global global = MuGlobal.Global.instance;
			if (global==null) return;
			global.WWWSend ( url );
		}
		
		
		
		
	}
	
}