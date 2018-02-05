using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Area : MonoBehaviour
{
	public string areaName = "starcrawler";
	public string tileset = "starcrawler";
	public string exteriorTileset = "starcrawler";
	public bool isStructure = true;
	public bool isVehicle = true;
	public List<Corridor> corridors; 

	void Awake ()
	{

	}

	void Update ()
	{

	}

	public int getAreaLength(){
		int output = 0;
		if(corridors.Count > 0){
			int min = corridors[0].x;
			int max = corridors[0].x + corridors[0].segments.Count;
			for(int i=0; i < corridors.Count;i++){
				Corridor corridor = corridors[i];
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

	public Corridor mergeCorridors(Corridor a,Corridor b){
		return a;
	}
}