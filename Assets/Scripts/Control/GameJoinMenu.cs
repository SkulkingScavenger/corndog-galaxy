using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class GameJoinMenu : MonoBehaviour {

	string ip;

	public void Awake(){
		transform.Find("Panel").transform.Find("InputField").GetComponent<InputField>().onValueChanged.AddListener(delegate { ManageInput(); });

		transform.Find("Panel").transform.Find("Button0").GetComponent<Button>().onClick.AddListener(delegate { JoinGame(); });
		transform.Find("Panel").transform.Find("Button1").GetComponent<Button>().onClick.AddListener(delegate { ReturnToTitle(); });

	}

	public void ManageInput(){
		//manage it
	}


	public void JoinGame(){
		GameObject.FindGameObjectWithTag("Control").GetComponent<Control>().EnterGame();
	}

	public void ReturnToTitle(){
		Transform mainCanvas = transform.parent;
		GameObject currentMenu = Instantiate(Resources.Load<GameObject>("Prefabs/UI/TitleMenu"));
		currentMenu.transform.SetParent(mainCanvas,false);
		Destroy(this);
	}

}