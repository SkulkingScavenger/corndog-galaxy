using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour{
	public GameObject creatureObj;
	public Control mainControl;
	public GameObject mainCanvas;
	public GameObject headsUpDisplay;
	public GameObject mainCamera;
	public Creature creature = null;
	[SyncVar] public string interfaceMode = "combat";
	[SyncVar] public float startX = 4f;
	[SyncVar] public float startY = -0.5f;
	[SyncVar] public int score = 0;
	public CreatureControl inputControl;

	// Use this for initialization
	void Awake() {
		inputControl = GetComponent<CreatureControl>();
		inputControl.isPlayerControlled = true;
		mainControl = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
		mainControl.players.Add(this);
		DontDestroyOnLoad(transform.gameObject);
	}

	public override void OnStartLocalPlayer(){
		mainCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Canvas"));
		headsUpDisplay = Instantiate(Resources.Load<GameObject>("Prefabs/UI/HudCombat"));
		headsUpDisplay.transform.SetParent(mainCanvas.transform,false);

		mainCamera = Instantiate(Resources.Load<GameObject>("Prefabs/Control/mainCamera"));
		mainCamera.GetComponent<CameraControl>().root = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer){
			return;
		}
		if(creature!= null){
			transform.position = creature.transform.position;
		}

		Rect r = new Rect(mainCamera.transform.position.x - 512/128f,mainCamera.transform.position.y - 256/128f,1024,128);
		inputControl.isHudCommand = !r.Contains(mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition));

		inputControl.interfaceMode = interfaceMode;

		inputControl.moveCommand = Input.GetAxis("MouseRight") != 0;
		inputControl.attackCommand = Input.GetAxis("MouseLeft") != 0;

		inputControl.commandX = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition).x;
		inputControl.commandY = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition).y;

		inputControl.actionModifier[0] = Input.GetKey ("w");
		inputControl.actionModifier[1] = Input.GetKey ("a");
		inputControl.actionModifier[2] = Input.GetKey ("s");
		inputControl.actionModifier[3] = Input.GetKey ("d");

		inputControl.stanceModifier[0] = Input.GetKey ("1");
		inputControl.stanceModifier[1] = Input.GetKey ("2");
		inputControl.stanceModifier[2] = Input.GetKey ("3");
		inputControl.stanceModifier[3] = Input.GetKey ("4");

		inputControl.shift = Input.GetAxis("Shift") != 0;
		inputControl.ctrl = Input.GetAxis("Ctrl") != 0;
		inputControl.jump = Input.GetButtonDown("Jump");
		inputControl.Sync();
	}

	public void Init(){
		creatureObj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Characters/Creature"), Vector3.zero, Quaternion.identity);
		creature = creatureObj.GetComponent<Creature>();
		creature.controlID = GetComponent<NetworkIdentity>().netId.Value;
		creatureObj.transform.position = new Vector3(startX,startY,0); 
		NetworkServer.Spawn(creatureObj);
	}

	public void SetInterfaceMode(string mode){
		Destroy(headsUpDisplay.gameObject);
		switch(mode){
		case "combat":
			headsUpDisplay = Instantiate(Resources.Load<GameObject>("Prefabs/UI/HudCombat"));
			break;
		case "industry":

			break;
		case "exploration":
			headsUpDisplay = Instantiate(Resources.Load<GameObject>("Prefabs/UI/HudExploration"));
			break;
		}
		interfaceMode = mode;
		headsUpDisplay.transform.SetParent(mainCanvas.transform,false);
	}

}