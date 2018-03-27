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
	public Galaxy galaxy;

	public static Control Instance { get; private set; }

	void Awake (){
		//ensure uniqueness
		if(Instance != null && Instance != this){
			Destroy(gameObject);
		}
		Instance = this;
		DontDestroyOnLoad(transform.gameObject);

		Application.targetFrameRate = 60;
	}

	void Start(){

	}

	void Update (){

	}

	public void StartServer(){
		GameObject networkObj = GameObject.FindGameObjectWithTag("NetworkControl");
		networkControl = networkObj.GetComponent<NetworkControl>();
		networkControl.StartHost();
	}

	public void JoinServer(){
		GameObject networkObj = GameObject.FindGameObjectWithTag("NetworkControl");
		networkControl = networkObj.GetComponent<NetworkControl>();
		networkControl.StartClient();
	}

	public void StartGame(){
		galaxy = new Galaxy();
		//TODO sync multiplayer
		Starship starship = new Starship();
		starship.currentSector = galaxy.sectors[0,0];
		starship.coordinateX = 150;
		starship.coordinateY = 100;
		starship.name = "Kirrikash Virr";
		starship.classification = "Zakasi Starcrawler";
		starship.hitpoints = 4500;
		GameObject area = Instantiate(Resources.Load<GameObject>("Prefabs/Environment/Starcrawler"), Vector3.zero, Quaternion.identity);
		area.GetComponent<Area>().starship = starship;

	}

	public Player GetPlayer(int id = -1){
		Player p;
		int i;
		if(id < 0){
			for(i=0;i<players.Count;i++){
				p = players[i];
				if(p.GetComponent<NetworkIdentity>().isLocalPlayer){
					return p;
				}
			}
		}else{
			for(i=0;i<players.Count;i++){
				p = players[i];
				if(p.GetComponent<NetworkIdentity>().netId.Value == (uint) (int) id){
					return p;
				}
			}
		}
		return null;
	}

	private void createArea(){
		Area area = new Area();
		Corridor corridor = new Corridor();
		area.corridors.Add(corridor);
		for(int i=0;i<4;i++){

		}
	}
}