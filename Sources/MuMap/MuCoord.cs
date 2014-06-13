
using UnityEngine;

public class MuCoord : Object {
	
	public int x;
	public int y;
	
	public Vector2 ToVector() {
		return new Vector2((float)x, (float)y);
	}
	
	public override bool Equals(System.Object obj) {
		if ((object)obj == null) {
			return false;
		}
		MuCoord c = obj as MuCoord;
		return (x == c.x) && (y == c.y);
    }
	
	public override int GetHashCode() {
		return x ^ y;
	}
	
	public override string ToString () {
		return string.Format ("[MuCoord] x: {0}, y: {1}", x, y);
	}
}