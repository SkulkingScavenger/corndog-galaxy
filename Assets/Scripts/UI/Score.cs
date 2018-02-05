using UnityEngine;
using System.Collections;

//public class HudControl : MonoBehaviour
public class Score : MonoBehaviour
{
	public int score = 0;					// The player's score.

	private Control control;
	private Player player;	// Reference to the player control script.


	void Awake ()
	{
		// Setting up the reference.
		control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
		player = control.players[control.currentPlayerId];
	}


	void Update ()
	{
		// Set the score text.
		GetComponent<GUIText>().text = "Score: " + score;

		// Set the previous score to this frame's score.
	}

}
