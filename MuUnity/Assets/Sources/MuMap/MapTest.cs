
using UnityEngine;
using System.Collections;

namespace MuMap {
	
	public class MapTest : WorldScene {
		
		void Start() {
			InitGlobal();
			InitUI();
			
			//если трава включена у кого-то то выключим ему
			if (!global.settings.RenderGrass) {
				global.settings.RenderGrass = true;
				global.settings.AnimatedGrass = true;
			}
		}
		
		void OnGUI() {
			if (GUI.Button(new Rect(10f, 10f, 100f, 20f), "Lorencia")) {
				global.map = Util.Map.Location.Lorencia;
				StartCoroutine ( NextScene("MuWorld") );
			}
			if (GUI.Button(new Rect(10f, 40f, 100f, 20f), "Devias")) {
				global.map = Util.Map.Location.Devias;
				StartCoroutine ( NextScene("MuWorld") );
			}
			if (GUI.Button(new Rect(10f, 70f, 100f, 20f), "Noria")) {
				global.map = Util.Map.Location.Noria;
				StartCoroutine ( NextScene("MuWorld") );
			}
			if (GUI.Button(new Rect(10f, 100f, 100f, 20f), "Dungeun")) {
				global.map = Util.Map.Location.Dungeun;
				StartCoroutine ( NextScene("MuWorld") );
			}
			if (GUI.Button(new Rect(10f, 130f, 100f, 20f), "Atlans")) {
				global.map = Util.Map.Location.Atlans;
				StartCoroutine ( NextScene("MuWorld") );
			}
			if (GUI.Button(new Rect(10f, 160f, 100f, 20f), "LostTower")) {
				global.map = Util.Map.Location.LostTower;
				StartCoroutine ( NextScene("MuWorld") );
			}
			if (GUI.Button(new Rect(10f, 190f, 100f, 20f), "Stadium")) {
				global.map = Util.Map.Location.Stadium;
				StartCoroutine ( NextScene("MuWorld") );
			}
			if (GUI.Button(new Rect(10f, 220f, 100f, 20f), "Tarkan")) {
				global.map = Util.Map.Location.Tarcan;
				StartCoroutine ( NextScene("MuWorld") );
			}
			if (GUI.Button(new Rect(10f, 250f, 100f, 20f), "BloodCastle")) {
				global.map = Util.Map.Location.BloodCastle;
				StartCoroutine ( NextScene("MuWorld") );
			}
			if (GUI.Button(new Rect(10f, 280f, 100f, 20f), "DevilSquare")) {
				global.map = Util.Map.Location.DevilSquare;
				StartCoroutine ( NextScene("MuWorld") );
			}
			
			
			if (coord!=null)
				GUI.Label(new Rect(Screen.width-160, 5, 160, 25), coord.ToString());
		}
		
		
		public MuCoord coord {
			set; get;
		}
		
		
	}
	
}
