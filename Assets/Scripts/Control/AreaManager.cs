using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AreaManager : NetworkBehaviour {
	public GameObject areaObject;
	public GameObject segmentObject;
	public GameObject[] installationPrefabs = new GameObject[4];
	public Sprite[] spaceSkySprites;
	public float segmentHeight = 768f/128f;
	public float segmentWidth = 1024f/128f;


	public static AreaManager Instance { get; private set; }

	void Awake (){
		if(Instance != null && Instance != this){
			Destroy(gameObject);
		}
		Instance = this;
		DontDestroyOnLoad(transform.gameObject);

		areaObject = Resources.Load<GameObject>("Prefabs/Environment/Area");
		segmentObject = Resources.Load<GameObject>("Prefabs/Environment/AreaSegment");
		installationPrefabs[0] = Resources.Load<GameObject>("Prefabs/Environment/Installation0");
		installationPrefabs[1] = Resources.Load<GameObject>("Prefabs/Environment/Installation1");
		installationPrefabs[2] = Resources.Load<GameObject>("Prefabs/Environment/Installation2");
		installationPrefabs[3] = Resources.Load<GameObject>("Prefabs/Environment/Installation3");
		spaceSkySprites = Resources.LoadAll<Sprite>("Sprites/_Environment/sky_space_sector_00");
	}

	public void Init(){
		Starship starship = new Starship();
		starship.currentSector = Galaxy.Instance.starmap.sectors[0,0];
		starship.coordinates.x = 150;
		starship.coordinates.y = 100;
		starship.name = "Kirrikash Virr";
		starship.classification = "Zakasi Starcrawler";
		starship.hitpoints = 4500;

		Area area = new Area(starship);
		AreaCorridor corridor;

		corridor = area.spawnCorridor(0,0,3);
		corridor.segments[1].installations[3] = new AreaSegmentInstallation(3,corridor.segments[1]);
		corridor.segments[1].installations[3].type = "door";
		corridor.segments[1].installations[3].name = "Internal Door";

		corridor = area.spawnCorridor(3,0,3);
		corridor.segments[0].installations[3] = new AreaSegmentInstallation(3,corridor.segments[1]);
		corridor.segments[0].installations[3].type = "door";
		corridor.segments[0].installations[3].name = "Internal Door";

		corridor = area.spawnCorridor(1,1,3);
		corridor.segments[0].installations[1] = new AreaSegmentInstallation(1,corridor.segments[0]);
		corridor.segments[0].installations[1].type = "door";
		corridor.segments[0].installations[1].name = "Internal Door";
		corridor.segments[1].installations[3] = new AreaSegmentInstallation(3,corridor.segments[1]);
		corridor.segments[1].installations[3].type = "door";
		corridor.segments[1].installations[3].name = "Internal Door";
		corridor.segments[2].installations[1] = new AreaSegmentInstallation(1,corridor.segments[2]);
		corridor.segments[2].installations[1].type = "door";
		corridor.segments[2].installations[1].name = "Internal Door";

		corridor = area.spawnCorridor(5,1,1);
		corridor.segments[0].installations[3] = new AreaSegmentInstallation(3,corridor.segments[0]);
		corridor.segments[0].installations[3].type = "door";
		corridor.segments[0].installations[3].name = "Internal Door";

		corridor = area.spawnCorridor(0,2,6);
		corridor.segments[2].installations[1] = new AreaSegmentInstallation(1,corridor.segments[2]);
		corridor.segments[2].installations[1].type = "door";
		corridor.segments[2].installations[1].name = "Internal Door";
		corridor.segments[5].installations[1] = new AreaSegmentInstallation(1,corridor.segments[5]);
		corridor.segments[5].installations[1].type = "door";
		corridor.segments[5].installations[1].name = "Internal Door";

		Control.Instance.currentArea = SpawnArea(area);
	}

	public GameObject SpawnArea(Area area){
		GameObject areaObj = GameObject.Instantiate<GameObject>(areaObject);
		if(area.isStarship){
			areaObj.transform.Find("Sky").gameObject.GetComponent<SpriteRenderer>().sprite = spaceSkySprites[area.starship.currentSector.skySpriteIndex];
		}
		for(int i=0;i<area.corridors.Count;i++){
			AreaCorridor corridor = area.corridors[i];
			Debug.Log(corridor.segments.Count);
			for(int j=0;j<corridor.segments.Count;j++){
				AreaSegment seg = corridor.segments[j];
				Vector3 segmentLocation = new Vector3(seg.x*segmentWidth,-1*seg.y*segmentHeight,0);
				GameObject areaSegment = GameObject.Instantiate<GameObject>(segmentObject);
				areaSegment.transform.SetParent(areaObj.transform);
				areaSegment.transform.localPosition = segmentLocation;
				for(int k=0;k<4;k++){
					GameObject installationObj;
					GameObject wallObj = areaSegment.transform.Find("Wall"+k.ToString()).gameObject;
					if(seg.walls[k] == null){
						GameObject.Destroy(wallObj);
					}else{
						area.tileset.setWallSprite(wallObj,seg.walls[k]);
						AreaSegmentInstallation installation = seg.installations[k];
						if(installation != null){
							installationObj = GameObject.Instantiate(installationPrefabs[k]);
							installationObj.transform.SetParent(wallObj.transform);
							installationObj.transform.localPosition = new Vector3(0,0,-1);
							switch(installation.type){
							case "door": 
								installationObj.AddComponent<DoorInstallation>();
								installationObj.GetComponent<DoorInstallation>().attachedWall = wallObj;
								installationObj.GetComponent<DoorInstallation>().installationName = installation.name;
								installationObj.GetComponent<SpriteRenderer>().sprite = area.tileset.doorSprites[k];
								installationObj.GetComponent<DoorInstallation>().directionId = k;
								break;
							case "interactive": 
								installationObj.AddComponent<InteractiveInstallation>();
								installationObj.GetComponent<InteractiveInstallation>().attachedWall = wallObj;
								installationObj.GetComponent<InteractiveInstallation>().installationName = installation.name;
								installationObj.GetComponent<InteractiveInstallation>().interfaceOverlayName = installation.overlayName;
								installationObj.GetComponent<SpriteRenderer>().sprite = area.tileset.getInstallationSprite(installation.name,k);
								break;
							}
						}
					}
				}
			}
		}
		return areaObj;
	}
}