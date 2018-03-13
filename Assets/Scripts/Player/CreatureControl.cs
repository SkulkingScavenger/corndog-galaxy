using UnityEngine;
using UnityEngine.Networking;

public class CreatureControl : NetworkBehaviour {
	[SyncVar] public string interfaceMode = "combat";
	[SyncVar] public bool moveCommand = false;
	[SyncVar] public bool attackCommand = false;

	[SyncVar] public float commandX = 0;
	[SyncVar] public float commandY = 0;

	public SyncListBool actionModifier = new SyncListBool();
	public SyncListBool stanceModifier = new SyncListBool();

	[SyncVar] public bool shift = false;
	[SyncVar] public bool ctrl = false;
	[SyncVar] public bool jump = false;

	public void ManageArrays(){
		if(actionModifier.Count == 0){
			for(int i=0;i<4;i++){
				actionModifier.Add(false);
				stanceModifier.Add(false);
			}
		}
	}
}