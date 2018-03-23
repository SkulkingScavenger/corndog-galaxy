using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class NavigationConsoleOverlay : InterfaceOverlay{

	

	public GameObject starmap;
	public int selectionCoordinateX;
	public int selectionCoordinateY;
	public int shipCoordinateX;
	public int shipCoordinateY;

	void Awake(){
		Player localPlayer = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>().GetPlayer();
		cam = localPlayer.mainCamera.transform;
		transform.position = new Vector3(cam.position.x, cam.position.y+1.5f, transform.position.z);
		control = localPlayer.inputControl;
		user = localPlayer.creature;
		user.control = null;
		installation = user.interactionInstallation;
		NavigationConsoleAwake();
	}

	void Update(){
		if(ShouldClose()){
			Destroy(gameObject);
			return;
		}
		transform.position = new Vector3(cam.position.x, cam.position.y+1.5f, transform.position.z);
		NavigationConsoleUpdate();
	}

	void Destroy(){
		if(user != null){
			user.control = control;
		}
	}

	private bool ShouldClose(){
		return control.ctrl;
	}

	public void NavigationConsoleAwake(){
		//starmap = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/InterfaceOverlays/NavigationConsoleDisplay"), Vector3.zero, Quaternion.identity);
		starmap = transform.Find("StarMap").gameObject;
	}

	public void NavigationConsoleUpdate(){
		float starmapWidth = 368;
		float starmapHeight = 320f;
		Vector2 mousepos = new Vector2(control.commandX,control.commandY);
		Rect mapBounds = new Rect(
			starmap.transform.position.x-starmapWidth/256f,
			starmap.transform.position.y-starmapHeight/256f,
			starmapWidth/128f,
			starmapHeight/128f);
		if(mapBounds.Contains(mousepos)){
			if(control.moveCommand){

			}
			if(control.attackCommand){
				selectionCoordinateX = (int)Mathf.Round(((mousepos.x-mapBounds.x)/mapBounds.width)*368);
				selectionCoordinateY = (int)Mathf.Round((mapBounds.y-mousepos.y/mapBounds.height)*320f);
				transform.Find("indicatorX").position = new Vector3(mapBounds.x+selectionCoordinateX/128f, mapBounds.y,-1);
				transform.Find("indicatorY").position = new Vector3(mapBounds.x, mapBounds.y+selectionCoordinateY/128f,-1);
			}
		}
	}
}