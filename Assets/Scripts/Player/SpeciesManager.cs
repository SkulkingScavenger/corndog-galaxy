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

		XmlAttribute attribute = xml.getNextAttribute();
		while(attribute.name != ""){
			switch(attribute.name){
				case "name":
					species.name = attribute.value;
					break;
				case "prototypeID":
					species.id = int.Parse(attribute.value);
					break;
			}
			attribute = xml.getNextAttribute();
		}

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

		string name = "default";
		int id = -1;
		int hitpoints = 0;
		float x = 0f;
		float y = 0f;
		float z = 0f;

		XmlAttribute attribute = xml.getNextAttribute();

		while(attribute.name != ""){
			switch(attribute.name){
				case "name":
					name = attribute.value;
					break;
				case "prototypeID":
					id = int.Parse(attribute.value);
					break;
				case "hitpoints":
					hitpoints = int.Parse(attribute.value);
					break;
				case "x":
					x = float.Parse(attribute.value)/128f;
					break;
				case "y":
					y = float.Parse(attribute.value)/128f;
					break;
				case "z":
					z = float.Parse(attribute.value)/128f;
					break;
			}
			attribute = xml.getNextAttribute();
		}

		segment = OrganPrototypes.Instance.LoadSegment(id);
		segment.hitpoints = hitpoints;
		segment.basePosition = new Vector3(x,y,z);

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
		
		string name = "default";
		int id = -1;
		int hitpoints = 0;
		float x = 0f;
		float y = 0f;
		float z = 0f;

		XmlAttribute attribute = xml.getNextAttribute();

		while(attribute.name != ""){
			switch(attribute.name){
				case "name":
					name = attribute.value;
					break;
				case "prototypeID":
					id = int.Parse(attribute.value);
					break;
				case "hitpoints":
					hitpoints = int.Parse(attribute.value);
					break;
				case "x":
					x = float.Parse(attribute.value)/128f;
					break;
				case "y":
					y = float.Parse(attribute.value)/128f;
					break;
				case "z":
					z = float.Parse(attribute.value)/128f;
					break;
			}
			attribute = xml.getNextAttribute();
		}

		limb = OrganPrototypes.Instance.LoadLimb(id);
		limb.hitpoints = hitpoints;
		limb.basePosition = new Vector3(x,y,z);

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

		string name = "default";
		int id = 0;
		int hitpoints = 0;
		float x = 0f;
		float y = 0f;
		float z = 0f;

		XmlAttribute attribute = xml.getNextAttribute();

		while(attribute.name != ""){
			switch(attribute.name){
				case "name":
					name = attribute.value;
					break;
				case "prototypeID":
					id = int.Parse(attribute.value);
					break;
				case "hitpoints":
					hitpoints = int.Parse(attribute.value);
					break;
				case "x":
					x = float.Parse(attribute.value)/128f;
					break;
				case "y":
					y = float.Parse(attribute.value)/128f;
					break;
				case "z":
					z = float.Parse(attribute.value)/128f;
					break;
			}
			attribute = xml.getNextAttribute();
		}

		appendage = OrganPrototypes.Instance.LoadAppendage(id);
		appendage.hitpoints = hitpoints;

		return appendage;
	}
}