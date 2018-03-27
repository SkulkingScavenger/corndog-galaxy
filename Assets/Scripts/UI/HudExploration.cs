using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HudExploration : MonoBehaviour {

	Player player;
	CreatureControl control; 
	Text detailsBox;

	public void Awake(){
		player = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>().GetPlayer();
		control = player.inputControl;

		detailsBox = transform.Find("DetailsBox").transform.Find("Text").GetComponent<Text>();

		transform.Find("IndustryModeButton").GetComponent<Button>().onClick.AddListener(delegate { SetInterfaceModeCombat(); });
		transform.Find("CombatModeButton").GetComponent<Button>().onClick.AddListener(delegate { SetInterfaceModeCombat(); });

	}

	public void Update(){
		if(player.creature != null){
			if(player.creature.contactedInstallation != null){
					if(player.creature.interactionInstallation != null){
						detailsBox.text = "Press Ctrl to stop using the " + player.creature.interactionInstallation.GetComponent<InteractiveInstallation>().installationName;
					}else{
						detailsBox.text = "Press Shift to use the " + player.creature.contactedInstallation.GetComponent<InteractiveInstallation>().installationName;
					}
				}else{
					detailsBox.text = "All Systems Stable";
				}
		}else{
			detailsBox.text = "You Are Dead";
		}

	}

	public void SetInterfaceModeIndustry(){
		player.SetInterfaceMode("industry");
	}

	public void SetInterfaceModeCombat(){
		player.SetInterfaceMode("combat");
	}
}