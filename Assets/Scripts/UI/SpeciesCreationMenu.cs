using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SpeciesCreationMenu : MonoBehaviour {
	GameObject mainCamera;

	string editMode = "Move";

	public GameObject previewRoot;
	public GameObject childPanel = null;

	public GameObject structurePanel;
	public GameObject previewPanel;
	public GameObject structureRoot;
	public GameObject selectedOrgan;

	public Species species = new Species();

	public Sprite[] hudSprites;
	public Sprite[] buttonSprites;

	public void Awake(){
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		hudSprites = Resources.LoadAll<Sprite>("Sprites/_UI/hud_elements_01");
		buttonSprites = Resources.LoadAll<Sprite>("Sprites/_UI/ui_buttons_zakasi");

		transform.Find("ControlPanel").transform.Find("LoadButton").GetComponent<Button>().onClick.AddListener(delegate { LoadSpecies(); });
		transform.Find("ControlPanel").transform.Find("SaveButton").GetComponent<Button>().onClick.AddListener(delegate { SaveSpecies(); });
		transform.Find("ControlPanel").transform.Find("BackButton").GetComponent<Button>().onClick.AddListener(delegate { ReturnToTitle(); });
		transform.Find("ControlPanel").transform.Find("RestartButton").GetComponent<Button>().onClick.AddListener(delegate { ClearAll(); });

		transform.Find("ToolBar").transform.Find("MoveButton").GetComponent<Button>().onClick.AddListener(delegate { ChangeEditMode("Move"); });
		transform.Find("ToolBar").transform.Find("RotateButton").GetComponent<Button>().onClick.AddListener(delegate { ChangeEditMode("Rotate"); });
		transform.Find("ToolBar").transform.Find("SelectButton").GetComponent<Button>().onClick.AddListener(delegate { ChangeEditMode("Select"); });
		transform.Find("ToolBar").transform.Find("LinkButton").GetComponent<Button>().onClick.AddListener(delegate { ChangeEditMode("Link"); });

		transform.Find("ToolBar").transform.Find("MoveButton").GetComponent<Image>().sprite = hudSprites[20];
		
		previewPanel = transform.Find("PreviewPanel").gameObject;
		previewRoot = transform.Find("PreviewPanel").transform.Find("PreviewNode").gameObject;

		structurePanel = transform.Find("TreePanel").transform.Find("ScrollView").transform.Find("Viewport").transform.Find("Content").gameObject;
		structureRoot = structurePanel.transform.Find("OrganItem").gameObject;
		structureRoot.GetComponent<SpeciesMenuOrganItem>().preview = previewRoot;

		selectedOrgan = structureRoot;
	}

	public void Update(){
		if (childPanel != null){
			return;
		}
		if(IsMouseInPreview() && Input.GetAxis("MouseLeft") != 0){
			Vector3 mousepos = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
			switch(editMode){
			case "Move":
				SpeciesMenuOrganItem item = selectedOrgan.GetComponent<SpeciesMenuOrganItem>();
				if(item.preview.GetComponent<RectTransform>().rect.Contains(mousepos - item.preview.GetComponent<RectTransform>().position)){
					item.preview.transform.position = mousepos;
				}
				break;
			}
		}
	}

	void OnMouseDrag(){
		
	}

	public void LoadSpecies(){
		species = new Species();
	}

	public void SaveSpecies(){

	}

	public void ClearAll(){
		
	}

	public void ReturnToTitle(){
		Transform mainCanvas = transform.parent;
		GameObject currentMenu = Instantiate(Resources.Load<GameObject>("Prefabs/UI/TitleMenu"));
		currentMenu.transform.SetParent(mainCanvas,false);
		Destroy(transform.gameObject);
	}

	public void ChangeEditMode(string mode){
		transform.Find("ToolBar").transform.Find(editMode+"Button").GetComponent<Image>().sprite = hudSprites[19];
		transform.Find("ToolBar").transform.Find(mode+"Button").GetComponent<Image>().sprite = hudSprites[20];
		editMode = mode;
	}

	private bool IsMouseInPreview(){
		Vector3 mousepos = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
		mousepos = mousepos*128 - previewPanel.GetComponent<RectTransform>().position*128;
		return previewPanel.GetComponent<RectTransform>().rect.Contains(mousepos);
	}

	private void CreateStructureNode(GameObject parent){

	}
}