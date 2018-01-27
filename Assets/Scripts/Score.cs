using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
	public int score = 0;					// The player's score.


	private ZakasiControl zakasiControl;	// Reference to the player control script.
	private int previousScore = 0;			// The score in the previous frame.


	void Awake ()
	{
		// Setting up the reference.
		zakasiControl = GameObject.FindGameObjectWithTag("Player").GetComponent<ZakasiControl>();
	}


	void Update ()
	{
		// Set the score text.
		GetComponent<GUIText>().text = "Score: " + score;

		// If the score has changed...
		if(previousScore != score)
			// ... play a taunt.
			zakasiControl.StartCoroutine(zakasiControl.Taunt());

		// Set the previous score to this frame's score.
		previousScore = score;
	}

}
