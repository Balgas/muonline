using UnityEngine;
using System.Collections;

namespace MuUI {
	/* добавочный класс для работы с Rect */
	public static class UIRect {
	
		//добавляет координаты к Rect
		public static void AddOffset(this Rect r, float x, float y) {
			r.x += x;
			r.y += y;	
		}
		
		//добавляет координаты к Rect
		public static void Offset(this Rect r, float x, float y) {
			r.x = x;
			r.y = y;	
		}
		
		//добавляет высоту к Rect
		public static void AddHeight(this Rect r, float height) {
			r.height += height;
		}
		
		public static Rect Zero() {
			return new Rect(0f, 0f, 0f, 0f);
		}
		
		//изменяет в ссылке высоту и ширину
		public static void Resize(ref Rect r, float width, float height) {
			r.width = width;
			r.height = height;
		}
		
		//новый Rect
		public static Rect New(float width, float height) {
			Rect r = Zero();
			Resize(ref r, width, height);
			return r;
		}
		
		//новый Rect
		public static Rect New(Rect rect, IElement element) {
			Element e = element as Element;
			Rect r = Zero();
			r.x = e.left;
			r.y = e.top;
			r.width = rect.width-e.left-e.right;
			r.height = rect.height-e.top-e.bottom;
			return r;
		}
		
		//Rect текстуры
		public static Rect FromTexture(Texture texture) {
			return FromTexture(0, 0, texture);
		}
		
		//Rect текстуры
		public static Rect FromTexture(float x, float y, Texture texture) {
			return new Rect(x, y, (float)texture.width, (float)texture.height);
		}
		
		//возвращает Rect по центру View
		public static Rect ScreenCenterMiddle(float width, float height) {
			return new Rect((Screen.width - width)/2, (Screen.height - height)/2, width, height);	
		}
		
		
		
	}
}
