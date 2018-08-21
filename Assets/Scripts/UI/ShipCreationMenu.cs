using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ShipCreationMenu : MonoBehaviour {
	GameObject mainCamera;

	string editMode = "Move";

	public string name = "default";

	public GameObject PalettePanel;
	public GameObject previewRoot;
	public GameObject childPanel = null;

	public GameObject previewPanel;
	public GameObject placementPreview;
	public GameObject selectedSegment;

	public float zoomLevel = 1f;


	Sprite[] hudSprites;

	public void Awake(){
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		hudSprites = Resources.LoadAll<Sprite>("Sprites/_UI/hud_elements_01");

		transform.Find("ControlPanel").transform.Find("LoadButton").GetComponent<Button>().onClick.AddListener(delegate { LoadShip(); });
		transform.Find("ControlPanel").transform.Find("SaveButton").GetComponent<Button>().onClick.AddListener(delegate { SaveShip(); });
		transform.Find("ControlPanel").transform.Find("BackButton").GetComponent<Button>().onClick.AddListener(delegate { ReturnToTitle(); });
		transform.Find("ControlPanel").transform.Find("RestartButton").GetComponent<Button>().onClick.AddListener(delegate { ClearAll(); });

		transform.Find("ToolBar").transform.Find("MoveButton").GetComponent<Button>().onClick.AddListener(delegate { ChangeEditMode("Move"); });
		transform.Find("ToolBar").transform.Find("RotateButton").GetComponent<Button>().onClick.AddListener(delegate { ChangeEditMode("Rotate"); });
		transform.Find("ToolBar").transform.Find("SelectButton").GetComponent<Button>().onClick.AddListener(delegate { ChangeEditMode("Select"); });
		transform.Find("ToolBar").transform.Find("AddButton").GetComponent<Button>().onClick.AddListener(delegate { ChangeEditMode("Add"); });

		transform.Find("ToolBar").transform.Find("MoveButton").GetComponent<Image>().sprite = hudSprites[20];
		
		previewPanel = transform.Find("PreviewPanel").gameObject;
		previewRoot = transform.Find("PreviewPanel").transform.Find("PreviewRoot").gameObject;
		placementPreview = previewRoot.transform.Find("PlacementPreview").gameObject;
		

		PalettePanel = transform.Find("PalettePanel").gameObject;
		ChangeEditMode("Add");
	}

	public void Update(){
		if (childPanel != null){
			return;
		}
		switch(editMode){
		case "Move":
			// ShipMenuOrganItem item = selectedOrgan.GetComponent<ShipMenuOrganItem>();
			// if(item.preview.GetComponent<RectTransform>().rect.Contains(mousepos - item.preview.GetComponent<RectTransform>().position)){
			// 	item.preview.transform.position = mousepos;
			// }
			break;
		case "Add":
			if(IsMouseInPreview()){
				placementPreview.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0.5f);
				float x = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition).x;
				float y = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition).y;
				float z = placementPreview.transform.position.z;
				y = Mathf.Round(y*zoomLevel*4)/(zoomLevel*4);
				x = Mathf.Round(x*zoomLevel*4)/(zoomLevel*4);
				placementPreview.transform.position = new Vector3(x,y,z);
			}else{
				placementPreview.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);
			}
			break;
		}
		if(IsMouseInPreview() && Input.GetAxis("MouseLeft") != 0){
			Vector3 mousepos = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
			switch(editMode){
			case "Move":
				// ShipMenuOrganItem item = selectedOrgan.GetComponent<ShipMenuOrganItem>();
				// if(item.preview.GetComponent<RectTransform>().rect.Contains(mousepos - item.preview.GetComponent<RectTransform>().position)){
				// 	item.preview.transform.position = mousepos;
				// }
				break;
			}
		}
		else{

		}
	}

	void OnMouseDrag(){
		
	}

	public void LoadShip(){

	}

	public void SaveShip(){
		XmlWriter xml = new XmlWriter();
		XmlNode node = new XmlNode();
		List<XmlNode> nodes = new List<XmlNode>();
		nodes.Add(node);
		xml.writeToFile("output.xml",nodes);

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

	private void CreateAreaSegment(GameObject parent){

	}
}