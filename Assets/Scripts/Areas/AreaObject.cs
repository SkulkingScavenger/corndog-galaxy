using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class AreaObject : NetworkBehaviour {
	public int directionId;
	public string type;
	public Coordinates coordinates = new Coordinates();
	public int x {get {return coordinates.x;} set{coordinates.x = value;}}
	public int y {get {return coordinates.y;} set{coordinates.y = value;}}

	void Update(){

	}

	// public AreaSegment GetAbstract(){
	// 	Area area = AreaManager.Instance.currentArea;
	// }
}
