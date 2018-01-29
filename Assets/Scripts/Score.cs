using UnityEngine;
using System.Collections;

public class HudControl : MonoBehaviour
{
	public int score = 0;					// The player's score.


	private ZakasiControl zakasiControl;	// Reference to the player control script.


	void Awake ()
	{
		// Setting up the reference.
		zakasiControl = GameObject.FindGameObjectWithTag("Player").GetComponent<ZakasiControl>();
	}


	void Update ()
	{
		// Set the score text.
		GetComponent<GUIText>().text = "Score: " + score;

		// Set the previous score to this frame's score.
	}

}
