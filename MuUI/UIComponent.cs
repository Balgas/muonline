
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MuUI {
	
	public class UIComponent : UIEffect {
		
		public bool isHover = false;
		public bool ignoreRaycast = true;
		
		protected Rect rect;
		protected Dictionary<string, IElement> elements = new Dictionary<string, IElement>();
		
		//уничтожает объект
	 	public void remove() {
	 	 	Destroy(gameObject);
		}
		
		//возвращает элемент по имени
		public IElement GetElement(string name) {
			return elements[name];
		}
		
		protected IElement Add(string name, string className) {
			elements[name] = (IElement)gameObject.AddComponent(className);
			elements[name].rect = UIRect.New(rect, elements[name]);
			elements[name].Init();
			return elements[name];
		}
		
		void OnGUI() {
			if (ignoreRaycast) //если игнорируется рэйкаст то проверяется наведен ли курсор
				DetectHover();
			else isHover = false;
			
			ApplyColor();
			GUILayout.BeginArea(rect);
			foreach (KeyValuePair<string,IElement> element in elements) {
				element.Value.Show();	
			}
			GUILayout.EndArea();
		}
		
		void DetectHover() {
			isHover = rect.Contains(Event.current.mousePosition);
		}
		
		
	}
	
}