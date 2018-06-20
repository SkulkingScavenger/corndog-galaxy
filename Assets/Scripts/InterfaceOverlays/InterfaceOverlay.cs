using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InterfaceOverlay : MonoBehaviour{
	public InteractiveInstallation installation = null;
	public bool isHudMenu = false;
	public CreatureControl control = null;
	public Creature user = null;
	public Transform cam;
	public GameObject InterfaceOverlayUI;
	
	void Awake(){
		Player localPlayer = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>().GetPlayer();
		cam = localPlayer.mainCamera.transform;
		transform.position = new Vector3(cam.position.x, cam.position.y+1.5f, transform.position.z);
		control = localPlayer.inputControl;
		user = localPlayer.creature;
		user.control = null;
		installation = user.interactionInstallation;
		InterfaceOverlayUI = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/InterfaceOverlays/InterfaceOverlayUI"), Vector3.zero, Quaternion.identity);
		InterfaceOverlayUI.transform.SetParent(localPlayer.mainCanvas.transform,false);
		InterfaceOverlayUI.transform.localPosition = new Vector3(0f,192f,0f);
		InterfaceOverlayAwake();
	}

	void Update(){
		if(ShouldClose()){
			Destroy(gameObject);
			return;
		}
		transform.position = new Vector3(cam.position.x, cam.position.y+1.5f, transform.position.z);
		InterfaceOverlayUpdate();
	}

	void OnDestroy(){
		if(user != null){
			user.control = control;
		}
		Destroy(InterfaceOverlayUI);
		InterfaceOverlayOnDestroy();
	}

	public bool ShouldClose(){
		return control.ctrl;
	}

	public virtual void InterfaceOverlayAwake(){}

	public virtual void InterfaceOverlayUpdate(){}

	public virtual void InterfaceOverlayOnDestroy(){}
}