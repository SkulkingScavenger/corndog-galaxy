using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HudCombat : MonoBehaviour {

	Player player;
	CreatureControl control; 

	public void Awake(){
		player = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>().GetPlayer();
		control = player.inputControl;

		transform.Find("ExplorationModeButton").GetComponent<Button>().onClick.AddListener(delegate { SetInterfaceModeExploration(); });
		transform.Find("IndustryModeButton").GetComponent<Button>().onClick.AddListener(delegate { SetInterfaceModeExploration(); });

	}

	public void Update(){

	}

	public void SetInterfaceModeExploration(){
		player.SetInterfaceMode("exploration");
	}
	public void SetInterfaceModeIndustry(){
		player.SetInterfaceMode("industry");
	}
}