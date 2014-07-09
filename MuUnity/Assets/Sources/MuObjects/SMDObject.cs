
using UnityEngine;
using System.Collections;

public class SMDObject : MonoBehaviour {
	
	protected Transform SMD;
	protected Renderer SMDrendered;
	
	protected void InitSMD() {
		SMD = gameObject.transform.FindChild("SMDImport");
		if (SMD!=null)
			SMDrendered = SMD.renderer;
		else {
			Debug.LogWarning("Not found SMD");	
		}
	}
	
}
