using UnityEngine;
using System.Collections;

namespace MuUI {
	
	
	public class Button : Element, IElement {
		
		public delegate void ButtonClick();
		public event ButtonClick Click;
		
		public UITexture texture;
		
		private int state = 0;
		
		public int stateNormal	= 0;
		public int stateHover	= 1;
		public int stateDown	= 2;
		
		public Button(string path) {
			texture = new UITexture(path);
		}
		
		public void Init() {
			if (texture==null) return;
			texture.CreateSprite(left, top);
			rect = texture.rect;
			state = stateNormal;
		}
		
		public void Show() {
			if (texture==null) return;
			//определяем стейт
			DetectHover();
			DetectDown();
			DetectClick();
			//устанавливаем стейт
			if(isHover) state = stateHover;
			else state = stateNormal;
			if (isDown) state = stateDown;
			
			if(isClick && Click != null) Click();
			
			texture.Draw(state);
		}
		
		
		
		
		
	}
}
