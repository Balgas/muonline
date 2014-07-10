using UnityEngine;
using System.Collections;

namespace Util {
	
	public static class GO {
		
		public enum Layer:int {
			Terrain = 8,
			Players = 9,
			WorldObjects = 10,
			Effects = 11,
			UI = 12
		}
		
		public enum Tag:int {
			UI,
			Character,
			World
		}

		public static void DestroyChildrens ( GameObject parent ) {
			if (parent == null) return;
			while (parent.transform.childCount>0) {
				GameObject.DestroyImmediate(parent.transform.GetChild(0));
			}
		}

		public static GameObject Create ( string name, Transform parent ) {
			GameObject go = new GameObject ( name );
			SetParent ( go.transform, parent );
			return go;
		}
		
		public static GameObject Create ( string name, Transform parent, Tag tag ) {
			GameObject go = Create ( name, parent );
			SetTag ( go, tag );
			return go;
		}
		
		public static GameObject Create ( string name, Transform parent, Layer layer ) {
			GameObject go = Create ( name, parent );
			SetLayer ( go, layer );
			return go;
		}
		
		public static GameObject Create ( string name, Transform parent, Layer layer, Tag tag ) {
			GameObject go = Create ( name, parent );
			SetLayer ( go, layer );
			SetTag ( go, tag );
			return go;
		}
		
		public static void SetParent (Transform child, Transform parent) {
			child.parent = parent; 
			child.localPosition = Vector3.zero;
		}
		
		public static void SetScale ( GameObject go, float x, float y, float z ) {
			go.transform.localScale = new Vector3(x, y, z);
		}
		
		public static void SetTag(GameObject go, Tag tag) {
			go.tag = tag.ToString();
		}
		
		public static void SetLayer(GameObject go, Layer layer) {
			go.layer = (int)layer;
		}
		
		public static void SetAnimator(GameObject go, string resourceController) {
			Object controller = Resources.Load(resourceController);
			if (controller!=null) {
				Animator animator = go.GetComponent<Animator>();
				animator.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(controller);
				animator.speed = Config.AnimationSpeedWorldObjects;
				animator.applyRootMotion = false;
			}
		}
		
		public static GameObject GetTag( Tag tag ) {
			return GameObject.FindWithTag( tag.ToString() );	
		}	
		
		public static GameObject CreateAdvancedObject(string name, Transform parent, string resourceObject, string resourceController, Vector3 position, Vector3 rotation, Layer layer, float scale) {
			GameObject obj = Util.Storage.LoadPrefab(resourceObject); if (obj==null) return null;
			GameObject muObject = (GameObject)MonoBehaviour.Instantiate(obj, Vector3.zero, Quaternion.identity);
			muObject.name = name;
			
			if (resourceController!=null) 
				SetAnimator ( muObject, resourceController );
			
			SetLayer	(muObject, layer);
			SetParent	(muObject.transform, parent);
			SetScale	(muObject, scale*Config.ScaleObject.x, scale*Config.ScaleObject.y, scale*Config.ScaleObject.z);
			
			muObject.transform.position = position;
			muObject.transform.localRotation = Quaternion.Euler(rotation);
			
			return muObject;
		}
		
	}
	
}
