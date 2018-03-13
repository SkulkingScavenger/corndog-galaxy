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
	public GameObject mainCamera;
	public GameObject mainCanvas;
	public NetworkManager networkControl;
	public GameObject currentMenu;

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
		// networkControl = GameObject.FindGameObjectWithTag("NetworkControl").GetComponent<NetworkManager>();
		// mainCamera = Instantiate(Resources.Load<GameObject>("Prefabs/Control/mainCamera"));
		// mainCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Canvas"));

	}

	void Start(){

	}

	void Update (){

	}

	public void StartGame(){
		StartCoroutine(StartGameAsync("Level"));
	}

	public void EnterGame(){
		StartCoroutine(EnterGameAsync("Level"));
	}

	private IEnumerator StartGameAsync(string sceneName){
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
		while (!asyncLoad.isDone){
			yield return null;
		}
		GameObject networkObj = Instantiate(Resources.Load<GameObject>("Prefabs/Control/networkManager"));
		networkControl = networkObj.GetComponent<NetworkManager>();
		networkControl.StartHost();
		mainCamera = Instantiate(Resources.Load<GameObject>("Prefabs/Control/mainCamera"));
		mainCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Canvas"));

		// }
		// Destroy(currentMenu);
		// currentMenu = Instantiate(Resources.Load<GameObject>("Prefabs/UI/HudCombat"));
		// currentMenu.transform.SetParent(mainCanvas.transform,false);
	}

	private IEnumerator EnterGameAsync(string sceneName){
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
		while (!asyncLoad.isDone){
			yield return null;
		}
		GameObject networkObj = Instantiate(Resources.Load<GameObject>("Prefabs/Control/networkManager"));
		networkControl = networkObj.GetComponent<NetworkManager>();
		networkControl.StartClient();
		mainCamera = Instantiate(Resources.Load<GameObject>("Prefabs/Control/mainCamera"));
		mainCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Canvas"));
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