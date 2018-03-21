using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class NavigationConsoleOverlay : InterfaceOverlay{
	InteractiveInstallation installation = null;
	CreatureControl control = null;
	Creature user = null;
	Transform cam;

	void Awake(){
		Player localPlayer = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>().GetPlayer();
		cam = localPlayer.mainCamera.transform;
		control = localPlayer.inputControl;
		Debug.Log(control);
		user = localPlayer.creature;
		user.control = null;
		Debug.Log(user.control);
	}

	void Update(){
		if(ShouldClose()){
			Destroy(gameObject);
			return;
		}
		transform.position = new Vector3(cam.position.x, cam.position.y+1.5f, transform.position.z);


	}

	private bool ShouldClose(){
		return control.ctrl;
	}

	void Destroy(){
		if(user != null){
			user.control = control;
		}
	}

	
}