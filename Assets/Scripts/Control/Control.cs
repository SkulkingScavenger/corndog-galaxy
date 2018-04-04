using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Control : MonoBehaviour{
	public List<Player> players = new List<Player>();
	public int currentPlayerId = 0;
	public List<Area> areas = new List<Area>();
	public GameObject mainCanvas;
	public NetworkManager networkControl;
	public GameObject currentMenu;
	public bool isDedicatedServer = false;
	public GameObject currentArea = null;

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

	public void StartGame(){
		GameObject galaxy = Instantiate(Resources.Load<GameObject>("Prefabs/Control/Galaxy"), Vector3.zero, Quaternion.identity);
		NetworkServer.Spawn(galaxy);
		Galaxy.Instance.Init();

		
		GameObject areaManager = Instantiate(Resources.Load<GameObject>("Prefabs/Control/AreaManager"), Vector3.zero, Quaternion.identity);
		NetworkServer.Spawn(areaManager);
		AreaManager.Instance.Init();
		
	}

	
}