
using UnityEngine;
using System.Collections;
using MuUI;
using MuGlobal;

public class Scene : MonoBehaviour {
	
	protected Global global;
	protected UI ui;
	
	protected void InitGlobal () {
		global = Global.instance;
		if (global==null && GameObject.Find("Global")==null) {
			GameObject globalGO = new GameObject("Global");
			globalGO.AddComponent<Global>();
			globalGO.isStatic = true;
			global = Global.instance;
			global.Init();
		}
	}
	
	protected void InitUI () {
		GameObject findUI = GameObject.Find("UI");
		if (findUI==null) {
			GameObject UIGO = new GameObject("UI");
			ui = UIGO.AddComponent<UI>();
		} else {
			ui = findUI.GetComponent<UI>();
			if (ui==null)
				ui = findUI.AddComponent<UI>();
		}
		
	}
	
	protected IEnumerator NextScene(string name, string loaderUI = null)  {
		if (loaderUI==null)  {
	 		ui.add("LoaderUI");
	 		loaderUI = "LoaderUI";
		}
		
		AsyncOperation async = Application.LoadLevelAsync(name);
		
		while (!async.isDone) {
	 	 	
			ui.send(loaderUI, "SetProgress", async.progress);
 	 		yield return null;
			
		}
		
	}
	
	protected void OnEnable () {
		Application.RegisterLogCallback(Util.Debug.LogCallback);
	}
	
	protected void OnDisable () {
		Application.RegisterLogCallback(null);
	}
	
}
