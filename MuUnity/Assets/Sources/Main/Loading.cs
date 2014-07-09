
using UnityEngine;
using System.Collections;

public class Loading : Scene {
	
	void Start () {
		
		this.InitGlobal();
		this.InitUI();
		
		//добавляем UI
		ui.add ("LoadingUI");
		
		ui.send ("LoadingUI", "SetMaxParts", 2);
		
		//загружаем файлы для шифрования
		MuPacket.MuEncDec.LoadKeys (Util.File.DIRECTORY_KEYS);
		
		//если файлы не загружены геймовер
		if (!MuPacket.MuEncDec.LoadedKey) {
			ui.error("NotFoundDataFiles");
			return; 
		} else {
			ui.send ("LoadingUI", "SetPart", 1);
		}
		
	 	//загружаем следующую карту
		StartCoroutine(NextScene("MuWorld", "LoadingUI"));
		
	}
	
}
