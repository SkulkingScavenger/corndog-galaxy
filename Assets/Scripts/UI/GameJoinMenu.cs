using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class GameJoinMenu : MonoBehaviour {

	InputField IpInput;
	InputField PortInput;

	public void Awake(){
		IpInput = transform.Find("Panel").transform.Find("IpInput").GetComponent<InputField>();
		PortInput = transform.Find("Panel").transform.Find("PortInput").GetComponent<InputField>();

		IpInput.onValueChanged.AddListener(delegate { ManageIpInput(); });
		PortInput.onValueChanged.AddListener(delegate { ManagePortInput(); });

		transform.Find("Panel").transform.Find("Button0").GetComponent<Button>().onClick.AddListener(delegate { JoinGame(); });
		transform.Find("Panel").transform.Find("Button1").GetComponent<Button>().onClick.AddListener(delegate { ReturnToTitle(); });

	}

	public void ManageIpInput(){
		//Debug.Log(IpInput.text);
	}

	public void ManagePortInput(){
		//Debug.Log(PortInput.text);
	}

	public void JoinGame(){
		NetworkControl nc = GameObject.FindGameObjectWithTag("NetworkControl").GetComponent<NetworkControl>();
		nc.networkAddress = IpInput.text;
		nc.networkPort = int.Parse(PortInput.text);
		GameObject.FindGameObjectWithTag("Control").GetComponent<Control>().JoinServer();
	}

	public void ReturnToTitle(){
		Transform mainCanvas = transform.parent;
		GameObject currentMenu = Instantiate(Resources.Load<GameObject>("Prefabs/UI/TitleMenu"));
		currentMenu.transform.SetParent(mainCanvas,false);
		Destroy(transform.gameObject);
	}

}