
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MuUI;

namespace MuMap {
	public class WorldController : WorldScene {
		
		//private Dictionary<string, string> SettingsGraphics = new Dictionary<string, string>() {
		//	{ "Objects", "checkbox" }, { "FOG", "checkbox" }, { "Target", "checkbox" }, { "Grass", "checkbox" }, { "ObjectAnims", "checkbox" }, { "GrassAnims", "checkbox" }
		//};
		
		public IEnumerator Init () {
			
			//инициализация глобальных компонентов
			this.InitGlobal();
			this.InitUI();
			this.InitWorld();
			
			//применяем настройки
			InitSettings();
			
			yield break;
		}
		
		
		void InitSettings() {
			
			//настройки рендера
			RenderSettings.fogColor = cam.backgroundColor = WorldConfig.GetBackground(global.map);
			RenderSettings.fogEndDistance = cam.farClipPlane;
			RenderSettings.fogStartDistance = RenderSettings.fogEndDistance-Config.FogLenght;
			
			
			//применяются глобальные настройки
			SetFOG			(global.settings.FOG);
			SetActiveObject	(global.settings.RenderWorldObjects);
			
			//skybox по сути нам не нужен
			//RenderSettings.skybox = Util.Storage.LoadMaterialFromResources("Sky/SkyBox");
		}
		
		/*
		void Update () {
			bool inputMenu = Input.GetKeyUp(KeyCode.Space);
			#if UNITY_ANDROID && !UNITY_EDITOR
				inputMenu = Input.GetKeyUp(KeyCode.Menu);
			#endif
			if (inputMenu) {
				SwitchMenu();	
			}
		}
		
		//открыть/закрыть настройки
		void SwitchMenu () {
			if (ui) {
				if (ui.get("SettingsUI")==null) CreateSettings(SettingsGraphics);
				else RemoveSettings();
			}
		}
		
		//удаляем настройки
		void RemoveSettings() {
			ui.remove("SettingsUI");
		}
		
		//создаем настройки передав параметры
		void CreateSettings(Dictionary<string, string> items) {
			SettingsUI settings = ui.add("SettingsUI") as SettingsUI;
			//отправляем необходимые данные
			settings.SetSettings(items);
			foreach (KeyValuePair<string, string> setting in items) {
				if (setting.Value == "checkbox") {
					string name = setting.Key;
					Checkbox box = (Checkbox)settings.GetElement(name) as Checkbox;
					box.isChecked = GetValueCheckbox(name);
					SetValueCheckbox(name, box);
				}
			}
			
		}
		
		//возвращает значение чекбокса
		bool GetValueCheckbox(string name) {
			bool b = false;
			switch (name) {
			case "Objects":		b = global.settings.RenderWorldObjects; break;
			case "FOG":			b = global.settings.FOG; break;
			case "Target":		b = global.settings.ShowMoveTarget; break;
			case "Grass":		b = global.settings.RenderGrass; break;
			case "ObjectAnims":	b = global.settings.AnimatedWorldObjects; break;
			case "GrassAnims":	b = global.settings.AnimatedGrass; break;
			}
			return b;
		}
		
		//применяет значения чекбокса
		void SetValueCheckbox(string name, Checkbox box) {
			switch (name) {
			case "Objects":		box.Check += SetActiveObject; break;
			case "FOG":			box.Check += SetFOG; break;
			case "Target":		box.Check += SetTarget; break;
			case "Grass":		box.Check += SetGrass; break;
			case "ObjectAnims":	box.Check += SetObjectsAnims; break;
			case "GrassAnims":	box.Check += SetGrassAnims; break;
			}
		}
		*/
		
		//отправляет в настройки инфу
		void SetInfoToSettings(string code) {
			//if (ui.get("SettingsUI")!=null)
				//((SettingsUI)ui.get("SettingsUI") as SettingsUI).SetInfo(code);
		}
		
		
		//выкл/откл объекты на сцене
		void SetActiveObject(bool status) {
			if (world.map.objects.go==null) return;
			
			global.settings.RenderWorldObjects = status;
			world.map.objects.go.SetActive(global.settings.RenderWorldObjects);
		}
		
		//выкл/вкл FOG
		void SetFOG(bool status) {
			global.settings.FOG = status;
			RenderSettings.fog = global.settings.FOG;
		}
		
		//выкл/вкл анимированого таргета
		void SetTarget(bool status) {
			global.settings.ShowMoveTarget = status;
		}
		
		//выкл/вкл рендера травы
		void SetGrass(bool status) { 
			global.settings.RenderGrass = status;
			SetInfoToSettings("SettingsApplyNextScene");
		}
		
		//выкл/вкл анимации объектов
		void SetObjectsAnims(bool status) {
			global.settings.AnimatedWorldObjects = status;
			SetInfoToSettings("SettingsApplyNextScene");
		}
		
		//выкл/вкл анимации травы
		void SetGrassAnims(bool status) {
			global.settings.AnimatedGrass = status;
			world.map.terrain.SetGrassSpeed(status);
		}
		
	}
}
