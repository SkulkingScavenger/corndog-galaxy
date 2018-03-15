using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Creature : NetworkBehaviour{
	public CreatureControl control = null;
	[SyncVar] public uint controlID = 0;
	[SyncVar] public bool facingRight = true;			// For determining which way the player is currently facing.
	private Animator anim;		// Reference to the player's animator component.
	public Transform display;

	public GameObject shadow;
	public int shadowIndex = 1;
	public GameObject healthBar;
	[SyncVar] public int stanceId = 0;
	public List<CombatStance> stances = new List<CombatStance>();
	public List<CreatureOrgan> organs = new List<CreatureOrgan>();
	public List<CreatureLimb> limbs = new List<CreatureLimb>();
	
	public float jumpForce = 0.03f;			// Amount of force added when the player jumps.
	public float gravityForce = -0.0008f;			// Amount of force added when the player jumps.
	private bool grounded = true;			// Whether or not the player is grounded.
	[SyncVar] public float z = 0f;
	public float zspeed = 0f;
				
	public float accelerationX = 100f;		// rate of change per second in x velocity while moving
	public float accelerationY = 50f;		// rate of change per second in Y velocity while moving
	public float speedX = 0f;				// The current velocity in the x axis.
	public float speedY = 0f;				// The current velocity in the y axis.
	public float maxSpeedX = 3f;			// The fastest the player can travel in the x axis.
	public float maxSpeedY = 1.5f;			// The fastest the player can travel in the x axis.
	private float frictionForceX = 20f;		// rate of change per second in x velocity due to friction
	private float frictionForceY = 10f;		// rate of change per second in y velocity due to friction

	[SyncVar] public bool isAttacking = false;
	
	[SyncVar] public float health = 100f;					// The player's health.
	public float repeatDamagePeriod = 2f;		// How frequently the player can be damaged.

	public int playerId;



	void Awake(){
		display = transform.Find("Display");
		if(isLocalPlayer){
			//mainCamera.GetComponent<CameraObject>().root = getPlayer().creatureObj.transform;
		}
		Init();
	}

	public void Init(){
		SetLimbs();
		SetShadow();
		SetHealthBar();
		SetStance();
	}


	void Update(){
		if(control == null){
			control = ClientScene.FindLocalObject(new NetworkInstanceId(controlID)).GetComponent<CreatureControl>();
			return;
		}
		MouseMovement();
		Jump();

		if(control.interfaceMode == "combat"){
			CombatMain();
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

	void SetLimbs(){
		CreatureLimb organ;
		CreatureAppendage appendage;

		//Right Major Tentacle
		organ = OrganPrototypes.Instance.LoadLimb(0);
		organ.hitpoints = 4;
		OrganPrototypes.Instance.AttachLimb(this, organ, new Vector3(0.3593f,0.8665f,-0.001f));

		appendage = OrganPrototypes.Instance.LoadAppendage(0);
		appendage.hitpoints = 2;
		OrganPrototypes.Instance.AttachAppendage(organ, appendage);

		//Left Major Tentacle
		organ = OrganPrototypes.Instance.LoadLimb(1);
		organ.hitpoints = 4;
		OrganPrototypes.Instance.AttachLimb(this, organ, new Vector3(0.08609991f,0.9372001f,0.001f));

		appendage = OrganPrototypes.Instance.LoadAppendage(1);
		appendage.hitpoints = 2;
		OrganPrototypes.Instance.AttachAppendage(organ, appendage);
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
		CombatStance stance1 = new CombatStance();
		for (int i=0;i<limbs.Count;i++){
			stance1.componentList.Add(new CombatStanceComponent(limbs[i]));
			stance1.componentList[i].actions[0] = limbs[i].combatActions[0];
		}
		
		stances.Add(stance1);
	}


	void MouseMovement(){
		float spd = 100f;
		float speedInitialX = speedX;
		float speedInitialY = speedY;
		if(control.moveCommand){
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

	void CombatMain(){
		if(control.attackCommand){
			CombatAction();
		}
	}

	void CombatAction(){
		bool attackSuccessful = false;
		if(stances[stanceId].componentList.Count > 0 && !isAttacking){
			for(int i=0;i<stances[stanceId].componentList.Count;i++){
				CombatStanceComponent component = stances[stanceId].componentList[i];
				int actionIndex = 0;
				if (component.limb.hitpoints > 0 && !component.limb.isParalyzed && component.limb.isReady){
					for(int j=0;j<4;j++){
						if(control.actionModifier[j]){
							actionIndex = j+1;
							break;
						}
					}
					if(control.shift){
						actionIndex += 5;
					}
					CombatAction act = component.actions[actionIndex];
					if(act != null){
						StartCoroutine(component.limb.Attack(act));
					}
					attackSuccessful = true;
				}
			}
		}
		if(attackSuccessful){
			//isAttacking = true;
			//anim.Play("attack");
		}
	}
}
