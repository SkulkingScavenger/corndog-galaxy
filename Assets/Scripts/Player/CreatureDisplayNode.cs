using UnityEngine;
using System.Collections;
public class CreatureDisplayNode : MonoBehaviour{
	public CreatureLimb root = null;
	public CreatureBodySegment rootSegment = null;
	
	public void AnimationEventCallback(int id){
		if(root != null){
			root.AnimationEventCallback(id);
		}else if(rootSegment != null){
			rootSegment.AnimationEventCallback(id);
		}
	}
}