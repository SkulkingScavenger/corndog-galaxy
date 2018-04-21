using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class StructuralOrganMenu : MonoBehaviour {
	public SpeciesMenuOrganItem menuOrganItem = null;
	public SpeciesCreationMenu root = null;

	public void Awake(){

	}

	public void Save(){
		GameObject menuItem = GameObject.Instantiate(menuOrganItem.gameObject);
		menuItem.transform.SetParent(menuOrganItem.transform);
		menuOrganItem.transform.Find("ExpandButton").GetComponent<Button>().interactable = true;
		menuOrganItem.isExpanded = true;
		menuOrganItem.SetExpandButton();
		GameObject.Destroy(gameObject);
	}

	public void Cancel(){
		GameObject.Destroy(gameObject);
	}

	string GetAvailableOrganTypes(string parentType){
		string type ="";
		switch(parentType){
		case "form":
			break;
		case "segment":
			break;
		case "limb":
			break;
		case "appendage":
			break;
		}
		return type;
	}
}