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
		Animator anim = obj.GetComponent<Animator>();
		if(act.windupDuration > 0 && phase < 1){
			isWindingUp = true;
			phase = 1;
			anim.Play(act.windupAnimation);
			setAnimationSpeed(anim,act.windupDuration);
			yield return new WaitForSeconds(act.windupDuration);
			root.StartCoroutine(Attack(act));
		}else if(act.attackDuration > 0 && phase < 2){
			isAttacking = true;
			phase = 2;
			anim.Play(act.attackAnimation);
			setAnimationSpeed(anim,act.attackDuration);
			yield return new WaitForSeconds(act.attackDuration);
			root.StartCoroutine(Attack(act));
		}else if(act.backswingDuration > 0 && phase < 3){
			phase = 3;
			isBackswinging = true;
			anim.Play(act.backswingAnimation);
			setAnimationSpeed(anim,act.backswingDuration);
			yield return new WaitForSeconds(act.backswingDuration);
			root.StartCoroutine(Attack(act));
		}else{
			phase = 0;
			anim.Play(act.idleAnimation);
			setAnimationSpeed(anim,-1);
			yield return new WaitForSeconds(act.cooldownDuration);
			isReady = true;
		}
	}

	public void setAnimationSpeed(Animator anim, float duration){
		if(duration > 0 && anim.GetCurrentAnimatorStateInfo(0).length > 0){
			anim.speed = 1 / duration;
		}else{
			anim.speed = 1;
		}
	}

	public void AnimationEventCallback(int id){
		if(appendage != null){
			appendage.offset = appendageOffsets[id];
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