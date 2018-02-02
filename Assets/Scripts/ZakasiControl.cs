using UnityEngine;
using System.Collections;

public class ZakasiControl : MonoBehaviour
{
	[HideInInspector]public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]private Animator anim;		// Reference to the player's animator component.

	[HideInInspector]public GameObject shadow;
	[HideInInspector]public GameObject shadowPrefab;
	
	//[HideInInspector]public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	[HideInInspector]public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	[HideInInspector]private Transform groundCheck;			// A position marking where to check if the player is grounded.
	[HideInInspector]private bool grounded = true;			// Whether or not the player is grounded.
	[HideInInspector]public bool jump = false;				// Condition for whether the player should jump.
	[HideInInspector]public float z = 0f;
	[HideInInspector]public float zspeed = 0f;
				
	//[HideInInspector]public float moveForceX = 20f;		// Amount of force added to move the player left and right.
	//[HideInInspector]public float moveForceY = 10f;		// Amount of force added to move the player left and right.
	[HideInInspector]public float accelerationX = 100f;		// The fastest the player can travel in the x axis.
	[HideInInspector]public float accelerationY = 50f;	// The fastest the player can travel in the x axis.
	[HideInInspector]public float speedX = 0f;				// The fastest the player can travel in the x axis.
	[HideInInspector]public float speedY = 0f;				// The fastest the player can travel in the x axis.
	[HideInInspector]public float maxSpeedX = 2f;				// The fastest the player can travel in the x axis.
	[HideInInspector]public float maxSpeedY = 1f;				// The fastest the player can travel in the x axis.
	[HideInInspector]private float frictionForceX = 20f;
	[HideInInspector]private float frictionForceY = 10f;
	



	void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();

		SetShadow();
	}


	void Update()
	{
		//main movement
		float speedInitialX = speedX;
		float speedInitialY = speedY;
		float h = 0;
		float v = 0;
		float hInput = Input.GetAxis("Horizontal");
		float vInput = Input.GetAxis("Vertical");

		if(hInput != 0){
			h = Mathf.Sign(hInput);
		}
		if(vInput != 0){
			v = Mathf.Sign(vInput);
		}

		//acceleration/deceleration
		if(h!=0){
			//speedX += h * accelerationX * Time.deltaTime;
			speedX = h * maxSpeedX;
		}else{
			speedX -=  Mathf.Sign(speedX) * frictionForceX * Time.deltaTime;
			if(Mathf.Sign(speedX) != Mathf.Sign(speedInitialX)){
				speedX = 0;
			}
		}
		if(v!=0){
			//speedY += v * accelerationY * Time.deltaTime;
			speedY = v * maxSpeedY;
		}else{
			speedY -=  Mathf.Sign(speedY) * frictionForceY * Time.deltaTime;
			if(Mathf.Sign(speedY) != Mathf.Sign(speedInitialY)){
				speedY = 0;
			}
		}

		//enforce min/max speeds
		if(Mathf.Abs(speedX) > maxSpeedX){speedX = maxSpeedX * Mathf.Sign(speedX);}
		if(Mathf.Abs(speedY) > maxSpeedY){speedY = maxSpeedY * Mathf.Sign(speedY);}

		//apply motion
		GetComponent<Rigidbody2D>().velocity = new Vector2(speedX,speedY);


		/*
		// If the player is changing direction or hasn't reached maxSpeed yet add a force to the player.
		if(h * GetComponent<Rigidbody2D>().velocity.x < maxSpeedX){
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForceX);
		}
		if(v * GetComponent<Rigidbody2D>().velocity.y < maxSpeedY){
			GetComponent<Rigidbody2D>().AddForce(Vector2.up * v * moveForceY);
		}

		// If the player's velocity is greater than the maxSpeed set the player's velocity to the maxSpeed.
		if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeedX){
			GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeedX, GetComponent<Rigidbody2D>().velocity.y);
		}
		if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > maxSpeedY){
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, Mathf.Sign(GetComponent<Rigidbody2D>().velocity.y) * maxSpeedY);
		}

		//friction effect

		if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > 0){
			GetComponent<Rigidbody2D>().velocity = new Vector2((GetComponent<Rigidbody2D>().velocity.x - Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * frictionForce), GetComponent<Rigidbody2D>().velocity.y);
			if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) < frictionForce){
				GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
			}
		}
		if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > 0){
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, (GetComponent<Rigidbody2D>().velocity.y - Mathf.Sign(GetComponent<Rigidbody2D>().velocity.y) * frictionForce));
			if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) < frictionForce){
				GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x,0);
			}
		}
		*/
		



		// Jump
		if(!grounded){
			zspeed -= 0.0008f;
			z += zspeed;
		}
		if(z <= 0){
			zspeed = 0;
			z = 0;
			grounded = true;
		}

		if(Input.GetButtonDown("Jump") && grounded){
			zspeed = 0.03f;
			grounded = false;
		}



		//update graphics
		GameObject.Find("zakasi_graphics").transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y + 0.55f + z,0);


		// Flip Sprite
		if(h > 0 && !facingRight)
			Flip();
		else if(h < 0 && facingRight)
			Flip();

	}
	
	 
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void SetShadow (){
		shadow = Instantiate(shadowPrefab, transform.position, transform.rotation);
		Shadow s = shadow.GetComponent<Shadow>();
		SpriteRenderer sr = shadow.GetComponent<SpriteRenderer>();
		sr.sprite = s.images[0];
		s.root = this;
		s.offset = new Vector3(0.16f,-0.1f,0);
		
		//GameObject.Find("shadow").transform.position = new Vector3(transform.position.x + -0.2093f, -0.1328f + z, 0);

	}
}
