using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Control : MonoBehaviour{
	public List<Player> players = new List<Player>();
	public int currentPlayerId = 0;
	public List<Area> areas = new List<Area>();
	public GameObject mainCamera;
	public GameObject mainCanvas;
	public GameObject currentMenu;

	void Awake ()
	{
		//createArea();
		Application.targetFrameRate = 60;
	}

	void Start(){
		players.Add(new Player());
		

		mainCamera = Instantiate(Resources.Load<GameObject>("Prefabs/mainCamera"));
		mainCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Canvas"));
		currentMenu = Instantiate(Resources.Load<GameObject>("Prefabs/UI/MainMenu"));
		currentMenu.transform.Find("Panel").transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate { StartGame(); });
		currentMenu.transform.SetParent(mainCanvas.transform,false);
		//StartGame();
	}

	void Update (){

	}

	public void StartGame(){
		getPlayer(0).SpawnSkirriashi();
		mainCamera.GetComponent<CameraObject>().root = getPlayer().creature.transform;
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