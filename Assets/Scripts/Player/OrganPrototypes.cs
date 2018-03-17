using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*
Organ Presets
*/

public class OrganPrototypes : MonoBehaviour {
	public List<CreatureBodySegment> segmentPrototypes = new List<CreatureBodySegment>();
	public List<CreatureLimb> limbPrototypes = new List<CreatureLimb>();
	public List<CreatureAppendage> appendagePrototypes = new List<CreatureAppendage>();
	
	public static OrganPrototypes Instance { get; private set; }

	public void Awake(){
		if(Instance != null && Instance != this){
			Destroy(gameObject);
		}
		Instance = this;
		DontDestroyOnLoad(transform.gameObject);

		RegisterOrgans();
	}

	public CreatureBodySegment LoadSegment(int index){
		CreatureBodySegment segment = new CreatureBodySegment();
		CreatureBodySegment source = segmentPrototypes[index];

		segment.name = source.name;
		segment.segmentType = source.segmentType;
		segment.animationControllerName = source.animationControllerName;
		segment.animations = source.animations;
		segment.segmentOffsets = source.segmentOffsets;
		
		return segment;
	}

	public void AttachSegment(Creature root, CreatureBodySegment segment, Vector3 offset){
		Transform parentTransform;
		GameObject obj;
		parentTransform = root.display;
		obj = Instantiate(Resources.Load<GameObject>("Prefabs/Characters/LimbObject"));
		obj.transform.parent = parentTransform.transform;
		segment.basePosition = offset;
		obj.GetComponent<CreatureLimbObject>().rootSegment = segment;
		obj.GetComponent<SpriteRenderer>().transform.localPosition = segment.basePosition;
		obj.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(segment.animationControllerName);
		if(root.segments.Count > 0){
			CreatureBodySegment previous = root.segments[root.segments.Count -1];
			obj.transform.parent = previous.obj.transform;
			segment.previousSegment = previous;
			previous.nextSegment = segment;
			segment.basePosition = obj.transform.localPosition;
		}
		segment.obj = obj;
		segment.root = root;
		root.segments.Add(segment);
	}

	public CreatureLimb LoadLimb(int index){
		CreatureLimb organ = new CreatureLimb();
		CreatureLimb source = limbPrototypes[index];
		CombatAction act;

		organ.name = source.name;
		organ.limbType = source.limbType;
		organ.animationControllerName = source.animationControllerName;
		organ.appendageOffsets = source.appendageOffsets;
		organ.animations = source.animations;
		
		//create combat action for the limb
		for(int i=0;i<source.combatActions.Count;i++){
			act = new CombatAction();
			act.name = source.combatActions[i].name;
			act.range = source.combatActions[i].range;
			act.damage = source.combatActions[i].damage;
			act.windupDuration = source.combatActions[i].windupDuration;
			act.attackDuration = source.combatActions[i].attackDuration;
			act.cooldownDuration = source.combatActions[i].cooldownDuration;
			act.windupAnimation = source.combatActions[i].windupAnimation;
			act.attackAnimation = source.combatActions[i].attackAnimation;
			act.backswingAnimation = source.combatActions[i].backswingAnimation;
			organ.combatActions.Add(act);
		}
		return organ;
	}

	public void AttachLimb(CreatureBodySegment segment, CreatureLimb limb, Vector3 offset){
		limb.offset = offset;
		GameObject limbObject = Instantiate(Resources.Load<GameObject>("Prefabs/Characters/LimbObject"));
		limbObject.transform.parent = segment.obj.transform;
		limbObject.GetComponent<CreatureLimbObject>().root = limb;
		limbObject.GetComponent<SpriteRenderer>().transform.localPosition = offset;
		limbObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(limb.animationControllerName);
		limb.obj = limbObject;
		limb.root = segment.root;
		segment.limbs.Add(limb);
	}

	public CreatureAppendage LoadAppendage(int index){
		CreatureAppendage appendage = new CreatureAppendage();
		CreatureAppendage source = appendagePrototypes[index];
		CombatAction act;

		appendage.name = source.name;
		appendage.animationControllerName = source.animationControllerName;
		appendage.animations = source.animations;
		
		
		//create combat action for the limb
		for(int i=0;i<source.combatActions.Count;i++){
			act = new CombatAction();
			act.name = source.combatActions[i].name;
			act.range = source.combatActions[i].range;
			act.damage = source.combatActions[i].damage;
			act.windupDuration = source.combatActions[i].windupDuration;
			act.attackDuration = source.combatActions[i].attackDuration;
			act.cooldownDuration = source.combatActions[i].cooldownDuration;
			act.windupAnimation = source.combatActions[i].windupAnimation;
			act.attackAnimation = source.combatActions[i].attackAnimation;
			act.backswingAnimation = source.combatActions[i].backswingAnimation;
			appendage.combatActions.Add(act);
		}
		return appendage;
	}

	public void AttachAppendage(CreatureLimb organ, CreatureAppendage appendage){
		GameObject obj;
		Transform display = organ.root.display;
		obj = Instantiate(Resources.Load<GameObject>("Prefabs/Characters/LimbObject"));
		obj.transform.parent = organ.obj.transform;
		obj.GetComponent<SpriteRenderer>().transform.position = new Vector3(display.transform.position.x + organ.offset.x, display.transform.position.y + organ.offset.y, organ.offset.z);
		obj.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(appendage.animationControllerName);
		appendage.obj = obj;
		appendage.root = organ.root;
		appendage.offset = organ.appendageOffsets[0];
		organ.appendage = appendage;
	}

