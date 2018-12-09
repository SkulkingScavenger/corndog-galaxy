using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SpeciesMenuOrganItem : MonoBehaviour {
	public GameObject preview;
	public SpeciesCreationMenu root;
	public List<SpeciesMenuOrganItem> children = new List<SpeciesMenuOrganItem>();
	public bool isExpanded = false;
	Sprite[] hudSprites;

	public string type = "form";
	public float x = 0;
	public float y = 0;
	public float z = 0;
	public float height = 1;
	public float width = 1;


	void Awake () {
		transform.Find("EditButton").GetComponent<Button>().onClick.AddListener(delegate { Edit(); });
		transform.Find("DeleteButton").GetComponent<Button>().onClick.AddListener(delegate { Delete(); });
		transform.Find("AddChildButton").GetComponent<Button>().onClick.AddListener(delegate { AddChild(); });
		transform.Find("ExpandButton").GetComponent<Button>().onClick.AddListener(delegate { Expand(); });

		transform.Find("ExpandButton").GetComponent<Button>().interactable = false;
		root = transform.root.gameObject.GetComponent<SpeciesCreationMenu>();
		hudSprites = Resources.LoadAll<Sprite>("Sprites/_UI/hud_elements_01");
	}
	
	void Update () {

	}

	public void Expand(){
		isExpanded = !isExpanded;
	}

	public void SetExpandButton(){
		if(isExpanded){
			transform.Find("ExpandButton").Find("Image").GetComponent<Image>().sprite = hudSprites[26];
		}else{
			transform.Find("ExpandButton").Find("Image").GetComponent<Image>().sprite = hudSprites[25];
		}
	}

	public void Edit(){

	}

	public void Delete(){

	}

	public void AddChild(){
		root.childPanel = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/SpeciesCreationMenus/StructuralOrganPanel"),Vector3.zero,Quaternion.identity,root.transform);
		root.childPanel.GetComponent<StructuralOrganMenu>().menuOrganItem = this;
		root.childPanel.GetComponent<StructuralOrganMenu>().root = root.GetComponent<SpeciesCreationMenu>();
		root.childPanel.GetComponent<StructuralOrganMenu>().purpose = "create";
		root.childPanel.GetComponent<StructuralOrganMenu>().parentType = "form";
		root.childPanel.GetComponent<StructuralOrganMenu>().Init();
	}

}
