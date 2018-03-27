using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationConsoleOverlay : InterfaceOverlay{
	public GameObject console;
	public GameObject starmap;
	public GameObject starshipIndicator;
	public List<GameObject> starSystemIndicators = new List<GameObject>();
	private int selectionCoordinateX = 184;
	private int selectionCoordinateY = 160;
	private int shipCoordinateX;
	private int shipCoordinateY;
	private Sprite[] planetSprites;
	private Sprite[] starSprites;
	private Sprite[] sectorSprites;
	public Sector currentSector;
	public Starship starship;
	public Planet selectedPlanet = null;

	

	public override void InterfaceOverlayAwake(){
		planetSprites = Resources.LoadAll<Sprite>("Sprites/_InterfaceOverlays/planet_sprites");
		starSprites = Resources.LoadAll<Sprite>("Sprites/_InterfaceOverlays/star_systems");
		sectorSprites = Resources.LoadAll<Sprite>("Sprites/_InterfaceOverlays/sectors");
		console = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/InterfaceOverlays/NavigationConsoleUI"), Vector3.zero, Quaternion.identity, InterfaceOverlayUI.transform);
		
		currentSector = Control.Instance.galaxy.sectors[0,0];
		starmap = transform.Find("StarMap").gameObject;
		starmap.GetComponent<SpriteRenderer>().sprite = sectorSprites[currentSector.spriteIndex];
		starship = installation.transform.root.gameObject.GetComponent<Area>().starship;

		GameObject starSystemIndicator = console.transform.Find("SectorDisplay").transform.Find("StarSystemIndicator").gameObject;
		GameObject temp;
		foreach(Planet planet in currentSector.planets){
			float x = (0 - 184f) + (float)planet.x;
			float y = (0 + 160f) - (float)planet.y;
			temp = GameObject.Instantiate<GameObject>(starSystemIndicator, Vector3.zero, Quaternion.identity, console.transform.Find("SectorDisplay").transform);
			temp.GetComponent<Image>().sprite = starSprites[planet.starSystemSpriteIndex];
			temp.transform.localPosition = new Vector3(x,y,-1f);
			starSystemIndicators.Add(temp);
		}
		Destroy(starSystemIndicator);

		starshipIndicator = transform.Find("StarshipIndicator").gameObject;
		console.transform.Find("ShipDisplay").transform.Find("Name").GetComponent<Text>().text = starship.name;
		console.transform.Find("ShipDisplay").transform.Find("Classification").GetComponent<Text>().text = starship.classification;

	}

	public override void InterfaceOverlayUpdate(){
		int starmapWidth = 368;
		int starmapHeight = 320;
		Vector2 mousepos = new Vector2(control.commandX,control.commandY);
		Rect mapBounds = new Rect(
			starmap.transform.position.x - starmapWidth/256f,
			starmap.transform.position.y - starmapHeight/256f,
			starmapWidth/128f,
			starmapHeight/128f
		);
		if(mapBounds.Contains(mousepos)){
			if(control.moveCommand){
				starship.destinationCoordinateX = (int)Mathf.Round(((mousepos.x-mapBounds.x)/mapBounds.width)*starmapWidth);
				starship.destinationCoordinateY = starmapHeight - (int)Mathf.Round(((mousepos.y-mapBounds.y)/mapBounds.height)*starmapHeight);
			}
			if(control.attackCommand){
				selectionCoordinateX = (int)Mathf.Round(((mousepos.x-mapBounds.x)/mapBounds.width)*starmapWidth);
				selectionCoordinateY = starmapHeight - (int)Mathf.Round(((mousepos.y-mapBounds.y)/mapBounds.height)*starmapHeight);
				selectedPlanet = GetPlanetNearCoordinates(selectionCoordinateX,selectionCoordinateY);
			}
		}
		if(selectedPlanet != null){
			console.transform.Find("PlanetDisplay").GetComponent<Image>().sprite = planetSprites[selectedPlanet.spriteIndex];
			console.transform.Find("PlanetDisplay").transform.Find("Name").GetComponent<Text>().text = selectedPlanet.name;
			console.transform.Find("PlanetDisplay").transform.Find("Description").GetComponent<Text>().text = selectedPlanet.shortDescription;
			console.transform.Find("PlanetDisplay").transform.Find("Coordinates").GetComponent<Text>().text = selectedPlanet.GetTextCoordinates();
			console.transform.Find("PlanetDisplay").transform.Find("SetCourseButton").GetComponent<Button>().interactable = false;//true;
			console.transform.Find("PlanetDisplay").transform.Find("PlanetDetailsButton").GetComponent<Button>().interactable = false;//true;
			selectionCoordinateX = selectedPlanet.x;
			selectionCoordinateY = selectedPlanet.y;
		}else{
			console.transform.Find("PlanetDisplay").GetComponent<Image>().sprite = sectorSprites[currentSector.spriteIndex];
			console.transform.Find("PlanetDisplay").transform.Find("Name").GetComponent<Text>().text = "Sector 0";
			console.transform.Find("PlanetDisplay").transform.Find("Description").GetComponent<Text>().text = "";
			console.transform.Find("PlanetDisplay").transform.Find("Coordinates").GetComponent<Text>().text = "Galactic Coordinates: 0,0";
			console.transform.Find("PlanetDisplay").transform.Find("SetCourseButton").GetComponent<Button>().interactable = false;
			console.transform.Find("PlanetDisplay").transform.Find("PlanetDetailsButton").GetComponent<Button>().interactable = false;
		}
		float starshipZ = -1f;
		if(currentSector != starship.currentSector){starshipZ = 4f;}
		float starshipX = mapBounds.x + starship.coordinateX/128f;
		float starshipY = mapBounds.y + (starmapHeight-starship.coordinateY)/128f;
		starshipIndicator.transform.position = new Vector3(starshipX,starshipY,starshipZ);
		transform.Find("indicatorX").position = new Vector3(mapBounds.x + selectionCoordinateX/128f, mapBounds.y,-1);
		transform.Find("indicatorY").position = new Vector3(mapBounds.x, mapBounds.y + (starmapHeight - selectionCoordinateY)/128f,-1);
		console.transform.Find("DisplayCoordinates").transform.Find("SectorCoordinates").GetComponent<Text>().text = "Sector: " + selectionCoordinateX.ToString() +", "+selectionCoordinateY.ToString();
		console.transform.Find("DestinationCoordinates").transform.Find("SectorCoordinates").GetComponent<Text>().text = "Sector: " + starship.destinationCoordinateX.ToString() +", "+starship.destinationCoordinateY.ToString();
		console.transform.Find("ShipDisplay").transform.Find("Coordinates").transform.Find("GalaxyCoordinates").GetComponent<Text>().text = starship.currentSector.GetTextCoordinates();
		console.transform.Find("ShipDisplay").transform.Find("Coordinates").transform.Find("SectorCoordinates").GetComponent<Text>().text = "Sector: " + starship.coordinateX.ToString() +", "+starship.coordinateY.ToString();
	}

	public override void InterfaceOverlayOnDestroy(){

	}

	public Planet GetPlanetNearCoordinates(int x, int y){
		foreach(Planet planet in currentSector.planets){
			if(GetPointDistance(x,y, planet.x,planet.y) <= 16){
				return planet;
			}
		}
		return null;
	}

	public float GetPointDistance(int x1, int y1, int x2, int y2){
		float dx = (float)Mathf.Abs((float)x1-(float)x2);
		float dy = (float)Mathf.Abs((float)y1-(float)y2);
		return Mathf.Sqrt(dx*dx + dy*dy);
	}

	public void SetCourseForPlanet(){
		starship.destinationCoordinateX = selectedPlanet.x;
		starship.destinationCoordinateY = selectedPlanet.y;
	}
}