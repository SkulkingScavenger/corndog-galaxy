using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;


public class CreatureBodySegment : CreatureOrgan{
	public List<CreatureLimb> limbs = new List<CreatureLimb>();
	public string segmentType = "";
	public Vector3 basePosition = new Vector3(0f,0f,0f);
	public List<Vector3> segmentOffsets = new List<Vector3>();
	public CreatureBodySegment previousSegment = null;
	public CreatureBodySegment nextSegment = null;


	public void AnimationEventCallback(int id){
		obj.transform.localPosition = segmentOffsets[id] + basePosition;
	}
}



public class CreatureAppendage : CreatureOrgan{
	public List<CreatureLimb> limbs = new List<CreatureLimb>();
	public List<CombatAction> combatActions = new List<CombatAction>();

	public void PlayAttackAnimation(CombatAction act, int animPhase){
		string animationName = GetAnimationByTag("idle");
		float phaseDuration = -1;
		switch(animPhase){
			case 0: animationName = GetAnimationByTag("idle"); phaseDuration = -1; break;
			case 1: animationName = GetAnimationByTag("windup"); phaseDuration = act.windupDuration; break;
			case 2: animationName = GetAnimationByTag("attack"); phaseDuration = act.attackDuration; break;
			case 3: animationName = GetAnimationByTag("backswing"); phaseDuration = act.backswingDuration; break;
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
	public int currentOffsetId = 0;
	public short phase = 0;
	public List<CombatAction> combatActions = new List<CombatAction>();
	public CreatureAppendage appendage;
	public Vector3 basePosition;
	public List<Vector3> appendageOffsets = new List<Vector3>();

	public CreatureLimb(){
		type = "limb";
		organLayer = "structural";
	}

	public bool CanAttack(){
		return (hitpoints > 0 && !isParalyzed && isReady);
	}

	public bool IsExecutingAttack(){
		return (isWindingUp || isAttacking || isBackswinging);
	}

	public IEnumerator Attack(CombatAction act){
		isReady = false;
		if(act.windupDuration > 0 && phase < 1){
			isWindingUp = true;
			phase = 1;
			PlayAttackAnimation(act,phase);
			yield return new WaitForSeconds(act.windupDuration);
			root.StartCoroutine(Attack(act));
		}else if(act.attackDuration > 0 && phase < 2){
			isAttacking = true;
			phase = 2;
			PlayAttackAnimation(act,phase);
			if(root.isServer){
				SpawnProjectile();
			}else{
				//CmdAttack();
			}
			
			yield return new WaitForSeconds(act.attackDuration);
			
			root.StartCoroutine(Attack(act));
		}else if(act.backswingDuration > 0 && phase < 3){
			phase = 3;
			isBackswinging = true;
			PlayAttackAnimation(act,phase);
			yield return new WaitForSeconds(act.backswingDuration);
			root.StartCoroutine(Attack(act));
		}else{
			phase = 0;
			PlayAttackAnimation(act,phase);
			yield return new WaitForSeconds(act.cooldownDuration);
			isReady = true;
		}
	}

	[Command] public void CmdAttack(){
		SpawnProjectile();
	}

	public void SpawnProjectile(){
		GameObject bullet = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Characters/Projectile"), root.transform.position, Quaternion.identity);
		
		//adjust display
		GameObject display = bullet.transform.Find("Display").gameObject;
		//display.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Thing");

		//adjust position
		int dir = (int)root.transform.localScale.x;
		display.transform.position = obj.transform.position + appendageOffsets[currentOffsetId] + new Vector3(dir*0.5f,0,0);
		display.transform.localScale = root.transform.localScale;

		// make the bullet move away in front of the player
		bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(dir*2f,0);
		NetworkServer.Spawn(bullet);

		// make bullet disappear after 2 seconds
		GameObject.Destroy(bullet, 4.0f); 
	}

	public void PlayAttackAnimation(CombatAction act, int animPhase){
		string animationName = GetAnimationByTag("idle");
		float phaseDuration = -1;
		switch(animPhase){
			case 0: animationName = GetAnimationByTag("idle"); phaseDuration = -1; break;
			case 1: animationName = GetAnimationByTag("windup"); phaseDuration = act.windupDuration; break;
			case 2: animationName = GetAnimationByTag("attack"); phaseDuration = act.attackDuration; break;
			case 3: animationName = GetAnimationByTag("backswing"); phaseDuration = act.backswingDuration; break;
		}
		Animator anim = obj.GetComponent<Animator>();
		anim.Play(animationName);
		if(phaseDuration > 0 && anim.GetCurrentAnimatorStateInfo(0).length > 0){
			anim.speed = 1 / phaseDuration;
		}else{
			anim.speed = 1;
		}

		if(appendage != null){
			appendage.PlayAttackAnimation(act,animPhase);
		}
	}

	public void AnimationEventCallback(int id){
		currentOffsetId = id;
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