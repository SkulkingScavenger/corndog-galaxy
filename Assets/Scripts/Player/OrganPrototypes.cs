using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*
Organ Presets
*/

public class OrganPrototypes : MonoBehaviour {
	public List<CreatureLimb> limbPrototypes = new List<CreatureLimb>();
	public List<CreatureAppendage> appendagePrototypes = new List<CreatureAppendage>();
	
	public static OrganPrototypes Instance { get; private set; }

	public void Awake(){
		if(Instance != null && Instance != this){
			Destroy(gameObject);
		}
		Instance = this;
		DontDestroyOnLoad(transform.gameObject);

		RegisterLimbs();
		RegisterAppendages();

	}

	public CreatureLimb LoadLimb(int index){
		CreatureLimb organ = new CreatureLimb();
		CreatureLimb source = limbPrototypes[index];
		CombatAction act;

		organ.name = source.name;
		organ.limbType = source.limbType;
		organ.animationControllerName = source.animationControllerName;
		organ.appendageOffsets = source.appendageOffsets;
		
		//create combat action for the limb
		for(int i=0;i<source.combatActions.Count;i++){
			act = new CombatAction();
			act.name = source.combatActions[i].name;
			act.range = source.combatActions[i].range;
			act.damage = source.combatActions[i].damage;
			act.windupDuration = source.combatActions[i].windupDuration;
			act.attackDuration = source.combatActions[i].attackDuration;
			act.cooldownDuration = source.combatActions[i].cooldownDuration;
			act.idleAnimation = source.combatActions[i].idleAnimation;
			act.windupAnimation = source.combatActions[i].windupAnimation;
			act.attackAnimation = source.combatActions[i].attackAnimation;
			act.backswingAnimation = source.combatActions[i].backswingAnimation;
			organ.combatActions.Add(act);
		}
		return organ;
	}

	public void AttachLimb(Creature root, CreatureLimb organ, Vector3 offset){
		organ.root = root;
		organ.offset = offset;
		GameObject limbObject = Instantiate(Resources.Load<GameObject>("Prefabs/Characters/LimbObject"));
		limbObject.transform.parent = root.transform.Find("Display").transform;
		limbObject.GetComponent<CreatureLimbObject>().root = organ;
		limbObject.GetComponent<SpriteRenderer>().transform.position = new Vector3(root.display.transform.position.x + offset.x, root.display.transform.position.y + offset.y, offset.z);
		limbObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(organ.animationControllerName);
		organ.obj = limbObject;
		root.limbs.Add(organ);
	}

	public CreatureAppendage LoadAppendage(int index){
		CreatureAppendage appendage = new CreatureAppendage();
		CreatureAppendage source = appendagePrototypes[index];
		CombatAction act;

		appendage.name = source.name;
		appendage.animationControllerName = source.animationControllerName;
		
		
		//create combat action for the limb
		for(int i=0;i<source.combatActions.Count;i++){
			act = new CombatAction();
			act.name = source.combatActions[i].name;
			act.range = source.combatActions[i].range;
			act.damage = source.combatActions[i].damage;
			act.windupDuration = source.combatActions[i].windupDuration;
			act.attackDuration = source.combatActions[i].attackDuration;
			act.cooldownDuration = source.combatActions[i].cooldownDuration;
			act.idleAnimation = source.combatActions[i].idleAnimation;
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

	private void RegisterLimbs(){
		CreatureLimb organ;
		CombatAction act;

		//Skirriashi Major Tentacle R
		organ = new CreatureLimb();
		organ.name = "Right Major Tentacle";
		organ.limbType = "tentacle";
		organ.animationControllerName = "Animation/Controllers/major_tentacle_r";
		organ.appendageOffsets.Add(new Vector3(0.515625f,-0.0390625f,0f));
		organ.appendageOffsets.Add(new Vector3(0.234375f,0.1484375f,0f));
		organ.appendageOffsets.Add(new Vector3(0.125f,-0.1171875f,0f));
		organ.appendageOffsets.Add(new Vector3(0.5703125f,-0.328125f,0f));


		act = new CombatAction();
		act.name = "tentacle lash";
		act.range = 2f;
		act.damage = 4;
		act.windupDuration = 0.25f;
		act.attackDuration = 0.25f;
		act.cooldownDuration = 0.25f;
		act.idleAnimation = "major_tentacle_r_idle";
		act.windupAnimation = "major_tentacle_r_windup";
		act.attackAnimation = "major_tentacle_r_attack";
		act.backswingAnimation = "";
		organ.combatActions.Add(act);

		limbPrototypes.Add(organ);

		//Skirriashi Major Tentacle L
		organ = new CreatureLimb();
		organ.name = "Left Major Tentacle";
		organ.limbType = "tentacle";
		organ.animationControllerName = "Animation/Controllers/major_tentacle_l";
		organ.appendageOffsets.Add(new Vector3(0.5703125f,-0.0078125f,0f));
		organ.appendageOffsets.Add(new Vector3(0.390625f,0.15625f,0f));
		organ.appendageOffsets.Add(new Vector3(0.25f,-0.078125f,0f));
		organ.appendageOffsets.Add(new Vector3(0.5859375f,-0.3984375f,0f));


		act = new CombatAction();
		act.name = "tentacle lash";
		act.range = 2f;
		act.damage = 4;
		act.windupDuration = 0.25f;
		act.attackDuration = 0.25f;
		act.cooldownDuration = 0.25f;
		act.idleAnimation = "major_tentacle_l_idle";
		act.windupAnimation = "major_tentacle_l_windup";
		act.attackAnimation = "major_tentacle_l_attack";
		act.backswingAnimation = "";
		organ.combatActions.Add(act);

		limbPrototypes.Add(organ);
	}

	private void RegisterAppendages(){
		CreatureAppendage appendage;
		CombatAction act;

		//Right Major Tentacle Claw
		appendage = new CreatureAppendage();

		appendage.name = "Right Major Tentacle Claw";
		appendage.animationControllerName = "Animation/Controllers/major_tentacle_claw_r";

		act = new CombatAction();
		act.name = "Claw Snap";
		act.range = 2f;
		act.damage = 4;
		act.windupDuration = 0.25f;
		act.attackDuration = 0.25f;
		act.cooldownDuration = 0.25f;
		act.idleAnimation = "major_tentacle_claw_r_idle";
		act.windupAnimation = "major_tentacle_claw_r_windup";
		act.attackAnimation = "major_tentacle_claw_r_attack";
		appendage.combatActions.Add(act);

		appendagePrototypes.Add(appendage);

		//Left Major Tentacle Claw
		appendage = new CreatureAppendage();
		
		appendage.name = "Left Major Tentacle Claw";
		appendage.animationControllerName = "Animation/Controllers/major_tentacle_claw_l";

		act = new CombatAction();
		act.name = "Claw Snap";
		act.range = 2f;
		act.damage = 4;
		act.windupDuration = 0.25f;
		act.attackDuration = 0.25f;
		act.cooldownDuration = 0.25f;
		act.idleAnimation = "major_tentacle_claw_l_idle";
		act.windupAnimation = "major_tentacle_claw_l_windup";
		act.attackAnimation = "major_tentacle_claw_l_attack";
		appendage.combatActions.Add(act);

		appendagePrototypes.Add(appendage);

	}

}