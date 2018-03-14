using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Control : NetworkBehaviour{
	public List<Player> players = new List<Player>();
	public int currentPlayerId = 0;
	public List<Area> areas = new List<Area>();
	public GameObject mainCanvas;
	public NetworkManager networkControl;
	public GameObject currentMenu;
	public bool isDedicatedServer = false;

	public static Control Instance { get; private set; }

	void Awake (){
		//ensure uniqueness
		if(Instance != null && Instance != this){
			Destroy(gameObject);
		}
		Instance = this;
		DontDestroyOnLoad(transform.gameObject);

		//createArea();
		Application.targetFrameRate = 60;
	}

	void Start(){

	}

	void Update (){

	}

	public void StartGame(){
		GameObject networkObj = GameObject.FindGameObjectWithTag("NetworkControl");
		networkControl = networkObj.GetComponent<NetworkControl>();
		networkControl.StartHost();

	}

	public void EnterGame(){
		GameObject networkObj = GameObject.FindGameObjectWithTag("NetworkControl");
		networkControl = networkObj.GetComponent<NetworkControl>();
		networkControl.StartClient();
	}

	public void AddPlayer(Player p){
		players.Add(p);
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