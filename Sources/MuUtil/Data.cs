using UnityEngine;
using System.Collections;

namespace Util {
	
	public class Data {
	
		//ковертирует инт в двухзначную строку
		public static string IntToDoubleString(int i) {
			string str = "";
			if (i<10) str = "0"+i.ToString();
			else str = i.ToString();
			return str;
		}
		
		//создать новую текстуру с закрашенным цветом
		public static Texture2D CreateFillTexture(Color color, int width, int height) {
			Texture2D tex = new Texture2D(width, height);
			Color[] colors = new Color[width*height];
			int i = 0;
			for (int x = 0; x < width; ++x) {
				for (int y = 0; y<height; ++y) {
					colors[i] = color;
					i++;
				}
			}
			tex.SetPixels(colors);
			tex.Apply();
			return tex;
		}
		
	}

}