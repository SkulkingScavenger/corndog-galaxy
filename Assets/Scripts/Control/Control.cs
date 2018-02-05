using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Control : MonoBehaviour{
	public List<Player> players = new List<Player>();
	public int currentPlayerId = 0;
	public List<Area> areas = new List<Area>();
	public GameObject mainCamera;

	void Awake ()
	{
		//createArea();
		players.Add(new Player());
		getPlayer(0).init();
		mainCamera = Instantiate(Resources.Load<GameObject>("Prefabs/mainCamera"));
		Application.targetFrameRate = 60;
	}

	void Update (){

	}

	public Player getPlayer(int id = -1){
		if(id==-1){id = currentPlayerId;}
		return players[id];
	}

	private void createArea(){
		Area area = new Area();
		Corridor corridor = new Corridor();
		area.corridors.Add(corridor);
		for(int i=0;i<4;i++){

		}
	}
}