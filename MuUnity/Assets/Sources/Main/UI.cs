
using UnityEngine;
using System.Collections;

namespace MuUI {
	
	public class UI : MonoBehaviour {
		
		public void error(string code, params object[] args) {
	 	 	string text = (args.Length>0) ? Lang.str ( code, args ) : Lang.str ( code );
	 	 	error(text);
		}
		
		public void error ( string text ) {
	 	 	Debug.LogWarning(text);
			
	 	 	add("ErrorUI");
			send("ErrorUI",  "SetText", text);
		}
		
		public void debug(string text) {
			if (get("DebugUI")==null)
				add("DebugUI");
			send("DebugUI", "Log", text);
		}
		
		public UIComponent add(string type) {
			GameObject go = Util.GO.Create (type, gameObject.transform, Util.GO.Layer.UI, Util.GO.Tag.UI );
			return go.AddComponent(type) as UIComponent;
		}
		
		public void remove(string type) {
			Destroy(GetComponent(type).gameObject);
		}
		
		public UIComponent get(string type) {
			Transform findGO = gameObject.transform.Find(type);
			if (findGO==null) return null;
			return findGO.GetComponent(type) as UIComponent;
		}
		
		public void send(string type, string method, object value) {
			if (get (type)!=null)
				get(type).SendMessage(method, value);	
		}
		
		public bool isHover() {
			bool hover = false;
			UIComponent[] components = gameObject.GetComponents<UIComponent>();
			foreach (UIComponent element in components) {
				hover = element.isHover;
				if (hover) break;
			}
			return hover;
		}
		
	}
	
}