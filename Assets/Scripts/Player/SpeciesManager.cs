using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SpeciesManager : MonoBehaviour {
	public List<Species> speciesList = new List<Species>();
	public bool isInitialized = false;
	
	public static SpeciesManager Instance { get; private set; }

	public void Awake(){
		if(Instance != null && Instance != this){
			Destroy(gameObject);
		}
		Instance = this;
		DontDestroyOnLoad(transform.gameObject);

		if(OrganPrototypes.isInitialized){
			RegisterDefaultSpecies();
			isInitialized = true;
		}
	}

	void Update(){
		if(!isInitialized){
			if(OrganPrototypes.isInitialized){
				RegisterDefaultSpecies();
				isInitialized = true;
			}
		}
	}

	public Species GetSpeciesById(int index){
		return speciesList[index];
	}

	private void RegisterDefaultSpecies(){
		XmlProcessor xml = new XmlProcessor("Text/SpeciesList");
		while(!xml.IsDone()){
			xml.getNextNode();
			ReadSpeciesNode(xml);
		}
	}

	private void ReadSpeciesNode(XmlProcessor xml){
		string node;
		Species species = new Species();
		species.name = xml.getNextAttribute();

		node = xml.getNextNode();
		while(node != "/SpeciesTemplateNode"){
			switch(node){
			case "SegmentNode":
				species.physiology.segments.Add(ReadSegmentNode(xml));
				break;
			}
			node = xml.getNextNode();
		}
		speciesList.Add(species);
	}

	private CreatureBodySegment ReadSegmentNode(XmlProcessor xml){
		string node;
		CreatureBodySegment segment;

		string name = xml.getNextAttribute();
		int id = int.Parse(xml.getNextAttribute());
		int hitpoints = int.Parse(xml.getNextAttribute());
		float x = float.Parse(xml.getNextAttribute())/128f;
		float y = float.Parse(xml.getNextAttribute())/128f;
		float z = float.Parse(xml.getNextAttribute());
		Vector3 basePosition = new Vector3(x,y,z);

		segment = OrganPrototypes.Instance.LoadSegment(id);
		segment.hitpoints = hitpoints;
		segment.basePosition = basePosition;

		node = xml.getNextNode();
		while(node != "/SegmentNode"){
			switch(node){
			case "LimbNode":
				segment.limbs.Add(ReadLimbNode(xml));
				break;
			}
			node = xml.getNextNode();
		}
		return segment;
	}

	private CreatureLimb ReadLimbNode(XmlProcessor xml){
		string node;
		CreatureLimb limb;

		string name = xml.getNextAttribute();
		int id = int.Parse(xml.getNextAttribute());
		int hitpoints = int.Parse(xml.getNextAttribute());
		float x = float.Parse(xml.getNextAttribute())/128f;
		float y = float.Parse(xml.getNextAttribute())/128f;
		float z = float.Parse(xml.getNextAttribute());
		Vector3 basePosition = new Vector3(x,y,z);

		limb = OrganPrototypes.Instance.LoadLimb(id);
		limb.hitpoints = hitpoints;
		limb.basePosition = basePosition;

		node = xml.getNextNode();
		while(node != "/LimbNode"){
			switch(node){
			case "AppendageNode":
				limb.appendage = ReadAppendageNode(xml);
				break;
			}
			node = xml.getNextNode();
		}
		return limb;
	}

	private CreatureAppendage ReadAppendageNode(XmlProcessor xml){
		string node;
		CreatureAppendage appendage;

		string name = xml.getNextAttribute();
		int id = int.Parse(xml.getNextAttribute());
		int hitpoints = int.Parse(xml.getNextAttribute());

		appendage = OrganPrototypes.Instance.LoadAppendage(id);
		appendage.hitpoints = hitpoints;

		return appendage;
	}
}