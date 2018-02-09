using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*
Organ Presets
*/

public  class OrganPrototypes : MonoBehaviour {
	public List<CreatureLimb> limbPrototypes = new List<CreatureLimb>();
	
	public static OrganPrototypes Instance { get; private set; }

	public void Awake(){
		if(Instance != null && Instance != this){
			Destroy(gameObject);
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);


		CreatureLimb organ;
		CombatAction act;

		//Skirriashi Major Tentacle R
		organ = new CreatureLimb();
		organ.name = "Right Major Tentacle";
		organ.limbType = "tentacle";
		organ.animationControllerName = "Animation/Controllers/major_tentacle_r";
		organ.appendageOffsets.Add(new Vector3(0.25f,0f,0f));
		organ.appendageOffsets.Add(new Vector3(-0.0312f,0.1876f,0f));
		organ.appendageOffsets.Add(new Vector3(-0.1406f,-0.0781f,0f));
		organ.appendageOffsets.Add(new Vector3(0.30465f,-0.28902f,0f));


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

	}

	public void LoadLimb(CreatureLimb organ, int index){
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

}