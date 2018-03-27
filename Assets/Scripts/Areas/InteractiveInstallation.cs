using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveInstallation : NetworkBehaviour{
	public string installationName = "Navigation Console";
	public string interfaceOverlayName = "NavigationConsoleOverlay";
	public GameObject interfaceOverlay = null;
	[SyncVar] public bool inUse = false;
	[SyncVar] public bool isDestroyed = false;
	public Creature user = null;
	public bool isDestructible = false;
	public bool isPickupable = false;
	public GameObject attachedWall = null;
	public int hitpoints = 40;

	public void Awake(){
		attachedWall = transform.parent.Find("wallN").gameObject;
		
	}

	public void Update(){
		//TODO
		//check if installation is destroyed
		//check if user is still alive;
		//check if should close
		if(inUse){
			if(interfaceOverlay == null){
				CloseInteration();
			}
		}
	}

	public void Interact(Creature interactor){
		if(!inUse){
			user = interactor;
			inUse = true;
			user.isInteracting = true;
			user.interactionInstallation = this;
			if(user.control.isPlayerControlled){
				if(user.control.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer){
					interfaceOverlay = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/InterfaceOverlays/"+interfaceOverlayName), Vector3.zero, Quaternion.identity);
				}
			}
		}
	}

	public void CloseInteration(){
		user.isInteracting = false;
		user.interactionInstallation = null;
		user = null;
		inUse = false;
		if(interfaceOverlay != null){
			Destroy(interfaceOverlay.transform.gameObject);
		}
	}
}