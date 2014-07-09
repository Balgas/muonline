using UnityEngine;
using System.Collections;

namespace MuUI {
	
	public class UIComponent : MonoBehaviour {
	
		public bool isHover = false;
		
		public Align align {
			set; get;	
		}
		
		public enum Align {
			LowerCenter,
			LowerLeft,
			LowerRight,
			MiddleCenter,
			MiddleLeft,
			MiddleRight,
			UpperCenter,
			UpperLeft,
			UpperRight
		}
		
	}
	
}
