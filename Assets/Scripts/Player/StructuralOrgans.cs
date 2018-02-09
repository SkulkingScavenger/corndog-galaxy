using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatureBodySegment : CreatureOrgan{
	public List<CreatureLimb> limbs = new List<CreatureLimb>();
	public CreatureBodySegment previousSegment = null;
	public CreatureBodySegment nextSegment = null;

}



public class CreatureAppendage : CreatureOrgan{
	public List<CreatureLimb> limbs = new List<CreatureLimb>();
	public List<CombatAction> combatActions = new List<CombatAction>();
	public int prototypeIndex;
	public string animationControllerName = "";

	public void PlayAnimation(CombatAction act, int animPhase){
		string animationName = act.idleAnimation;
		float phaseDuration = -1;
		switch(animPhase){
			case 0: animationName = combatActions[0].idleAnimation; phaseDuration = -1; break;
			case 1: animationName = combatActions[0].windupAnimation; phaseDuration = act.windupDuration; break;
			case 2: animationName = combatActions[0].attackAnimation; phaseDuration = act.attackDuration; break;
			case 3: animationName = combatActions[0].backswingAnimation; phaseDuration = act.backswingDuration; break;
		}
		Animator anim = obj.GetComponent<Animator>();
		anim.Play(animationName);
		if(phaseDuration > 0 && anim.GetCurrentAnimatorStateInfo(0).length > 0){
			anim.speed = 1 / phaseDuration;
		}else{
			anim.speed = 1;
		}
	}
}


public class CreatureLimb : CreatureOrgan{
	public string limbType = "";
	public bool isParalyzed = false;
	public bool isReady = true;
	public bool isWindingUp = false;
	public bool isAttacking = false;
	public bool isBackswinging = false;
	public short phase = 0;
	public string animationControllerName = "";
	public List<CombatAction> combatActions = new List<CombatAction>();
	public int prototypeIndex;
	public CreatureAppendage appendage;
	public List<Vector3> appendageOffsets = new List<Vector3>();

	public CreatureLimb(){
		type = "limb";
		organLayer = "structural";
	}

	public IEnumerator Attack(CombatAction act){
		isReady = false;
		if(act.windupDuration > 0 && phase < 1){
			isWindingUp = true;
			phase = 1;
			PlayAnimation(act,phase);
			yield return new WaitForSeconds(act.windupDuration);
			root.StartCoroutine(Attack(act));
		}else if(act.attackDuration > 0 && phase < 2){
			isAttacking = true;
			phase = 2;
			PlayAnimation(act,phase);
			yield return new WaitForSeconds(act.attackDuration);
			root.StartCoroutine(Attack(act));
		}else if(act.backswingDuration > 0 && phase < 3){
			phase = 3;
			isBackswinging = true;
			PlayAnimation(act,phase);
			yield return new WaitForSeconds(act.backswingDuration);
			root.StartCoroutine(Attack(act));
		}else{
			phase = 0;
			PlayAnimation(act,phase);
			yield return new WaitForSeconds(act.cooldownDuration);
			isReady = true;
		}
	}

	public void PlayAnimation(CombatAction act, int animPhase){
		string animationName = act.idleAnimation;
		float phaseDuration = -1;
		switch(animPhase){
			case 0: animationName = act.idleAnimation; phaseDuration = -1; break;
			case 1: animationName = act.windupAnimation; phaseDuration = act.windupDuration; break;
			case 2: animationName = act.attackAnimation; phaseDuration = act.attackDuration; break;
			case 3: animationName = act.backswingAnimation; phaseDuration = act.backswingDuration; break;
		}
		Animator anim = obj.GetComponent<Animator>();
		anim.Play(animationName);
		if(phaseDuration > 0 && anim.GetCurrentAnimatorStateInfo(0).length > 0){
			anim.speed = 1 / phaseDuration;
		}else{
			anim.speed = 1;
		}

		if(appendage != null){
			appendage.PlayAnimation(act,animPhase);
		}
	}

	public void AnimationEventCallback(int id){
		if(appendage != null){
			appendage.offset = appendageOffsets[id];
			appendage.obj.transform.localPosition = appendage.offset;
		}
	}
}


public static class LimbAttacks {
	public static void attack(int id){
		switch(id){
			case 0: 
				basicAttack();
			break;
		}
	}

	private static void basicAttack(){

	}
}