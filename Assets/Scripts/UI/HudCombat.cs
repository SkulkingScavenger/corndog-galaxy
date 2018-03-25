using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HudCombat : MonoBehaviour {

	Player player;
	CreatureControl control;
	GameObject[] moveSetIcons = new GameObject[10];
	Creature creature;
	Sprite[] moveSetIconSprites;
	Sprite[] hudElementSprites;
	Sprite moveSetIconBoxSprite;
	Sprite moveSetIconBoxSpriteHighlighted;
	Sprite moveSetIconBoxSpriteDisabled;
	Sprite moveSetIconBoxSpritePressed;

	public void Awake(){
		player = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>().GetPlayer();
		control = player.inputControl;

		transform.Find("ExplorationModeButton").GetComponent<Button>().onClick.AddListener(delegate { SetInterfaceModeExploration(); });
		transform.Find("IndustryModeButton").GetComponent<Button>().onClick.AddListener(delegate { SetInterfaceModeExploration(); });

		moveSetIcons[0] = transform.Find("MoveSetDisplay").transform.Find("PrimaryActionBar").transform.Find("ActionButton0").gameObject;
		moveSetIcons[1] = transform.Find("MoveSetDisplay").transform.Find("PrimaryActionBar").transform.Find("ActionButton1").gameObject;
		moveSetIcons[2] = transform.Find("MoveSetDisplay").transform.Find("PrimaryActionBar").transform.Find("ActionButton2").gameObject;
		moveSetIcons[3] = transform.Find("MoveSetDisplay").transform.Find("PrimaryActionBar").transform.Find("ActionButton3").gameObject;
		moveSetIcons[4] = transform.Find("MoveSetDisplay").transform.Find("PrimaryActionBar").transform.Find("ActionButton4").gameObject;
		moveSetIcons[5] = transform.Find("MoveSetDisplay").transform.Find("SecondaryActionBar").transform.Find("ActionButton0").gameObject;
		moveSetIcons[6] = transform.Find("MoveSetDisplay").transform.Find("SecondaryActionBar").transform.Find("ActionButton1").gameObject;
		moveSetIcons[7] = transform.Find("MoveSetDisplay").transform.Find("SecondaryActionBar").transform.Find("ActionButton2").gameObject;
		moveSetIcons[8] = transform.Find("MoveSetDisplay").transform.Find("SecondaryActionBar").transform.Find("ActionButton3").gameObject;
		moveSetIcons[9] = transform.Find("MoveSetDisplay").transform.Find("SecondaryActionBar").transform.Find("ActionButton4").gameObject;

		moveSetIconSprites = Resources.LoadAll<Sprite>("Sprites/_UI/combat_move_icons");
		hudElementSprites = Resources.LoadAll<Sprite>("Sprites/_UI/hud_elements_01");
		moveSetIconBoxSprite = hudElementSprites[12];
		moveSetIconBoxSpritePressed = hudElementSprites[13];
		moveSetIconBoxSpriteHighlighted = hudElementSprites[11];
		moveSetIconBoxSpriteDisabled = hudElementSprites[17];
	}

	public void Update(){
		creature = player.creature;
		if(creature == null){return;}
		SetMoveSetIcons();

	}

	public void SetInterfaceModeExploration(){
		player.SetInterfaceMode("exploration");
	}
	public void SetInterfaceModeIndustry(){
		player.SetInterfaceMode("industry");
	}

	private void SetMoveSetIcons(){
		for(int i=0;i<moveSetIcons.Length;i++){
			Image iconBoxImage = moveSetIcons[i].GetComponent<Image>();
			Image iconImage = moveSetIcons[i].transform.Find("Icon").GetComponent<Image>();
			CombatMoveSet moveSet = creature.stances[creature.stanceId].combatMoveSets[i];
			if(moveSet != null){
				iconImage.sprite = moveSetIconSprites[moveSet.iconIndex];
				iconImage.color = new Color(1f,1f,1f,1f);
				if(moveSet.IsDisabled()){
					iconBoxImage.sprite = moveSetIconBoxSpriteDisabled;
				}else if(i == GetHighlightedIconIndex()){
					iconBoxImage.sprite = moveSetIconBoxSpriteHighlighted;
				}else{
					iconBoxImage.sprite = moveSetIconBoxSprite;
				}
			}else{
				iconBoxImage.sprite = moveSetIconBoxSpritePressed;
				iconImage.color = new Color(1f,1f,1f,0f);
				
			}
		}
	}

	private int GetHighlightedIconIndex(){
		int iconIndex = 0;
		for(int i=0;i<4;i++){
			if(control.actionModifier[i]){
				iconIndex = i+1;
				break;
			}
		}
		if(control.shift){
			iconIndex += 5;
		}
		return iconIndex;
	}
}