using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class SimpleCreature : NetworkBehaviour{
	public CreatureControl control = null;

	public float accelerationX = 100f;		// rate of change per second in x velocity while moving
	public float accelerationY = 50f;		// rate of change per second in Y velocity while moving
	public float speedX = 0f;				// The current velocity in the x axis.
	public float speedY = 0f;				// The current velocity in the y axis.
	public float maxSpeedX = 3f;			// The fastest the player can travel in the x axis.
	public float maxSpeedY = 1.5f;			// The fastest the player can travel in the x axis.
	private float frictionForceX = 20f;		// rate of change per second in x velocity due to friction
	private float frictionForceY = 10f;		// rate of change per second in y velocity due to friction

	public void Update(){
		if(control == null){
			return;
		}
		Movement();
	}

	void Movement(){
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
}