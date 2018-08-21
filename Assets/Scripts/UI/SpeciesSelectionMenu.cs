using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SpeciesSelectionMenu : MonoBehaviour {
	public Player player = null;

	public void Awake(){
		transform.Find("Button0").GetComponent<Button>().onClick.AddListener(delegate { SelectSpecies(1); });
		transform.Find("Button1").GetComponent<Button>().onClick.AddListener(delegate { SelectSpecies(0); });
	}

	public void SelectSpecies(int speciesId){
		player.creatureObj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Characters/Creature"), Vector3.zero, Quaternion.identity);
		player.creature = player.creatureObj.GetComponent<Creature>();
		player.creature.controlID = player.GetComponent<NetworkIdentity>().netId.Value;
		player.creatureObj.transform.position = new Vector3(player.startX,player.startY,0); 
		NetworkServer.Spawn(player.creatureObj);
		player.creature.Init(speciesId);
		Destroy(transform.gameObject);
	}
}