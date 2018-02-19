using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour{
	public GameObject creatureObj;
	public Control mainControl;
	public Creature creature;
	[SyncVar] public string interfaceMode = "combat";
	[SyncVar] public float startX = 4f;
	[SyncVar] public float startY = -0.5f;
	[SyncVar] public int score = 0;
	public CreatureControl inputControl;

	// Use this for initialization
	void Awake () {
		inputControl = GetComponent<CreatureControl>();
		mainControl = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
		mainControl.players.Add(this);
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer){
			return;
		}

		inputControl.moveCommand = Input.GetAxis("MouseRight") != 0;
		inputControl.attackCommand = Input.GetAxis("MouseLeft") != 0;

		inputControl.commandX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
		inputControl.commandY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

		inputControl.actionModifier[0] = Input.GetKey ("w");
		inputControl.actionModifier[1] = Input.GetKey ("a");
		inputControl.actionModifier[2] = Input.GetKey ("s");
		inputControl.actionModifier[3] = Input.GetKey ("d");

		inputControl.stanceModifier[0] = Input.GetKey ("1");
		inputControl.stanceModifier[1] = Input.GetKey ("2");
		inputControl.stanceModifier[2] = Input.GetKey ("3");
		inputControl.stanceModifier[3] = Input.GetKey ("4");

		inputControl.shift = Input.GetAxis("Shift") != 0;
		inputControl.ctrl = false;
		inputControl.jump = Input.GetButtonDown("Jump");
	}


	public void SpawnCreature(){
		creatureObj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Characters/SimpleCreature"));
		creature = creatureObj.GetComponent<Creature>();
		creature.control = inputControl;
		creatureObj.transform.position = new Vector3(startX,startY,0); 
		creature.Init();
		NetworkServer.Spawn(creatureObj);
	}

}