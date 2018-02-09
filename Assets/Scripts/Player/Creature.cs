using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Creature : MonoBehaviour{
	public bool facingRight = true;			// For determining which way the player is currently facing.
	private Animator anim;		// Reference to the player's animator component.
	public Transform display;

	public GameObject shadow;
	public int shadowIndex = 0;
	public GameObject healthBar;
	public Player player;
	public int stanceId = 0;
	public List<CombatStance> stances = new List<CombatStance>();
	public List<CreatureOrgan> organs = new List<CreatureOrgan>();
	public List<CreatureLimb> limbs = new List<CreatureLimb>();
	
	public float jumpForce = 0.03f;			// Amount of force added when the player jumps.
	public float gravityForce = -0.0008f;			// Amount of force added when the player jumps.
	private bool grounded = true;			// Whether or not the player is grounded.
	public float z = 0f;
	public float zspeed = 0f;
				
	public float accelerationX = 100f;		// The fastest the player can travel in the x axis.
	public float accelerationY = 50f;	// The fastest the player can travel in the x axis.
	public float speedX = 0f;				// The fastest the player can travel in the x axis.
	public float speedY = 0f;				// The fastest the player can travel in the x axis.
	public float maxSpeedX = 2f;				// The fastest the player can travel in the x axis.
	public float maxSpeedY = 1f;				// The fastest the player can travel in the x axis.
	private float frictionForceX = 20f;
	private float frictionForceY = 10f;

	public bool isAttacking = false;
	
	public float health = 100f;					// The player's health.
	public float repeatDamagePeriod = 2f;		// How frequently the player can be damaged.



	void Awake(){
		display = transform.Find("Display");
	}

	public void Init(){
		SetLimbs();
		SetShadow();
		SetHealthBar();
		SetStance();
	}


	void Update(){
		MouseMovement();
		Jump();

		//action
		if(player.interfaceMode == "combat"){
			CombatMain();
		}

		//update graphics
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

		if(Input.GetButtonDown("Jump") && grounded){
			zspeed = jumpForce;
			grounded = false;
		}

	}

	void SetLimbs(){
		CreatureLimb organ;
		CreatureAppendage appendage;
		CombatAction act;
		GameObject limbObject;


		//Right Major Tentacle
		organ = new CreatureLimb();
		OrganPrototypes.Instance.LoadLimb(organ,0);
		organ.hitpoints = 4;
		OrganPrototypes.Instance.AttachLimb(this, organ, new Vector3(0.3593f,0.8665f,-0.001f));

		appendage = new CreatureAppendage();
		appendage.name = "Right Major Tentacle Claw";
		appendage.root = this;
		appendage.offset = organ.appendageOffsets[0];
		appendage.hitpoints = 2;

		//create combat action for the limb
		act = new CombatAction();
		act.name = "Claw Snap";
		act.range = 2f;
		act.damage = 4;
		act.windupDuration = 0.25f;
		act.attackDuration = 0.25f;
		act.cooldownDuration = 0.25f;
		act.idleAnimation = "major_tentacle_claw_r_idle";
		act.windupAnimation = "major_tentacle_claw_r_windup";
		act.attackAnimation = "major_tentacle_claw_r_attack";
		appendage.combatActions.Add(act);
	
		//create physical manifestation
		limbObject = Instantiate(Resources.Load<GameObject>("Prefabs/Characters/LimbObject"));
		limbObject.transform.parent = organ.obj.transform;
		limbObject.GetComponent<SpriteRenderer>().transform.position = new Vector3(display.transform.position.x + organ.offset.x,display.transform.position.y + organ.offset.y, organ.offset.z);
		limbObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Controllers/major_tentacle_claw_r");
		appendage.obj = limbObject;

		organ.appendage = appendage;


		//Left Major Tentacle
		organ = new CreatureLimb();
		OrganPrototypes.Instance.LoadLimb(organ,1);
		organ.hitpoints = 4;
		OrganPrototypes.Instance.AttachLimb(this, organ, new Vector3(0.08609991f,0.9372001f,0.001f));

		appendage = new CreatureAppendage();
		appendage.name = "Left Major Tentacle Claw";
		appendage.root = this;
		appendage.offset = organ.appendageOffsets[0];
		appendage.hitpoints = 2;

		//create combat action for the limb
		act = new CombatAction();
		act.name = "Claw Snap";
		act.range = 2f;
		act.damage = 4;
		act.windupDuration = 0.25f;
		act.attackDuration = 0.25f;
		act.cooldownDuration = 0.25f;
		act.idleAnimation = "major_tentacle_claw_l_idle";
		act.windupAnimation = "major_tentacle_claw_l_windup";
		act.attackAnimation = "major_tentacle_claw_l_attack";
		appendage.combatActions.Add(act);
	
		//create physical manifestation
		limbObject = Instantiate(Resources.Load<GameObject>("Prefabs/Characters/LimbObject"));
		limbObject.transform.parent = organ.obj.transform;
		limbObject.GetComponent<SpriteRenderer>().transform.position = new Vector3(display.transform.position.x + organ.offset.x,display.transform.position.y + organ.offset.y, organ.offset.z);
		limbObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Controllers/major_tentacle_claw_l");
		appendage.obj = limbObject;

		organ.appendage = appendage;


		// organ = new CreatureLimb();
		// organ.name = "Left Major Tentacle";
		// organ.root = this;
		// organ.offset = new Vector3(0.08609991f,0.9372001f,0.001f);
		// organ.limbType = "tentacle";
		// organ.hitpoints = 4;
		
		// //create combat action for the limb
		// act = new CombatAction();
		// act.name = "tentacle lash";
		// act.range = 2f;
		// act.damage = 4;
		// act.windupDuration = 0.25f;
		// act.attackDuration = 0.25f;
		// act.cooldownDuration = 0.25f;
		// act.idleAnimation = "major_tentacle_l_idle";
		// act.windupAnimation = "major_tentacle_l_windup";
		// act.attackAnimation = "major_tentacle_l_attack";
		// organ.combatActions.Add(act);
	
		// //create physical manifestation
		// limbObject = Instantiate(Resources.Load<GameObject>("Prefabs/Characters/LimbObject"));
		// limbObject.transform.parent = transform.Find("Display").transform;
		// limbObject.GetComponent<CreatureLimbObject>().root = organ;
		// limbObject.GetComponent<SpriteRenderer>().transform.position = new Vector3(display.transform.position.x + organ.offset.x,display.transform.position.y + organ.offset.y, organ.offset.z);
		// limbObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Controllers/major_tentacle_l");
		// organ.obj = limbObject;

		// limbs.Add(organ);
	}

	void SetShadow(){
		shadow = Instantiate(Resources.Load<GameObject>("Prefabs/Characters/Shadow"));
		Shadow s = shadow.GetComponent<Shadow>();
		SpriteRenderer sr = shadow.GetComponent<SpriteRenderer>();
		sr.sprite = s.images[shadowIndex];
		s.root = this;
		s.offset = new Vector3(0.16f,-0.1f,0);
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
		if(Input.GetAxis("MouseRight") != 0){
			Vector3 mouseCoordinates = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 start = new Vector2(transform.position.x,transform.position.y);
			Vector2 end = new Vector2(mouseCoordinates.x,mouseCoordinates.y);
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
		if(Input.GetAxis("MouseLeft") != 0){
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
					if (Input.GetKey ("w")){
						actionIndex = 1;
					}else if(Input.GetKey ("a")){
						actionIndex = 2;
					}else if(Input.GetKey ("s")){
						actionIndex = 3;
					}else if(Input.GetKey ("d")){
						actionIndex = 4;
					}
					if(Input.GetAxis("Shift") != 0){
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
