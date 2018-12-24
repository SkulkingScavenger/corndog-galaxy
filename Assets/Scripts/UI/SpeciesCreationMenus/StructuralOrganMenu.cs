using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class StructuralOrganMenu : MonoBehaviour {
	public SpeciesMenuOrganItem menuOrganItem = null;
	public SpeciesCreationMenu root = null;
	public string purpose;
	public string parentType;
	public string organType;
	public string Type;

	private string name;
	private string material;
	private string structureType;
	private int prototypeId;

	private TMP_Text nameInput;
	private Transform structureSelector;
	private Transform surfaceSelector;
	private string[] surfaceTypes = {"none","chitin","skin","rubbery flesh","fur","feathers","protoplasm","plates"};
	private int selectedStructureIndex = 0;
	private int selectedSurfaceIndex = 0;
	private List<GameObject> structureOptionButtons = new List<GameObject>();
	private List<GameObject> surfaceOptionButtons = new List<GameObject>();

	private Sprite buttonActive;
	private Sprite buttonInactive;

	public void Awake(){
		transform.Find("SaveButton").GetComponent<Button>().onClick.AddListener(delegate { Save(); });
		transform.Find("CancelButton").GetComponent<Button>().onClick.AddListener(delegate { Cancel(); });
		nameInput = transform.Find("NameInput").GetComponent<TMP_Text>();
		structureSelector = transform.Find("StructureScrollbox");
		surfaceSelector = transform.Find("SurfaceScrollbox");

		
	}

	public void Init(){

		buttonInactive = root.buttonSprites[2];
		buttonActive = root.buttonSprites[6];
		switch(purpose){
			case "create":

				break;
			case "edit":
				break;
		}
		organType = GetAvailableOrganTypes();
		loadStructureOptions(organType);
	}

	public void Save(){
		GameObject menuItem = GameObject.Instantiate(menuOrganItem.gameObject);
		menuItem.transform.SetParent(menuOrganItem.transform);
		menuOrganItem.transform.Find("ExpandButton").GetComponent<Button>().interactable = true;
		menuOrganItem.isExpanded = true;
		menuOrganItem.SetExpandButton();

		//Vector3 rootPos = new Vector3(menuOrganItem.transform.position.x,menuOrganItem.transform.position.y,menuOrganItem.transform.position.z);
		GameObject previewNode = GameObject.Instantiate(menuOrganItem.preview, Vector3.zero, Quaternion.identity, menuOrganItem.preview.transform.parent);

		GameObject.Destroy(gameObject);
	}

	public void Cancel(){
		GameObject.Destroy(gameObject);
	}

	private string GetAvailableOrganTypes(){
		string type = "";
		switch(parentType){
		case "form":
			type = "segment";
			break;
		case "segment":
			type = "limb";
			break;
		case "limb":
			type = "appendage";
			break;
		case "appendage":
			break;
		}
		return type;
	}

	void SelectStructure(int index){
		Debug.Log(index);
		structureOptionButtons[selectedStructureIndex].GetComponent<Image>().sprite = buttonInactive;
		structureOptionButtons[index].GetComponent<Image>().sprite = buttonActive;
		selectedStructureIndex = index;
	}

	void SelectSurface(int index){

	}

	private void loadStructureOptions(string structure){
		GameObject buttonPrototype;
		
		GameObject newButton;
		Transform buttonContainer = structureSelector.Find("Viewport").Find("Content");
		buttonPrototype = buttonContainer.Find("Button0").gameObject;
		Vector3 prototypePosition = buttonPrototype.transform.position;
		switch(structure){
		case "segment":
			for(int i=0;i<OrganPrototypes.Instance.segmentPrototypes.Count;i++){
				newButton = Instantiate(buttonPrototype, new Vector3(prototypePosition.x,prototypePosition.y - i*0.375f,prototypePosition.z), Quaternion.identity, buttonContainer);
				int j = i;
				newButton.GetComponent<Button>().onClick.AddListener(delegate { SelectStructure(j); });
				newButton.transform.Find("Text").gameObject.GetComponent<Text>().text = OrganPrototypes.Instance.segmentPrototypes[i].name;
				newButton.name = "Button" + i.ToString();
				structureOptionButtons.Add(newButton);
			}
			break;
		case "limb":
			for(int i=0;i<OrganPrototypes.Instance.limbPrototypes.Count;i++){
				newButton = Instantiate(buttonPrototype, new Vector3(prototypePosition.x,prototypePosition.y - i*0.375f,prototypePosition.z), Quaternion.identity, buttonContainer);
				int j = i;
				newButton.GetComponent<Button>().onClick.AddListener(delegate { SelectStructure(j); });
				newButton.transform.Find("Text").gameObject.GetComponent<Text>().text = OrganPrototypes.Instance.limbPrototypes[i].name;
				newButton.name = "Button" + i.ToString();
				structureOptionButtons.Add(newButton);
			}
			break;
		case "appendage":
			for(int i=0;i<OrganPrototypes.Instance.appendagePrototypes.Count;i++){
				newButton = Instantiate(buttonPrototype, new Vector3(prototypePosition.x,prototypePosition.y - i*0.375f,prototypePosition.z), Quaternion.identity, buttonContainer);
				int j = i;
				newButton.GetComponent<Button>().onClick.AddListener(delegate { SelectStructure(j); });
				newButton.transform.Find("Text").gameObject.GetComponent<Text>().text = OrganPrototypes.Instance.appendagePrototypes[i].name;
				newButton.name = "Button" + i.ToString();
				structureOptionButtons.Add(newButton);
			}
			break;
		}
		GameObject.Destroy(buttonPrototype);
	}

	private void loadSurfaceOptions(){
		GameObject buttonPrototype;
		GameObject newButton;
		Transform buttonContainer = surfaceSelector.Find("Viewport").Find("Content");
		buttonPrototype = buttonContainer.Find("Button0").gameObject;
		Vector3 prototypePosition = buttonPrototype.transform.position;

		for(int i=0;i<surfaceTypes.Length;i++){
			newButton = Instantiate(buttonPrototype, new Vector3(prototypePosition.x,prototypePosition.y - i*0.375f,prototypePosition.z), Quaternion.identity, buttonContainer);
			newButton.GetComponent<Button>().onClick.AddListener(delegate { SelectStructure(i); });
			newButton.transform.Find("Text").gameObject.GetComponent<Text>().text = surfaceTypes[i];
			newButton.name = "Button" + i.ToString();
			surfaceOptionButtons.Add(newButton);
		}

		GameObject.Destroy(buttonPrototype);
	}
}