	private void RegisterOrgans(){
		TextAsset file = Resources.Load("Text/OrganList") as TextAsset;
		string text = file.text;
		string node;
		int i=0;
		while(i < text.Length){
			node = getNextNode(text, ref i);
			switch(node){
			case "OrganSegmentNode":
				readOrganSegmentNode(text,ref i);
				break;
			case "OrganLimbNode":
				readOrganLimbNode(text,ref i);
				break;
			case "OrganAppendageNode":
				readOrganAppendageNode(text,ref i);
				break;
			default:
				break;
			}
		}
	}

	private bool isWhiteSpace(char c){
		return c == ' ' || c == '	' || c == '\n' || c == '\r';
	}

	private string getNextNode(string text,ref int i){
		string buffer = "";
		while(text[i] != '<'){
			i++;
		}
		i++;
		while(!isWhiteSpace(text[i]) && text[i]!='>'){
			buffer = buffer + text[i];
			i++;
		}
		i++;
		return buffer;
	}

	private string getNextAttribute(string text,ref int i){
		//string key = "";
		string attributeValue = "";
		while(text[i]!='"' && text[i]!='/'){
			i++;
		}
		if(text[i] == '"'){
			i++;
			while(text[i]!='"'){
				attributeValue = attributeValue + text[i];
				i++;
			}
			i++;
		}else{
			i+=2;
		}
		return attributeValue;
	}

	private void readOrganSegmentNode(string text,ref int i){
		string node;
		CreatureBodySegment segment = new CreatureBodySegment();
		//Get Tags
		segment.name = getNextAttribute(text,ref i);
		segment.segmentType = getNextAttribute(text,ref i);
		segment.animationControllerName = getNextAttribute(text,ref i);
		//Get Subnodes
		node = getNextNode(text, ref i);
		while(node != "/OrganSegmentNode"){
			switch(node){
			case "OffsetNode":
				segment.segmentOffsets.Add(readOffsetNode(text, ref i));
				break;
			case "AnimationNode":
				segment.animations.Add(readAnimationNode(text, ref i));
				break;
			}
			node = getNextNode(text, ref i);
		}
		segmentPrototypes.Add(segment);
	}

	private void readOrganLimbNode(string text,ref int i){
		string node;
		CreatureLimb limb = new CreatureLimb();
		//Get Tags
		limb.name = getNextAttribute(text,ref i);
		limb.limbType = getNextAttribute(text,ref i);
		limb.animationControllerName = getNextAttribute(text,ref i);
		//Get Subnodes
		node = getNextNode(text, ref i);
		while(node != "/OrganLimbNode"){
			switch(node){
			case "OffsetNode":
				limb.appendageOffsets.Add(readOffsetNode(text, ref i));
				break;
			case "AnimationNode":
				limb.animations.Add(readAnimationNode(text, ref i));
				break;
			case "ActionNode":
				limb.combatActions.Add(readActionNode(text, ref i));
				break;
			}
			node = getNextNode(text, ref i);
		}
		limbPrototypes.Add(limb);
	}

	private void readOrganAppendageNode(string text,ref int i){
		string node;
		CreatureAppendage appendage = new CreatureAppendage();
		//Get Tags
		appendage.name = getNextAttribute(text,ref i);
		appendage.animationControllerName = getNextAttribute(text,ref i);
		//Get Subnodes
		node = getNextNode(text, ref i);
		while(node != "/OrganAppendageNode"){
			switch(node){
			case "AnimationNode":
				appendage.animations.Add(readAnimationNode(text, ref i));
				break;
			case "ActionNode":
				appendage.combatActions.Add(readActionNode(text, ref i));
				break;
			}
			node = getNextNode(text, ref i);
		}
		appendagePrototypes.Add(appendage);
	}

	private Vector3 readOffsetNode(string text, ref int i){
		float x;
		float y; 
		float z;
		x = float.Parse(getNextAttribute(text,ref i))/128;
		y = float.Parse(getNextAttribute(text,ref i))/128;
		z = float.Parse(getNextAttribute(text,ref i));
		return new Vector3(x,y,z);
	}

	private OrganAnimation readAnimationNode(string text, ref int i){
		string node;
		OrganAnimation anim = new OrganAnimation();
		anim.name = getNextAttribute(text,ref i);
		node = getNextNode(text, ref i);
		while(node != "/AnimationNode"){
			switch(node){
			case "TagNode":
				anim.tags.Add(readTagNode(text, ref i));
				break;
			}
			node = getNextNode(text, ref i);
		}
		return anim;
	}

	private string readTagNode(string text, ref int i){
		return getNextAttribute(text,ref i);
	}

	private CombatAction readActionNode(string text, ref int i){
		CombatAction act = new CombatAction();
		act.name = getNextAttribute(text,ref i);
		act.range = float.Parse(getNextAttribute(text,ref i));
		act.damage = int.Parse(getNextAttribute(text,ref i));
		act.windupDuration = float.Parse(getNextAttribute(text,ref i));
		act.attackDuration = float.Parse(getNextAttribute(text,ref i));
		act.cooldownDuration = float.Parse(getNextAttribute(text,ref i));
		act.windupAnimation = getNextAttribute(text,ref i);
		act.attackAnimation = getNextAttribute(text,ref i);
		act.backswingAnimation = getNextAttribute(text,ref i);
		return act;
	}
}