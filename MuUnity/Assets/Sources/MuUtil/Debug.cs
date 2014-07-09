namespace Util {
	
	using UnityEngine;
	
	public class Debug {
		
		
		public static void Log ( string text, string stackTrace, LogType type = LogType.Log ) {
			if (!Config.RemoteLoggerEnabled) return;
			
			string t_url = Config.RemoteLoggerURL + "?t=" + type.ToString() + "&p=" + Util.Storage.UrlEncode ( text ) + "&s=" + Util.Storage.UrlEncode ( stackTrace );
			Util.Storage.WWWSend ( t_url );
	    }
		
		public static void LogCallback ( string text, string stackTrace, LogType type ) {
			if (!Config.RemoteLoggerEnabled) return;
			
			Log ( text, stackTrace, type );
			
	    }
		
	}
}