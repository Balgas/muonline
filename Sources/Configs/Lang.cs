
using System.Collections;
using MuGlobal;

public static class Lang {

	public enum Languages:int {
	 	en, ru
	}
	
	public static Languages GetLanguage() {
		return Languages.en;
	}
	
	public static string str(string code, params object[] args) {
		string text = Global.instance.strings[code].ToString();
		if (text==null) text = code;
		return string.Format(text, args);
	}
	
}

