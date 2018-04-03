using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Area {
	public string name = "starcrawler";
	public Tileset tileset;
	public string exteriorTileset = "starcrawler";
	
	public List<AreaCorridor> corridors = new List<AreaCorridor>(); 
	public Starship starship = null;
	public bool isStructure = false;
	public bool isStarship {get {return starship != null;} private set{}}


	public Area(Starship ss){
		starship = ss;
		name = ss.name;
		tileset = ss.tileset;
	}

	public int GetAreaLength(){
		int output = 0;
		if(corridors.Count > 0){
			int min = corridors[0].x;
			int max = corridors[0].x + corridors[0].segments.Count;
			for(int i=0; i < corridors.Count;i++){
				AreaCorridor corridor = corridors[i];
				if(corridor.x < min){
					min = corridor.x;
				}
				if(corridor.x + corridor.segments.Count > max){
					max = corridor.x + corridor.segments.Count;
				}
			}
			output = max - min;
		}
		return output;
	}

	public AreaCorridor MergeCorridors(AreaCorridor a, AreaCorridor b){
		return a;
	}

	public AreaCorridor spawnCorridor(int x, int y, int len){
		AreaCorridor cor = new AreaCorridor();
		cor.coordinates = new Coordinates(x,y);
		cor.area = this;
		for(int i=0;i<len;i++){
			AreaSegment segment = new AreaSegment(cor);
			if(i != 0){
				segment.walls[2] = null;
			}
			if(i != len-1){
				segment.walls[0] = null;
			}
			cor.segments.Add(segment);
		}
		corridors.Add(cor);
		return cor;
	}
}

public class AreaCorridor {
	public Area area;
	public List<AreaSegment> segments = new List<AreaSegment>(); 

	public Coordinates coordinates;
	public int x {get {return coordinates.x;} set{coordinates.x = value;}}
	public int y {get {return coordinates.y;} set{coordinates.y = value;}}
}

public class AreaSegment{
	public AreaCorridor corridor;
	public int index;
	public Area area {get {return corridor.area;} private set{}}
	public Coordinates coordinates {get {return new Coordinates(corridor.x+index,corridor.y);} private set{}}
	public int x {get {return coordinates.x;} private set{}}
	public int y {get {return coordinates.y;} private set{}}
	public AreaSegmentWall[] walls = new AreaSegmentWall[4];
	public AreaSegmentInstallation[] installations = new AreaSegmentInstallation[4];

	public AreaSegment(AreaCorridor cor){
		corridor = cor;
		index = cor.segments.Count;
		for(int i=0;i<4;i++){
			walls[i] = new AreaSegmentWall(i,this);
			installations[i] = null;
		}
	}

	public AreaSegment getNeighbor(int directionId){
		AreaSegment neighbor = null;
		Coordinates neighborCoords = new Coordinates(corridor.x+index,corridor.y);
		neighborCoords = coordinates.getAdjacent(directionId);
		foreach(AreaCorridor cor in area.corridors){
			if(cor.y == neighborCoords.y){
				foreach(AreaSegment seg in cor.segments){
					if (seg.x == neighborCoords.x){
						neighbor = seg;
					}
				}
			}
		}
		return neighbor;
	}
}

public class AreaSegmentWall{
	public int directionId; //0E : 1N : 2W : 3S 
	public int spriteId;
	public AreaSegment segment;

	public bool isBreached {get{return false;} private set{}}
	public int plateMaxHitpoints;
	public int structureMaxHitpoints;
	public int plateHitpoints;
	public int structureHitpoints;
	public string platingMaterial = "steel";
	public float platingThickness = 1f;
	public string structureMaterial = "steel";
	public float structureThickness = 1f;
	public bool isPlated = true;
	public float mass {get{return 150f;} private set{}}

	public AreaSegmentWall(int dir,AreaSegment seg){
		directionId = dir;
		segment = seg;
	}
}

public class AreaSegmentInstallation{
	public string type = "";
	public string name = "";
	public string overlayName = "";
	public int directionId;
	public AreaSegment segment;
	public int hitpoints;

	public AreaSegmentInstallation(int dir,AreaSegment seg){
		directionId = dir;
		segment = seg;
	}
}