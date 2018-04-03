using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starmap {
	public int width;
	public int height;
	public Sector[,] sectors;
	
	public Starmap(int w=3, int h=3){
		width = w;
		height = h;
		sectors = new Sector[width,height];
		for(int i=0;i<width;i++){
			for(int j=0;j<height;j++){
				sectors[i,j] = new Sector(i,j);
			}
		}
	}
}

public class Sector {
	public string name = "";
	public int spriteIndex;
	public int skySpriteIndex = 0;
	public List<Planet> planets = new List<Planet>();
	public Coordinates coordinates = new Coordinates();
	public int x {get {return coordinates.x;} set{coordinates.x = value;}}
	public int y {get {return coordinates.y;} set{coordinates.y = value;}}

	public Sector(int coordx, int coordy){
		x = coordx;
		y = coordy;
		name = "Sector " + coordinates.ToString();
	}

	public string GetTextCoordinates(){
		return "Galaxy: " + coordinates.ToString();
	}
}

public class Planet {
	public int spriteIndex;
	public int starSystemSpriteIndex;
	public string name;
	public string shortDescription;
	public Sector sector;
	public Coordinates coordinates = new Coordinates();
	public int x {get {return coordinates.x;} set{coordinates.x = value;}}
	public int y {get {return coordinates.y;} set{coordinates.y = value;}}

	public string GetTextCoordinates(){
		return "Coordinates: " + coordinates.ToString();
	}
}

public class Coordinates {
	public int x;
	public int y;

	public Coordinates(int a=0,int b=0){
		x=a;
		y=b;
	}

	public override string ToString(){
		return x.ToString() + ", " + y.ToString();
	}

	public Coordinates getAdjacent(int directionId){
		Coordinates adj = new Coordinates(x,y);
		switch(directionId){
			case 0: adj.x++; break;
			case 1: adj.y--; break;
			case 2: adj.x--; break;
			case 3: adj.y++; break;
		}
		return adj;
	}
}