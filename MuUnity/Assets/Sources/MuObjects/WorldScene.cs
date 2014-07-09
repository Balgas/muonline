
using UnityEngine;
using System.Collections;

public class WorldScene : Scene {
	
	protected GameObject scene;
	
	protected World world;
	protected Camera cam;
	
	protected void InitWorld() {
		
		cam = Camera.main;
		scene = Util.GO.GetTag ( Util.GO.Tag.World );
		world = cam.GetComponent<World>();
		
	}
	
}
