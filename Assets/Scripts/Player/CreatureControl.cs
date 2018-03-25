using UnityEngine;
using UnityEngine.Networking;

public class CreatureControl : NetworkBehaviour {
	public bool isPlayerControlled = false;
	public bool isHudCommand = false;
	public string interfaceMode = "combat";
	public bool moveCommand = false;
	public bool attackCommand = false;

	public float commandX = 0;
	public float commandY = 0;

	public bool[] actionModifier = {false,false,false,false};
	public bool[] stanceModifier = {false,false,false,false};

	public bool shift = false;
	public bool ctrl = false;
	public bool jump = false;

	public void Sync(){
		if(isServer){
			RpcSync(interfaceMode,isPlayerControlled,isHudCommand,moveCommand,attackCommand,commandX,commandY,actionModifier,stanceModifier,shift,ctrl,jump);
		}else{
			CmdSync(interfaceMode,isPlayerControlled,isHudCommand,moveCommand,attackCommand,commandX,commandY,actionModifier,stanceModifier,shift,ctrl,jump);
		}
	}

	[Command] public void CmdSync(string im, bool ipc, bool ihc, bool mc, bool ac, float cx, float cy, bool[] am, bool[] sm, bool shi, bool ctr, bool jum){
		interfaceMode = im;
		isPlayerControlled = ipc;
		isHudCommand = ihc;

		moveCommand = mc;
		attackCommand = ac;

		commandX = cx;
		commandY = cy;

		actionModifier = am;
		stanceModifier = sm;

		shift = shi;
		ctrl = ctr;
		jump = jum;
	}
	[ClientRpc] public void RpcSync(string im, bool ipc, bool ihc, bool mc, bool ac, float cx, float cy, bool[] am, bool[] sm, bool shi, bool ctr, bool jum){
		interfaceMode = im;
		isPlayerControlled = ipc;
		isHudCommand = ihc;

		moveCommand = mc;
		attackCommand = ac;

		commandX = cx;
		commandY = cy;

		actionModifier = am;
		stanceModifier = sm;

		shift = shi;
		ctrl = ctr;
		jump = jum;
	}	
}