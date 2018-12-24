using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Creature : NetworkBehaviour{
	
	//Control
	public int playerId = -1;
	public CreatureControl control = null;
	[SyncVar] public uint controlID = 0;

	//Display
	private Animator anim;		// Reference to the player's animator component.
	public Transform display;
	public GameObject shadow;
	public int shadowIndex = 1;
	public GameObject healthBar;
	public int speciesId;

	//Biology
	public List<CreatureOrgan> organs = new List<CreatureOrgan>();
	public List<CreatureBodySegment> segments = new List<CreatureBodySegment>();
	
	//Movement
	public float jumpForce = 0.03f;			// Amount of force added when the player jumps.
	public float gravityForce = -0.0008f;	// Amount of force added when the player is in the air.
	private bool grounded = true;			// Whether or not the player is grounded.
	[SyncVar] public float z = 0f;
	public float zspeed = 0f;
	public float accelerationX = 100f;		// rate of change per second in x velocity while moving
	public float accelerationY = 50f;		// rate of change per second in Y velocity while moving
	public float speedX = 0f;				// The current velocity in the x axis.
	public float speedY = 0f;				// The current velocity in the y axis.
	public float maxSpeedX = 6f;			// The fastest the player can travel in the x axis.
	public float maxSpeedY = 3f;			// The fastest the player can travel in the x axis.
	private float frictionForceX = 20f;		// rate of change per second in x velocity due to friction
	private float frictionForceY = 10f;		// rate of change per second in y velocity due to friction

	//Combat	
	[SyncVar] public int stanceId = 0;
	public List<CombatStance> stances = new List<CombatStance>();
	[SyncVar] public float health = 100f;					// The player's health.
	public float repeatDamagePeriod = 2f;		// How frequently the player can be damaged.

	//Interactions
	public bool isInteracting = false;
	public InteractiveInstallation interactionInstallation = null;
	public GameObject contactedInstallation = null;
	public GameObject contactedDoor = null;

	void Awake(){
		display = transform.Find("Display");
		if(isLocalPlayer){
			//mainCamera.GetComponent<CameraObject>().root = getPlayer().creatureObj.transform;
		}
	}

	public void Init(int speciesPrototypeId){
		speciesId = speciesPrototypeId;
		SetLimbs(speciesPrototypeId);
		SetShadow();
		SetHealthBar();
		SetStance();
	}


	void Update(){
		if(control == null){
			if(isInteracting){return;}
			control = ClientScene.FindLocalObject(new NetworkInstanceId(controlID)).GetComponent<CreatureControl>();
			return;
		}
		if(control.isHudCommand){return;}
		MouseMovement();
		Jump();

		switch(control.interfaceMode){
		case "combat":
			CombatMain();
			break;
		case "exploration":
			ExplorationMain();
			break;
		}

		display.transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y + 0.55f + z,0);
	}

	void Jump(){
		if(!grounded){
			zspeed += gravityForce;
			z += zspeed;
		}
		if(z <= 0){
			zspeed = 0;
			z = 0;
			grounded = true;
		}

		if(control.jump && grounded){
			zspeed = jumpForce;
			grounded = false;
		}

	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.GetComponent("InteractiveInstallation") != null){
			contactedInstallation = other.gameObject;
		}
		if(other.gameObject.GetComponent("DoorInstallation") != null){
			contactedDoor = other.gameObject;
			StartCoroutine(contactedDoor.GetComponent<DoorInstallation>().Traverse(this));
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.gameObject.GetComponent("InteractiveInstallation") != null && contactedInstallation != null){
			if(other.gameObject.GetInstanceID() == contactedInstallation.gameObject.GetInstanceID()){
				contactedInstallation = null;
			}
		}
		if(other.gameObject.GetComponent("DoorInstallation") != null && contactedDoor != null){
			if(other.gameObject.GetInstanceID() == contactedDoor.gameObject.GetInstanceID()){
				contactedDoor = null;
			}
		}
	}

	void SetLimbs(int speciesId){
		CreatureBodySegment segment;
		CreatureLimb limb;
		CreatureAppendage appendage;
		Species species = SpeciesManager.Instance.GetSpeciesById(speciesId);
		foreach(CreatureBodySegment abstractSegment in species.physiology.segments){
			segment = OrganPrototypes.Instance.LoadSegment(abstractSegment.prototypeIndex);
			segment.hitpoints = abstractSegment.hitpoints;
			OrganPrototypes.Instance.AttachSegment(this, segment, abstractSegment.basePosition);
			foreach(CreatureLimb abstractLimb in abstractSegment.limbs){
				limb = OrganPrototypes.Instance.LoadLimb(abstractLimb.prototypeIndex);
				limb.hitpoints = abstractLimb.hitpoints;
				OrganPrototypes.Instance.AttachLimb(segment, limb, abstractLimb.basePosition);
				if(abstractLimb.appendage != null){
					appendage = OrganPrototypes.Instance.LoadAppendage(abstractLimb.appendage.prototypeIndex);
					appendage.hitpoints = abstractLimb.appendage.hitpoints;
					OrganPrototypes.Instance.AttachAppendage(limb, appendage);
				}
			}
		}
	}

	void SetShadow(){
		shadow = Instantiate(Resources.Load<GameObject>("Prefabs/Characters/Shadow"));
		Shadow s = shadow.GetComponent<Shadow>();
		shadow.GetComponent<SpriteRenderer>().sprite = shadow.GetComponent<Shadow>().images[shadowIndex];
		s.root = this;
		//s.offset = new Vector3(0.16f,-0.1f,0);
		s.offset = new Vector3(0.03f,-0.22f,2);
	}

	void SetHealthBar(){
		healthBar = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ui_healthDisplay"));
		HealthBar h = healthBar.GetComponent<HealthBar>();
		h.root = this;
		h.offset = new Vector3(0.0f,2.0f,0);
	}

	void SetStance(){
		CombatMoveSet cms;
		CombatStance stance1 = new CombatStance();

		if (speciesId == 0){ //TO DO
			 //get rid of this check and just handle it correctly
			cms = new CombatMoveSet();
			cms.iconIndex = 0;
			cms.moves.Add(segments[1].limbs[0].combatActions[0]);//right major tentacle
			cms.moves.Add(segments[1].limbs[1].combatActions[0]);//left major tentacle
			stance1.combatMoveSets[0] = cms;
			
			cms = new CombatMoveSet();
			cms.iconIndex = 1;
			cms.moves.Add(segments[1].limbs[2].combatActions[0]);//bite
			stance1.combatMoveSets[1] = cms;
		}
		
		stances.Add(stance1);
	}


	void MouseMovement(){
		float spd = 100f;
		float speedInitialX = speedX;
		float speedInitialY = speedY;
		if(control.moveCommand){
			for(int i=0; i<segments.Count;i++){
				segments[i].PlayAnimation("run");
				for(int j=0;j<segments[i].limbs.Count;j++){
					if(segments[i].limbs[j].isReady){
						segments[i].limbs[j].PlayAnimation("run");
						if(segments[i].limbs[j].appendage != null){
							segments[i].limbs[j].appendage.PlayAnimation("run");
						}
					}
				}
			}
			Vector2 start = new Vector2(transform.position.x,transform.position.y);
			Vector2 end = new Vector2(control.commandX,control.commandY);
			float d = Vector2.Distance(start,end);
			

			if(d > 0.1){
				float angle = Mathf.Asin((end.y-start.y)/d);
				if (start.y <= end.y && start.x <= end.x){
					//nothing
				}else if (start.y <= end.y && start.x > end.x){
					angle = Mathf.PI - angle; 
				}else if (start.y > end.y && start.x > end.x){
					angle = Mathf.PI - angle; 
				}else if (start.y > end.y && start.x <= end.x){
					angle = 2*Mathf.PI + angle; 
				}

				speedX = spd*Mathf.Cos(angle);
				speedY = spd*Mathf.Sin(angle);
				float damping = 1/((Mathf.Abs(speedY)/spd)+1); //vertical movement is twice as expensive
				speedX = speedX * damping * Time.deltaTime;
				speedY = speedY * damping * Time.deltaTime;
			}
		}else{
			for(int i=0; i<segments.Count;i++){
				segments[i].PlayAnimation("idle");
				for(int j=0;j<segments[i].limbs.Count;j++){
					if(segments[i].limbs[j].isReady){
						segments[i].limbs[j].PlayAnimation("idle");
						if(segments[i].limbs[j].appendage != null){
							segments[i].limbs[j].appendage.PlayAnimation("idle");
						}
					}
				}
			}
			speedX -=  Mathf.Sign(speedX) * frictionForceX * Time.deltaTime;
			if(Mathf.Sign(speedX) != Mathf.Sign(speedInitialX)){
				speedX = 0;
			}
			speedY -=  Mathf.Sign(speedY) * frictionForceY * Time.deltaTime;
			if(Mathf.Sign(speedY) != Mathf.Sign(speedInitialY)){
				speedY = 0;
			}
		}
		if(speedX!=0){
			Vector3 theScale = transform.localScale;
			theScale.x = Mathf.Sign(speedX);
			transform.localScale = theScale;
		}
		GetComponent<Rigidbody2D>().velocity = new Vector2(speedX,speedY);
	}

	void ExplorationMain(){
		if(control.shift){
			if(contactedInstallation != null){
				contactedInstallation.GetComponent<InteractiveInstallation>().Interact(this);
			}
		}
	}

	void CombatMain(){
		if(control.attackCommand){
			Attack();
			// if(IsAttacking()){
			// 	TODO: playAttackAnimation
			// }
		}
	}

	void Attack(){
		CombatMoveSet moveSet;
		moveSet = GetSelectedMoveSet();
		if(moveSet != null){
			for(int i=0;i<moveSet.moves.Count;i++){
				CombatAction act = moveSet.moves[i];
				if (act != null && act.limb.CanAttack()){
					StartCoroutine(act.limb.Attack(act));
				}
			}
		}
	}

	private CombatMoveSet GetSelectedMoveSet(){
		CombatMoveSet moveSet = null;
		int actionIndex = 0;
		for(int i=0;i<4;i++){
			if(control.actionModifier[i]){
				actionIndex = i+1;
				break;
			}
		}
		if(control.shift){
			actionIndex += 5;
		}
		moveSet = stances[stanceId].combatMoveSets[actionIndex];
		return moveSet;
	}

	public bool IsAttacking(){
		foreach(CreatureBodySegment segment in segments){
			foreach(CreatureLimb limb in segment.limbs){
				if(limb.IsExecutingAttack()){
					return true;
				}
			}
		}
		return false;
	}
}
