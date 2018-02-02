using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Control : MonoBehaviour
{

	public List<Area> areas = new List<Area>();

	void Awake ()
	{
		//createArea();
	}

	void Update ()
	{

	}


	private void createArea(){
		Area area = new Area();
		Corridor corridor = new Corridor();
		area.corridors.Add(corridor);
		for(int i=0;i<4;i++){

		}
	}
}