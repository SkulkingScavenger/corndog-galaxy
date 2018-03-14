using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkControl : NetworkManager {
	public Control mainControl;

	// Use this for initialization
	void Awake () {
		mainControl = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
		DontDestroyOnLoad(transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId){
		GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		player.GetComponent<Player>().Init();
	}

	
}
