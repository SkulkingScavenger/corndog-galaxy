using UnityEngine;
using System.Collections;
public class CreatureLimbObject : MonoBehaviour{
	public CreatureLimb root = null;
	
	public void AnimationEventCallback(int id){
		if(root != null){
			root.AnimationEventCallback(id);
		}
	}
}