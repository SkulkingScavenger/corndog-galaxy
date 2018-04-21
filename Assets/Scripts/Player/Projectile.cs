using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour {
	[SyncVar] public uint creatorId;

	[SyncVar] public int z = 0;
	[SyncVar] public int depth = 0;

	[SyncVar] public float direction = 0;
	[SyncVar] public float speed = 0;
	[SyncVar] public float range = 0;
	[SyncVar] public int damage = 0;
	public bool canHitMultiple = false;
	public bool canBounce = false;
	public bool canDamageWalls = false;


	void Awake () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		// if(other.gameObject.GetComponent("Creature") != null){
		// 	Creature c = other.gameObject.GetComponent<Creature>();
		// 	if(other.gameObject.GetComponent<NetworkIdentity>().netId.Value != creatorId){
		// 		damageCreature(c);
		// 		if(!canHitMultiple){
		// 			Destroy(gameObject);
		// 		}
		// 	}
		// }else if(other.gameObject.GetComponent("wall") != null){
		// 	Destroy(gameObject);
		// }
	}


	void damageCreature(Creature c){
		
	}


	void OnDestroy(){

	}

	void Update () {
		
	}
}
