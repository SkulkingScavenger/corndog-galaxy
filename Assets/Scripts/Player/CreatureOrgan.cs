using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatureOrgan{
	public Creature root;
	public Vector3 offset;
	public string name;
	public CreatureOrgan rootOrgan;
	public string type;
	public string organLayer;
	public int hitpoints;
	public GameObject obj = null;

	public int prototypeIndex;

	public string animationControllerName = "";
	public List<OrganAnimation> animations = new List<OrganAnimation>();

	public void PlayAnimation(string tag){
		string animationName = GetAnimationByTag(tag);
		if(animationName != ""){
			obj.GetComponent<Animator>().Play(animationName);
		}else{
			Debug.Log(name+" has no animation with tag: " + tag);
		}
	}

	public string GetAnimationByTag(params string[] tags){
		for(int i=0;i<animations.Count;i++){
			if (animations[i].HasTag(tags)){
				return animations[i].name;
			}
		}
		return "";
	}

	public List<OrganAnimation> GetAnimationsWithTag(string tag){
		List<OrganAnimation> temp = new List<OrganAnimation>();
		for(int i=0;i<animations.Count;i++){
			if (animations[i].HasTag(tag)){
				temp.Add(animations[i]);
			}
		}
		return temp;
	}
}

public class OrganAnimation {
	public string name = "";
	public List<string> tags = new List<string>();

	public bool HasTag(params string[] attrs){
		string[] subtags;
		bool[] flags = new bool[attrs.Length];
		if(attrs.Length == 0){return false;}
		for(int i=0;i<attrs.Length;i++){
			flags[i] = false;
		}
		for(int i=0;i<tags.Count;i++){
			subtags = tags[i].Split('-');
			for(int j=0;j<subtags.Length;j++){
				for(int k=0;k<attrs.Length;k++){
					if(subtags[j] == attrs[k]){
						flags[k] = true;
					}
				}
			}
		}
		for(int i=0;i<attrs.Length;i++){
			if(flags[i] == false){
				return false;
			}
		}
		return true;
	}
}


public class Chemical{

}

public class Material : Chemical {

}

public class OrganTissue : Material{

}