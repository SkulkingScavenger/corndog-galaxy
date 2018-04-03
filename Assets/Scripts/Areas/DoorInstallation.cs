using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class DoorInstallation : NetworkBehaviour{
	public string installationName = "Internal Door";
	[SyncVar] public bool isDestroyed = false;
	public bool isDestructible = false;
	public bool isPickupable = false;
	public GameObject attachedWall = null;
	public int hitpoints = 40;
	public DoorInstallation otherSide = null;
	public int directionId;

	public void Awake(){
		
	}

	public void Update(){

	}

	public void Interact(Creature interactor){
		interactor.isInteracting = true;
		//user.door = this;
		if(interactor.control.isPlayerControlled){
			if(interactor.control.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer){
				//go through
			}
		}
	}


	public IEnumerator Traverse(Creature c){
		yield return new WaitForSeconds(1f);
		if(c.contactedDoor != null && gameObject.GetInstanceID() == c.contactedDoor.gameObject.GetInstanceID()){
			c.gameObject.transform.position = c.gameObject.transform.position + GetDestination();
		}
	}

	public Vector3 GetDestination(){
		float x = 0;
		float y = 0;
		float z = 0;
		switch(directionId){
			case 0: x = 4.5f; break;
			case 1: y = 5.5f; break;
			case 2: x = -4.5f; break;
			case 3: y = -5.5f; break;
		}
		return new Vector3(x,y,z);

	}
}