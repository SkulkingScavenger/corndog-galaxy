using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatureOrgan{
	public Creature root;
	public Vector3 offset;
	public string name;
	public CreatureOrgan rootOrgan;
	public string type;
	public string organLayer;
	public int hitpoints;
	public GameObject obj;
	
}


public class Chemical{

}

public class Material : Chemical {

}

public class OrganTissue : Material{

}