using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatStanceComponent{
	public string name = "Tentacle Lash";
	public CreatureLimb limb;
	public CombatAction[] actions = new CombatAction[10];

	public CombatStanceComponent(CreatureLimb l){
		limb = l;
		for(int i=0;i<10;i++){
			actions[i] = null;
		}
	}
}

public class CombatAction{
	public string name = "";
	public float range = 0;
	public int damage = 0;
	public float cooldownDuration = 0;
	public float windupDuration = 0;
	public float attackDuration = 0;
	public float backswingDuration = 0;
	public string attackAnimation = "";
	public string backswingAnimation = "";
	public string windupAnimation = "";
	public string idleAnimation = "";
}