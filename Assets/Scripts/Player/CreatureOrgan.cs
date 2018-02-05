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
	public bool isWindingUp = false;
	public bool isAttacking = false;
	public bool isBackswinging = false;
	public List<CombatAction> combatActions = new List<CombatAction>();

	public CreatureLimb(){
		type = "limb";
	}

	
}


public static class LimbAttacks {
	public static void attack(int id){
		switch(id){
			case 0: 
				basicAttack();
			break;
		}
	}

	private static void basicAttack(){

	}
}