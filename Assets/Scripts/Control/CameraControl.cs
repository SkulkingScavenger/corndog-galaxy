using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	public float xMargin = 1f;		// Distance in the x axis the player can move before the camera follows.
	public float yMargin = 1f;		// Distance in the y axis the player can move before the camera follows.
	public float xSmooth = 80f;		// How smoothly the camera catches up with it's target movement in the x axis.
	public float ySmooth = 80f;		// How smoothly the camera catches up with it's target movement in the y axis.

	public float speed;
	private float avg;
	private float lastframe = 0f;
	private float currentframe = 0f;
	private float myDelta = 0f;


	public Transform root = null;		// Reference to the player's transform.


	void Awake (){

	}

	void Update (){
		//calc my delta time
		currentframe = Time.realtimeSinceStartup;
		myDelta = currentframe - lastframe;
		lastframe = currentframe; 
	}

	void LateUpdate(){
		avg = (Time.deltaTime + Time.smoothDeltaTime + myDelta)*0.33333f;
		 //transform.position += new Vector3(speed, 0f, 0f) * avg;
		if(root != null){
			TrackPlayer();
		}else{
			transform.position = new Vector3(0, 0, transform.position.z);
		}
		if(Control.Instance.currentArea != null){
			Control.Instance.currentArea.transform.Find("Sky").transform.position = transform.position + new Vector3(0,0,20);
		}
	}

	private bool CheckXMargin(){
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return Mathf.Abs(transform.position.x - root.position.x) > xMargin;
	}
	
	private void TrackPlayer (){
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = transform.position.x;
		float targetY = Mathf.Round((root.position.y*128)/768) * 6 ;

		// If the player has moved beyond the x margin...
		if(CheckXMargin()){
			// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
			targetX = Mathf.Lerp(transform.position.x, root.position.x, avg);
		}

		// The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
		//targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);

		// Set the camera's position to the target position with the same z component.
		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}
}
