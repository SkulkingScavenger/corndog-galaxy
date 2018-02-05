using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Creature : MonoBehaviour
{
	public bool facingRight = true;			// For determining which way the player is currently facing.
	private Animator anim;		// Reference to the player's animator component.

	public GameObject shadow;
	public int shadowIndex = 0;
	public GameObject healthBar;
	public Player player;
	public int stanceId = 0;
	public List<CombatStance> stances = new List<CombatStance>();
	public List<CreatureOrgan> organs = new List<CreatureOrgan>();
	public List<CreatureLimb> limbs = new List<CreatureLimb>();
	
	//public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public float jumpForce = 0.03f;			// Amount of force added when the player jumps.
	public float gravityForce = -0.0008f;			// Amount of force added when the player jumps.
	private bool grounded = true;			// Whether or not the player is grounded.
	public float z = 0f;
	public float zspeed = 0f;
				
	//public float moveForceX = 20f;		// Amount of force added to move the player left and right.
	//public float moveForceY = 10f;		// Amount of force added to move the player left and right.
	public float accelerationX = 100f;		// The fastest the player can travel in the x axis.
	public float accelerationY = 50f;	// The fastest the player can travel in the x axis.
	public float speedX = 0f;				// The fastest the player can travel in the x axis.
	public float speedY = 0f;				// The fastest the player can travel in the x axis.
	public float maxSpeedX = 2f;				// The fastest the player can travel in the x axis.
	public float maxSpeedY = 1f;				// The fastest the player can travel in the x axis.
	private float frictionForceX = 20f;
	private float frictionForceY = 10f;
	
	public float health = 100f;					// The player's health.
	public float repeatDamagePeriod = 2f;		// How frequently the player can be damaged.



	void Awake(){
		anim = GetComponent<Animator>();	
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
		GameObject.Find("Display").transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y + 0.55f + z,0);
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
		GameObject limbObject = Instantiate(Resources.Load<GameObject>("Prefabs/Characters/MajorTentacleR"));
		SpriteRenderer sr = limbObject.GetComponent<SpriteRenderer>();
		limbObject.transform.parent = transform.Find("Display").transform;
		sr.transform.position = new Vector3(transform.Find("Display").transform.position.x + 0.3593f,transform.Find("Display").transform.position.y + 0.8665f,0);
		
		CreatureLimb organ = new CreatureLimb();
		organ.name = "Left Major Tentacle";
		organ.root = this;
		organ.offset = new Vector3(0.3593f,0.8665f,0);
		organ.limbType = "tentacle";
	
		organ.obj = limbObject;
		limbs.Add(organ);
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
		stance1.componentList.Add(new CombatStanceComponent(limbs[0]));
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
		if(Input.GetAxis("MouseRight") != 0){
			CombatAction();
		}
	}

	void CombatAction(){
		for(int i=0;i<stances[stanceId].componentList.Count;i++){
			CombatStanceComponent component = stances[stanceId].componentList[i];
			int actionIndex = 4;
			// if (component.limb.hitpoints > 0 && component.limb.isParalyzed == false && component.limb.isReady){
			// 	if (Input.GetKey ("w")){
			// 		actionIndex = 0;
			// 	}else if(Input.GetKey ("a")){
			// 		actionIndex = 1;
			// 	}else if(Input.GetKey ("s")){
			// 		actionIndex = 2;
			// 	}else if(Input.GetKey ("d")){
			// 		actionIndex = 3;
			// 	}
			// 	component.action[actionIndex](Input.GetAxis("Shift") != 0);
			// }
		}
	}
}
