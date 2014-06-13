using UnityEngine;
using System.Collections;

namespace MuPlayer {
	public class PlayerData : MonoBehaviour {
		
		private float _Dir;
		
		public Util.Player.Class Class; //класс
		public MuCoord Coord; //координата
		public Vector3 Position; //координата в векторе
		public bool isSwim = false; //переменная для того, чтобы поворачивать объект по X
		public Quaternion Rotate; //угол
		public bool isCharacter; //игрок которым управляют
		
		public float Dir {
			get { return _Dir; }
			set {
				_Dir = value;
				Rotate = Quaternion.Euler((isSwim ? 90f : 0f), value, 0f);
			}
		}
		
		public IEnumerator SmoothRotation(Transform body) {
			
			float Speed = 10f;
			while (Mathf.Abs(Quaternion.Dot(body.localRotation, Rotate))!=1) {
				body.localRotation = Quaternion.Slerp(body.localRotation, Rotate, Time.deltaTime*Speed);
				yield return null;
			}
			
			yield break;
			
		}
		
		
	}
	
}
