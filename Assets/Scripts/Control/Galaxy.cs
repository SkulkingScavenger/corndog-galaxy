using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Galaxy {
	public int width;
	public int height;
	public Sector[,] sectors;
	public List<Starship> starships = new List<Starship>();

	public Galaxy(int w=3, int h=3){
		width = w;
		height = h;
		sectors = new Sector[width,height];
		for(int i=0;i<width;i++){
			for(int j=0;j<height;j++){
				sectors[i,j] = new Sector(i,j);
			}
		}

		Planet planet = new Planet();
		planet.name = "Zakaas";
		planet.spriteIndex = 0;
		planet.starSystemSpriteIndex = 0;
		planet.shortDescription = "Zakaas II is the Homeworld of the Zakasi. Because the atmosphere is thin and the surface primarily liquid water, most sentient residents inhabit the hollow interiors of the sunken continents";
		planet.sector = sectors[0,0];
		planet.x = 115;
		planet.y = 116;
		sectors[0,0].planets.Add(planet);

		planet = new Planet();
		planet.name = "Karkirraas";
		planet.spriteIndex = 1;
		planet.starSystemSpriteIndex = 1;
		planet.shortDescription = "Karkirraas V is a dark and toxic planet with oceans of sulphuric acid and an atmosphere dense enough to block out the light from its star. Notable for its uranium mines.";
		planet.sector = sectors[0,0];
		planet.x = 245;
		planet.y = 83;
		sectors[0,0].planets.Add(planet);

		planet = new Planet();
		planet.name = "Arrikshaar";
		planet.spriteIndex = 2;
		planet.starSystemSpriteIndex = 2;
		planet.shortDescription = "Arrikshaar III is a dusty and desolate world. It is notable for its large quantities of sodium-chloride and other chloride salts covering the crust and dissolved within its oceans.";
		planet.sector = sectors[0,0];
		planet.x = 256;
		planet.y = 167;
		sectors[0,0].planets.Add(planet);

		planet = new Planet();
		planet.name = "Varrazaar";
		planet.spriteIndex = 3;
		planet.starSystemSpriteIndex = 3;
		planet.shortDescription = "Varrazaar III is world of fog and sea. Its relatively mild climate and oceans of water make it a hospitable world for many carbon-based lifeforms.";
		planet.sector = sectors[0,0];
		planet.x = 118;
		planet.y = 250;
		sectors[0,0].planets.Add(planet);
	}
}

public class Sector {
	public int spriteIndex;
	public List<Planet> planets = new List<Planet>();
	public int x;
	public int y;

	public Sector(int coordx, int coordy){
		x = coordx;
		y = coordy;
	}

	public string GetTextCoordinates(){
		return "Galaxy: " + x.ToString() + ", " + y.ToString();
	}
}

public class Planet {
	public int spriteIndex;
	public int starSystemSpriteIndex;
	public string name;
	public string shortDescription;
	public Sector sector;
	public int x;
	public int y;

	public string GetTextCoordinates(){
		return "Coordinates: " + x.ToString() + ", " + y.ToString();
	}
}