using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player{
	public GameObject creatureObj;
	public Creature creature;
	public string interfaceMode = "combat";
	private float startX = 4f;
	private float startY = -0.5f;

	public void init(){
		creatureObj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Characters/Skirriashi"));
		creature = creatureObj.GetComponent<Creature>();
		creature.player = this;
		creatureObj.transform.position = new Vector3(startX,startY,0); 
		creature.Init();
	}

}