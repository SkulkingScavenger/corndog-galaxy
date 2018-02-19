using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Control : NetworkBehaviour{
	public List<Player> players = new List<Player>();
	public int currentPlayerId = 0;
	public List<Area> areas = new List<Area>();
	public GameObject mainCamera;
	public GameObject mainCanvas;
	public NetworkManager networkControl;
	public GameObject currentMenu;

	void Awake ()
	{
		//createArea();
		Application.targetFrameRate = 60;
		networkControl = GameObject.FindGameObjectWithTag("NetworkControl").GetComponent<NetworkManager>();
		mainCamera = Instantiate(Resources.Load<GameObject>("Prefabs/Control/mainCamera"));
		mainCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Canvas"));
	}

	void Start(){
		
		currentMenu = Instantiate(Resources.Load<GameObject>("Prefabs/UI/MainMenu"));
		currentMenu.transform.Find("Panel").transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate { StartGame(); });
		currentMenu.transform.SetParent(mainCanvas.transform,false);

	}

	void Update (){

	}

	public void StartGame(){
		Player p;
		if(isServer){
			for(int i=0;i<players.Count;i++){
				p = getPlayer(i);
				p.creatureObj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Characters/Creature"));
				p.creature = p.creatureObj.GetComponent<Creature>();
				p.creature.control = p.inputControl;
				p.creatureObj.transform.position = new Vector3(p.startX,p.startY,0); 
				p.creature.Init();
				NetworkServer.Spawn(p.creatureObj);
			}
			
			
		}
		Destroy(currentMenu);
		currentMenu = Instantiate(Resources.Load<GameObject>("Prefabs/UI/HudCombat"));
		currentMenu.transform.SetParent(mainCanvas.transform,false);
		
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