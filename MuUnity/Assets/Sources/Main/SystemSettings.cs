
using UnityEngine;
using System.Collections;
using MuUI;

public class SystemSettings : MonoBehaviour {

	void Start () {
		Screen.sleepTimeout = Config.SleepTimeout;
	}

	
}
