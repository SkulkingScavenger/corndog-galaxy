using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Starship {
	public int index;

	public Sector currentSector;
	public Coordinates coordinates;
	public int x {get {return coordinates.x;} set{coordinates.x = value;}}
	public int y {get {return coordinates.y;} set{coordinates.y = value;}}

	public string name;
	public string classification;
	public int hitpoints;
	public bool isTraveling = false;
	public bool isWarping = false;
	public Coordinates destinationCoordinates = new Coordinates();
	public Coordinates warpDestinationCoordinates = new Coordinates();

	public Area area;
	public Tileset tileset = new Tileset("starcrawler");


	public Starship(int x=0, int y=0){
		coordinates = new Coordinates();
		index = Galaxy.Instance.starships.Count;
		Galaxy.Instance.starships.Add(this);
	}
}