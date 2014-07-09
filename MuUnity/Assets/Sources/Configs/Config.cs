
using UnityEngine;
	
public static class Config {
	
	
	public static int		MapLength = 256; //количество ячеек в ширину и высоту
	public static int		TileSize = 100; //размер квадрата
	public static float		TileHeight = 400f; //высота поверхности
	public static float		HoleHeight = 0.4f; //высота дыр
	public static float		AnimationSpeedWorldObjects = 0.25f; //скорость воспроизведения объектов
	public static float		GrassWidth = 80f; //ширина травы
	public static float		GrassSpeed = 0.3f; //скорость ветра
	public static float		GrassStrength = 0.1f; //сила ветра
	public static float		GrassAmount = 0.4f; //количество ветров
	public static int		GrassPerPatch = 8; //частота травы
	public static Vector3	ScaleObject = new Vector3(100f, 100f, 100f); //размеры объектов
	public static float		DefaultPlayerDirection = 90f; //поворот игрока по умолчанию
	public static float		HeightmapPixelError = 20; //качество земли
	public static Vector3	CameraDistancePlayer = new Vector3(750f, 750f, -750f); //Удаленность камеры от игрока
	public static float		FixHeightObject = 43f; //фикс позиции обхектов по высоте
	public static float		WaitLogoMap = 5f; //сколько сек показывать логотип карты
	public static int		MinRadiusViewTiles = 13; //минимальный радиус видимых тилей
	public static int		MaxRadiusViewTiles = 50; //максимальный радиус видимых тилей
	public static float		FogLenght = 1100; //размер и сглаженность тумана
	public static bool		ErrorWorldObjectNotFound = false; //выводить ошибку если объект не найден
	public static float		TerrainClickBetween = 0.3f; //интервал между кликами по земле
	public static bool		SmoothPlayerRotation = true; //плавный поворот игрока
	public static Vector3	PlayerCenter = new Vector3(0f, -100f, 0f); //центр игрока
	public static bool		AccurateMoveTarget = true; //точный ли клик мыши по террейн, или округленный
	public static float		TimeWalk = 3.2f; //время звука одного шага
	public static float		RatioMapSound = 0.8f; //соотношения звука карт
	public static int		SleepTimeout = -1; //время перед сном, -1 никогда не спать
	public static string	RemoteLoggerURL = "https://script.google.com/macros/s/AKfycbzj0KQC0TVpb7zlz179cPUEY3IJ-KVszJdHshHjbVuvLcyfflJf/exec"; //Google Logs
	public static bool		RemoteLoggerEnabled = true; //вкл/выкл удаленный логгер
	
	
	

}
