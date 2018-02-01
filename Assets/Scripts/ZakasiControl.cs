using UnityEngine;
using System.Collections;

public class ZakasiControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.
	public float z = 0;
	public float zspeed = 0;

	public GameObject shadow;
	public GameObject shadowPrefab;

	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeedX = 4f;				// The fastest the player can travel in the x axis.
	public float maxSpeedY = 2f;				// The fastest the player can travel in the x axis.
	public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.

	

	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = false;			// Whether or not the player is grounded.
	private Animator anim;					// Reference to the player's animator component.

	private float frictionForce = 2f;




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
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

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

		// If the player is changing direction or hasn't reached maxSpeed yet add a force to the player.
		if(h * GetComponent<Rigidbody2D>().velocity.x < maxSpeedX){
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);
		}
		if(v * GetComponent<Rigidbody2D>().velocity.y < maxSpeedY){
			GetComponent<Rigidbody2D>().AddForce(Vector2.up * v * moveForce);
		}

		// If the player's velocity is greater than the maxSpeed set the player's velocity to the maxSpeed.
		if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeedX){
			GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeedX, GetComponent<Rigidbody2D>().velocity.y);
		}
		if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > maxSpeedY){
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, Mathf.Sign(GetComponent<Rigidbody2D>().velocity.y) * maxSpeedY);
		}


		// Jump
		if(!grounded){
			zspeed -= 0.00098f;
			z += zspeed;
		}
		if(z <= 0){
			zspeed = 0;
			z = 0;
			grounded = true;
		}

		if(Input.GetButtonDown("Jump") && grounded){
			zspeed = 0.07f;
			grounded = false;
		}



		//update graphics
		GameObject.Find("zakasi_graphics").transform.position = new Vector3(transform.position.x, transform.position.y + 0.11f + z,0);


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
		s.offset = new Vector3(0.1293f,-1.18f,0);
		
		//GameObject.Find("shadow").transform.position = new Vector3(transform.position.x + -0.2093f, -0.1328f + z, 0);

	}
}
//-8.168782		0.5335
//-8.31			0.561

//0.141218

//3328