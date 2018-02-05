using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CombatStanceComponent{
	public string name = "Tentacle Lash";
	public CreatureLimb limb;
	public Action<bool>[] action;

	public CombatStanceComponent(CreatureLimb l){
		limb = l;
		// action[0] = limb.combatActions[0];
		// action[1] = limb.combatActions[1];
		// action[2] = limb.combatActions[2];
		// action[3] = limb.combatActions[3];
		// action[4] = limb.combatActions[4];
	}

}