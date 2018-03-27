using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Starship {
	public Sector currentSector;
	public int coordinateX;
	public int coordinateY;
	public string name;
	public string classification;
	public int hitpoints;
	public bool isTraveling = false;
	public bool isWarping = false;
	public int destinationCoordinateX;
	public int destinationCoordinateY;
	public int warpDestinationCoordinateX;
	public int warpDestinationCoordinateY;

	public Starship(){
		
	}
}