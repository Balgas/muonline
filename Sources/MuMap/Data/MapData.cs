
using System.Collections;
using MuPacket;

namespace MuMap {
	
	public class MapData {
		
		public delegate void DelegateErrorFile(string file);
		public event DelegateErrorFile EventErrorFile;
		
		public Util.Map.Location map { get; set; }
		protected string file { get; set; }
		
		protected byte[] Load() {
			string path = FilePath();
			if (path==null) { return null; }
			
			byte[] encode = Util.Storage.Load(path);
			
			if (encode==null || encode.Length==0) { 
				if (EventErrorFile!=null)
					EventErrorFile ( path );
				return null;
			}
			
			return encode;
		}
		
		string FilePath() {
			string dir = Util.File.WorldStorageDir(map);
			if (string.IsNullOrEmpty(file) || string.IsNullOrEmpty(dir)) return null;
			string path = Util.File.DIRECTORY_DATA + dir + "/" + file;
			return path;
		}
		
		protected void ParseGround(ref byte[] bytes) {
			if (bytes.Length == MuEncDec.MAP_FILE_SIZE) {
				MuEncDec.MuXorFile(ref bytes, MuEncDec.MAP_FILE_SIZE);
			}
		}
		
		protected void ParseZones(ref byte[] bytes) {
			if (bytes.Length == MuEncDec.ATT_FILE_65KB_SIZE) {
				MuEncDec.MuXorFile(ref bytes, MuEncDec.ATT_FILE_65KB_SIZE);
				MuEncDec.MuXorFile2(ref bytes, MuEncDec.ATT_FILE_65KB_SIZE);
			}
		}
	}
}
