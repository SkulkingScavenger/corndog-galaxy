using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{	

	private SpriteRenderer healthBar;			// Reference to the sprite renderer of the health bar.
	private float lastHitTime;					// The time at which the player was last hit.
	private Vector3 healthScale;				// The local scale of the health bar initially (with full health).
	public Creature root = null;		// Reference to the ZakasiControl script.
	public Vector3 offset;			// The offset at which the Shadow follows the root.

	void Awake ()
	{
		// Setting up references.
		healthBar = GameObject.Find("HealthBar").GetComponent<SpriteRenderer>();

		// Getting the intial scale of the healthbar (whilst the player has full health).
		healthScale = healthBar.transform.localScale;
	}

	void Update(){
		if(root != null){
			transform.position = new Vector3(root.transform.position.x + offset.x, root.transform.position.y + offset.y + root.z, 0);
		}
	}



	public void UpdateHealthBar ()
	{
		// Set the health bar's colour to proportion of the way between green and red based on the player's health.
		healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - root.health * 0.01f);

		// Set the scale of the health bar to be proportional to the player's health.
		healthBar.transform.localScale = new Vector3(healthScale.x * root.health * 0.01f, 1, 1);
	}
}
