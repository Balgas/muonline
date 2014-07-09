using UnityEngine;
using System.Collections;

namespace MuUI {
	public class Checkbox : Element, IElement {
	
		public delegate void CheckboxCheck(bool status);
		public event CheckboxCheck Check;
		
		public UITexture texture;
		
		private int state = 0;
		
		public int stateNormal	= 1;
		public int stateCheck	= 0;
		
		public Checkbox(string path) {
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
			//проверяем нажатие
			DetectHover();
			DetectClick();
			//отправляем event нажатия
			if(isClick && Check != null) Check(SwitchCheck());
			//рисуем текстуру
			texture.Draw(state);
		}
		
		private bool SwitchCheck() {
			isChecked = !isChecked;
			return isChecked;
		}
		
		public bool isChecked {
			set { state = (value) ? stateCheck : stateNormal; }
			get { return state==stateCheck; }
		}
		
	}
}
