
using UnityEngine;
using System.Collections;

public class Settings {
	
	public bool AnimatedWorldObjects	{ get { return GetBool("AnimatedWorldObjects"); }	set { SetBool ("AnimatedWorldObjects", value); } }
	public bool LoadMapObjectsSpecial	{ get { return GetBool("LoadMapObjectsSpecial"); }	set { SetBool ("LoadMapObjectsSpecial", value); } }
	public bool FOG						{ get { return GetBool("FOG"); }					set { SetBool ("FOG", value); } }
	public bool RenderWorldObjects		{ get { return GetBool("RenderWorldObjects"); }		set { SetBool ("RenderWorldObjects", value); } }
	public bool RenderGrass				{ get { return GetBool("RenderGrass", false); }		set { SetBool ("RenderGrass", value); } }
	public bool AnimatedGrass			{ get { return GetBool("AnimatedGrass", false); }	set { SetBool ("AnimatedGrass", value); } }
	public bool ShowMoveTarget			{ get { return GetBool("ShowMoveTarget"); }			set { SetBool ("ShowMoveTarget", value); } }
	public int BufferObjectsLength	 	{ get { return GetInt("BufferObjectsLength", 13); } set { if (value>Config.MinRadiusViewTiles && value<=Config.MaxRadiusViewTiles) SetInt("BufferObjectsLength", value); } }
	public float VolumeSound		 	{ get { return GetFloat("VolumeSound"); }			set { SetFloat("VolumeSound", value); } }
	
	bool IntToBool(int v) { return v==1; }
	int BoolToInt(bool v) { return (v ? 1 : 0); }
	
	bool GetBool(string name, bool _default = true) {
		return IntToBool(GetInt(name, BoolToInt(_default)));
	}
	
	void SetBool(string name, bool value) {
		SetInt(name, BoolToInt(value));
	}
	
	int GetInt(string name, int value = 0) {
		return PlayerPrefs.GetInt(name, value);
	}
	
	void SetInt(string name, int value = 0) {
		PlayerPrefs.SetInt(name, value);
	}
	
	float GetFloat(string name, float value = 1f) {
		return PlayerPrefs.GetFloat(name, value);
	}
	
	void SetFloat(string name, float value = 0) {
		PlayerPrefs.SetFloat(name, value);
	}
}
