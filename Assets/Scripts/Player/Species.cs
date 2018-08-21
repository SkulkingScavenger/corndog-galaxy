using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Species {
	public string name = "";
	public int id = -1;
	public SpeciesPhysiology physiology = new SpeciesPhysiology();
}

public class SpeciesPhysiology {
	public List<CreatureBodySegment> segments = new List<CreatureBodySegment>();
}