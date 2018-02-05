using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatureOrgan{
	public Creature root;
	public Vector3 offset;
	public string name;
	public CreatureOrgan rootOrgan;
	public string type;
	public int hitpoints;
	public GameObject obj;
	
}

public class CreatureLimb : CreatureOrgan{
	public string limbType;
	public bool isParalyzed = false;
	public bool isReady = true;
	//public Action<bool>[] combatActions;

	public CreatureLimb(){
		type = "limb";
	}

	
}