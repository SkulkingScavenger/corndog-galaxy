using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tileset {
	public Sprite floorPanel = null;
	public Sprite backgroundPanels = null;
	public Sprite[] wallPanelsTop;
	public Sprite[] wallPanelsBottom;
	public Sprite foregroundPanel = null;
	public Sprite[] doorSprites = new Sprite[4];
	public Sprite consoleSpriteN = null;
	

	public Tileset(string name){
		floorPanel = Resources.Load<Sprite>("Sprites/_Environment/floorpanel_"+name);
		backgroundPanels = Resources.Load<Sprite>("Sprites/_Environment/env_background_"+name+"_00");
		wallPanelsTop = Resources.LoadAll<Sprite>("Sprites/_Environment/wall_"+name);
		wallPanelsBottom = Resources.LoadAll<Sprite>("Sprites/_Environment/wall_bottom_"+name);
		foregroundPanel = Resources.Load<Sprite>("Sprites/_Environment/foreground_"+name);
		doorSprites[1] = Resources.Load<Sprite>("Sprites/_Environment/door_n_"+name);
		doorSprites[3] = Resources.Load<Sprite>("Sprites/_Environment/door_s_"+name);
		consoleSpriteN = Resources.Load<Sprite>("Sprites/_Environment/env_console_"+name+"_00");
	}

	public void setWallSprite(GameObject obj,AreaSegmentWall wall){
		switch (wall.directionId){
		case 0:
			obj.transform.localScale = new Vector3(-1,1,1);
			obj.GetComponent<SpriteRenderer>().sprite = wallPanelsTop[wall.spriteId];
			obj.transform.Find("Foreground").gameObject.GetComponent<SpriteRenderer>().sprite = wallPanelsBottom[wall.spriteId];
			break;
		case 1:
			obj.GetComponent<SpriteRenderer>().sprite = backgroundPanels;
			break;
		case 2:
			obj.GetComponent<SpriteRenderer>().sprite = wallPanelsTop[wall.spriteId];
			obj.transform.Find("Foreground").gameObject.GetComponent<SpriteRenderer>().sprite = wallPanelsBottom[wall.spriteId];
			break;
		case 3:
			obj.GetComponent<SpriteRenderer>().sprite = foregroundPanel;
			break;
		}
	}

	public Sprite getInstallationSprite(string name, int dir){
		Sprite spr = null;
		if(name == "Navigation Console"){
			if(dir == 1){
				spr = consoleSpriteN;
			}
		}
		return spr;
	}
}