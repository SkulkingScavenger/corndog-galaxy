using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class NavigationConsoleOverlay : InterfaceOverlay{
	InteractiveInstallation installation = null;
	CreatureControl control = null;
	Creature user = null;

	void Awake(){
		Player localPlayer = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>().GetPlayer();
		control = localPlayer.inputControl;
		user = localPlayer.creature;
	}

	void Update(){
		if(ShouldClose()){
			Destroy(gameObject);
		}

	}

	private bool ShouldClose(){
		return control.shift;
	}

	
}