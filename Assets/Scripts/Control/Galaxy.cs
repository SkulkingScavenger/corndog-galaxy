using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Galaxy : NetworkBehaviour {
	public Starmap starmap = null;
	public List<Starship> starships = new List<Starship>();

	public static Galaxy Instance { get; private set; }

	public void Awake(){
		if(Instance != null && Instance != this){
			Destroy(gameObject);
		}
		Instance = this;
		DontDestroyOnLoad(transform.gameObject);
	}

	public void Init(){
		starmap = new Starmap();

		Sector sector = starmap.sectors[0,0];
		sector.name = "Zakasi Space";

		Planet planet;
		planet = new Planet();
		planet.name = "Zakaas";
		planet.spriteIndex = 0;
		planet.starSystemSpriteIndex = 0;
		planet.shortDescription = "Zakaas II is the Homeworld of the Zakasi. Because the atmosphere is thin and the surface primarily liquid water, most sentient residents inhabit the hollow interiors of the sunken continents";
		planet.sector = sector;
		planet.x = 115;
		planet.y = 116;
		starmap.sectors[0,0].planets.Add(planet);

		planet = new Planet();
		planet.name = "Karkirraas";
		planet.spriteIndex = 1;
		planet.starSystemSpriteIndex = 1;
		planet.shortDescription = "Karkirraas V is a dark and toxic planet with oceans of sulphuric acid and an atmosphere dense enough to block out the light from its star. Notable for its uranium mines.";
		planet.sector = sector;
		planet.x = 245;
		planet.y = 83;
		starmap.sectors[0,0].planets.Add(planet);

		planet = new Planet();
		planet.name = "Arrikshaar";
		planet.spriteIndex = 2;
		planet.starSystemSpriteIndex = 2;
		planet.shortDescription = "Arrikshaar III is a dusty and desolate world. It is notable for its large quantities of sodium-chloride and other chloride salts covering the crust and dissolved within its oceans.";
		planet.sector = sector;
		planet.x = 256;
		planet.y = 167;
		starmap.sectors[0,0].planets.Add(planet);

		planet = new Planet();
		planet.name = "Varrazaar";
		planet.spriteIndex = 3;
		planet.starSystemSpriteIndex = 3;
		planet.shortDescription = "Varrazaar III is world of fog and sea. Its relatively mild climate and oceans of water make it a hospitable world for many carbon-based lifeforms.";
		planet.sector = sector;
		planet.x = 118;
		planet.y = 250;
		starmap.sectors[0,0].planets.Add(planet);

		SyncGalaxy();
	}

	public void SyncGalaxy(){
		Sector sec;
		if(isServer){
			RpcSpawnGalaxy(starmap.width, starmap.height);
		}
		for(int i=0;i<starmap.width;i++){
			for(int j=0;j<starmap.height;j++){
				sec = starmap.sectors[i,j];
				foreach(Planet p in sec.planets){
					if(isServer){
						RpcAddPlanet(p.name,p.spriteIndex,p.starSystemSpriteIndex,p.shortDescription,p.sector.x,p.sector.y,p.x,p.y);
					}
				}
			}
		}
	}

	[ClientRpc] public void RpcSpawnGalaxy(int gw, int gh){
		if (starmap != null){
			starmap = new Starmap(gw, gh);
		}
	}

	[ClientRpc] public void RpcAddPlanet(string name, int si, int sspi, string sdesc, int secx, int secy, int x, int y){
		Planet planet = new Planet();
		planet.name = name;
		planet.spriteIndex = si;
		planet.starSystemSpriteIndex = sspi;
		planet.shortDescription = sdesc;
		planet.sector = starmap.sectors[secx,secy];
		planet.x = x;
		planet.y = y;
		starmap.sectors[secx,secy].planets.Add(planet);
	}
}