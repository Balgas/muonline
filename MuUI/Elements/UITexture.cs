using UnityEngine;
using System.Collections;

namespace MuUI {
	public class UITexture {
		
		public Texture texture;
		
		public int countX = 1;
		public int countY = 1;
		
		public Rect rect;
		
		public UITexture(string path) {
			texture = (Texture)Resources.Load(path, typeof(Texture));
		}
		
		public void CreateImage(float left, float top) {
			if (texture==null) return;
			rect = UIRect.FromTexture(left, top, texture);	
		}
		
		public void CreateSprite(float left, float top) {
			if (texture==null) return;
			CreateImage(left, top);
			UIRect.Resize(ref rect, rect.width/countX, rect.height/countY);
		}
		
		private Rect GetStateRect(int state) {
			float x = (countX>1 ? 1f-1f/countX*state-1f/countX : 0f);
			float y = (countY>1 ? 1f-1f/countY*state-1f/countY : 0f);
			return new Rect(x, y, (float)1/countX, (float)1/countY);
		}
		
		public void Draw(int state) {
			if (countX==1 && countY==1) 
				GUI.DrawTexture(rect, texture, ScaleMode.ScaleToFit, true);
			else
				GUI.DrawTextureWithTexCoords(rect, texture, GetStateRect(state), true);	
		}
		
		
	}
}