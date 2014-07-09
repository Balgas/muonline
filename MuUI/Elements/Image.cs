using UnityEngine;
using System.Collections;

namespace MuUI {
	public class Image : Element, IElement {
		
		private UITexture texture;
		
		public Image( string path ) {
			texture = new UITexture(path);
		}
		
		public void Init() {
			if (texture==null) return;
			texture.CreateImage(left, top);
			rect = texture.rect;
		}
		
		public void Show() {
			if (texture==null) return;
			texture.Draw(0);
		}
		
	}
	
}
