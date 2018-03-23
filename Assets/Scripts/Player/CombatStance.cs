using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatStance{
	public string name = "Fish Stance";
	public CombatMoveSet[] combatMoveSets = new CombatMoveSet[10]; 
}

public class CombatMoveSet{
	public List<CombatAction> moves = new List<CombatAction>();
}

public class CombatAction{
	public CreatureLimb limb;
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
}