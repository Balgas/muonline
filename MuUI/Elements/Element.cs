using UnityEngine;
using System.Collections;

namespace MuUI {
	
	public class Element : MonoBehaviour {
		
		protected bool isHover	= false;
		protected bool isDown	= false;
		protected bool isClick	= false;
		
		public float top = 0f;
		public float left = 0f;
		public float right = 0f;
		public float bottom = 0f;
		
		private float minIntervalClick = 0.1f;
		private float lastClickFrame = 0f;
		
		public Element() {
			style = new GUIStyle();
		}
		
		public GUIStyle style {
			set; get;	
		}
		
		public Rect rect {
			set; get;	
		}
		
		protected void DetectHover() {
			isHover = rect.Contains(Event.current.mousePosition);
		}
		
		protected void DetectDown() {
			isDown = (isHover && Input.GetMouseButton(0));
		}
		
		protected void DetectClick() {
			isClick = (isHover && Input.GetMouseButtonUp(0));
			if (isClick) {
				if (lastClickFrame==0f || Time.time-lastClickFrame>minIntervalClick) lastClickFrame = Time.time;
				else isClick = false;
			}
		}
		
		
		
		
		
		
	}
	
	public interface IElement {
		
		GUIStyle	style	{ set; get; }
		Rect		rect	{ set; get; }
		
		void Show();
		void Init();
		
	}
}
