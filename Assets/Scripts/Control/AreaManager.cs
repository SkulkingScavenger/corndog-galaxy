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

	public Area currentArea;
	public GameObject currentAreaObj;

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
		corridor.segments[0].installations[1] = new AreaSegmentInstallation(1,corridor.segments[0]);
		corridor.segments[0].installations[1].type = "interactive";
		corridor.segments[0].installations[1].name = "Navigation Console";
		corridor.segments[0].installations[1].overlayName = "NavigationConsoleOverlay";
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
		corridor.segments[0].installations[1] = new AreaSegmentInstallation(1,corridor.segments[0]);
		corridor.segments[0].installations[1].type = "interactive";
		corridor.segments[0].installations[1].name = "Navigation Console";
		corridor.segments[0].installations[1].overlayName = "NavigationConsoleOverlay";
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

		currentArea = area;
		currentAreaObj = GenerateAreaObject();
	}

	public GameObject GenerateAreaObject(){
		Area area = currentArea;
		GameObject areaObj = GameObject.Instantiate<GameObject>(areaObject);
		if(area.isStarship){
			areaObj.transform.Find("Sky").gameObject.GetComponent<SpriteRenderer>().sprite = spaceSkySprites[area.starship.currentSector.skySpriteIndex];
		}
		for(int i=0;i<area.corridors.Count;i++){
			AreaCorridor corridor = area.corridors[i];
			for(int j=0;j<corridor.segments.Count;j++){
				AreaSegment seg = corridor.segments[j];
				Vector3 segmentLocation = new Vector3(seg.x*segmentWidth,-1*seg.y*segmentHeight,0);
				GameObject areaSegment = GameObject.Instantiate<GameObject>(segmentObject);
				seg.netId = areaSegment.GetComponent<NetworkIdentity>();
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
							installation.netId = installationObj.GetComponent<NetworkIdentity>();
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

	public void SpawnAreaObject(){
		GameObject areaObj = currentAreaObj;
		NetworkServer.Spawn(areaObj);
		for(int i=0;i<currentArea.corridors.Count;i++){
			AreaCorridor corridor = currentArea.corridors[i];
			for(int j=0;j<corridor.segments.Count;j++){
				AreaSegment seg = corridor.segments[j];
				Vector3 segmentLocation = new Vector3(seg.x*segmentWidth,-1*seg.y*segmentHeight,0);
				NetworkServer.Spawn(seg.netId.gameObject);
				RpcSetupAreaSegment(areaObj.GetComponent<NetworkIdentity>(), seg.netId, corridor.index, seg.index, segmentLocation);
				for(int k=0;k<4;k++){
					if(seg.walls[k] != null){
						AreaSegmentInstallation installation = seg.installations[k];
						if(installation != null){
							NetworkServer.Spawn(installation.netId.gameObject);
							RpcSetupAreaInstallation(seg.netId, installation.netId, corridor.index, seg.index, k);
						}
					}
				}
			}
		}
	}

	[ClientRpc] private void RpcSetupAreaSegment(NetworkIdentity areaNetId, NetworkIdentity segNetId, int corridorId, int segmentId, Vector3 segmentLocation){
		GameObject areaObj = areaNetId.gameObject;
		GameObject areaSegment = segNetId.gameObject;
		AreaSegment seg = currentArea.corridors[corridorId].segments[segmentId];

		areaSegment.transform.SetParent(areaObj.transform);
		areaSegment.transform.localPosition = segmentLocation;
		for(int i=0;i<4;i++){
			GameObject wallObj = areaSegment.transform.Find("Wall"+i.ToString()).gameObject;
			if(seg.walls[i] == null){
				GameObject.Destroy(wallObj);
			}else{
				currentArea.tileset.setWallSprite(wallObj,seg.walls[i]);
			}
		}
	}

	[ClientRpc] private void RpcSetupAreaInstallation(NetworkIdentity segNetId, NetworkIdentity installationNetId, int corridorId, int segmentId, int dir){
		GameObject wallObj = segNetId.gameObject.transform.Find("Wall"+dir.ToString()).gameObject;
		GameObject installationObj = installationNetId.gameObject;
		AreaSegmentInstallation installation = currentArea.corridors[corridorId].segments[segmentId].installations[dir];

		installationObj.transform.SetParent(wallObj.transform);
		installationObj.transform.localPosition = new Vector3(0,0,-1);
		switch(installation.type){
		case "door": 
			installationObj.AddComponent<DoorInstallation>();
			installationObj.GetComponent<DoorInstallation>().attachedWall = wallObj;
			installationObj.GetComponent<DoorInstallation>().installationName = installation.name;
			installationObj.GetComponent<SpriteRenderer>().sprite = currentArea.tileset.doorSprites[dir];
			installationObj.GetComponent<DoorInstallation>().directionId = dir;
			break;
		case "interactive": 
			installationObj.AddComponent<InteractiveInstallation>();
			installationObj.GetComponent<InteractiveInstallation>().attachedWall = wallObj;
			installationObj.GetComponent<InteractiveInstallation>().installationName = installation.name;
			installationObj.GetComponent<InteractiveInstallation>().interfaceOverlayName = installation.overlayName;
			installationObj.GetComponent<SpriteRenderer>().sprite = currentArea.tileset.getInstallationSprite(installation.name,dir);
			break;
		}
	}

	public void SpawnArea(){
		Area area = currentArea;
		RpcSpawnArea(area.starship.index);
		foreach(AreaCorridor corridor in area.corridors){
			RpcSpawnCorridor(corridor.x,corridor.y,corridor.segments.Count);
			foreach(AreaSegment segment in corridor.segments){
				RpcSpawnSegment(corridor.index, segment.index, segment.netId);
				for(int i=0;i<4;i++){
					AreaSegmentInstallation inst = segment.installations[i];
					if(inst != null){
						RpcSpawnInstallation(corridor.index, segment.index, i, inst.type, inst.name, inst.netId);
					}
					AreaSegmentWall wall = segment.walls[i];
					if(wall != null){
						//RpcSpawnWall(corridor.index, segment.index, i);
					}
				}
			}
		}
	}

	[ClientRpc] private void RpcSpawnArea(int starshipIndex){
		Starship starship = Galaxy.Instance.starships[starshipIndex];
		currentArea = new Area(starship);
	}

	[ClientRpc] private void RpcSpawnCorridor(int x, int y, int length){
		currentArea.spawnCorridor(x,y,length);
	}

	[ClientRpc] private void RpcSpawnSegment(int corridorId, int segmentId, NetworkIdentity id){
		currentArea.corridors[corridorId].segments[segmentId].netId = id;
	}

	[ClientRpc] private void RpcSpawnInstallation(int corridorId, int segmentId, int dir, string type, string name, NetworkIdentity id){
		AreaCorridor corridor = currentArea.corridors[corridorId];
		AreaSegment segment = corridor.segments[segmentId];
		segment.installations[dir] = new AreaSegmentInstallation(dir,segment);
		segment.installations[dir].type = type;
		segment.installations[dir].name = name;
		segment.installations[dir].netId = id;
	}
}