using UnityEngine;
using System.Collections;

public class Shadow : MonoBehaviour
{
	public Vector3 offset;			// The offset at which the Shadow follows the root.
	public Sprite[] images;
	public Creature root = null;		// Reference to the root.


	void Awake ()
	{

	}

	void Update ()
	{
		// Set the position to the player's position with the offset.
		if(root != null){
			transform.position = new Vector3(root.transform.position.x + offset.x, root.transform.position.y + offset.y, 0);
			transform.localScale = root.transform.localScale;
		}
	}
}
