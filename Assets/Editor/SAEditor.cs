using UnityEngine;
using UnityEditor;

using UnityEditor.Animations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System;

public class VirtualClip {
	public int startFrame = 0;
	public int endFrame = 0;
	public string name;


}

public class VirtualFrame {
	public int attachmentPreviousX = 0;
	public int attachmentPreviousY = 0;
	public int attachmentX = 0;
	public int attachmentY = 0;
	public Vector3 pivot;
	public Rect rect;
	public string name;
	public Texture2D texture;

}

public class SAEditor : EditorSplitWindowVertical{
	public Texture2D circleTexture;

	public static SAEditor Window { get; private set; }

	public Texture2D SpriteSheet;

	public float UniScale { get; set; }

	private string spriteName = "";
	
	//attachment stuff
	bool showAttachmentPointMenu = false;
	bool editingAttachmentPoint = false;
	int attachmentX = 0;
	int attachmentY = 0;

	//sprite stuff
	private int selectedSprite;
	private List<string> gridSpriteNames = new List<string>();
	private List<VirtualFrame> virtualFrames = new List<VirtualFrame>();
	private List<Sprite> sprites = new List<Sprite>();

	//animation stuff
	private AnimatorController animationControl = null;
	private List<VirtualClip> virtualClips = new List<VirtualClip>();

	private bool showSprites = true;
	private bool showAttachments = true;
	private bool popupActive;
	private EditorPopupEnterString popup;
	private UnityEngine.Object oldSelection;


	[MenuItem("Window/Sprite Sheet Attachments")]
	private static void Init()
	{
		Window = GetWindow<SAEditor>("Sprite Attachments Editor");
	}

	protected override void Awake(){
		base.Awake();
		UniScale = 1;
		selectedSprite = 0;
		oldSelection = null;
		virtualFrames.Clear();
		sprites.Clear();
		virtualClips.Clear();
		gridSpriteNames.Clear();
		circleTexture = Resources.Load("Assets/Circle32") as Texture2D;
	}

	private void OnDestroy(){

	}

	private void Update(){
		if (Window == null)
			Window = GetWindow<SAEditor>();

		if (oldSelection != Selection.activeObject){
			if (Selection.activeObject != null){
				if (Selection.activeObject.GetType() == typeof(Texture2D)){
					SpriteSheet = (Texture2D)Selection.activeObject;
					OnSpriteSheetLoaded();
				}else{
					SpriteSheet = null;
				}
				Repaint();
			}
		}

		oldSelection = Selection.activeObject;

		// if (sprites.Count > selectedSprite){
		// 	foreach (VirtualFrame vf in virtualFrames){
		// 		float sW = vf.rect.width * UniScale;
		// 		float sH = vf.rect.height * UniScale;

		// 		Vector3 pivot = new Vector3(vf.pivot.x * UniScale, vf.pivot.y * UniScale, 0.0f);
		// 		Vector2 iconSize = new Vector2(circleTexture.width / 2, circleTexture.height / 2);

		// 		float percentX = (attachment.EditorPosition.x - pivot.x + iconSize.x) / sW;
		// 		float percentY = (attachment.EditorPosition.y - pivot.y + iconSize.y) / sH;

		// 		attachment.NormalizedPosition = new Vector2(percentX, -percentY);
		// 	}
		// }
	}

	protected override void OnGUI(){
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("", EditorStyles.toolbar, GUILayout.MaxWidth(Screen.width));
		EditorGUILayout.EndHorizontal();
		base.OnGUI();
	}

	protected override void OnGUILeftView(){
		EditorGUIUtility.labelWidth = 80;
		EditorGUILayout.LabelField("");

		if (SpriteSheet != null){
			GUILayout.Space(10);

			//Attachment Point Stuff
			showAttachmentPointMenu = EditorGUILayout.Foldout(showAttachmentPointMenu, "Attachment Point");
			if (showAttachmentPointMenu){
				if(editingAttachmentPoint){
					if (GUILayout.Button("Remove Attachment Point")){
						foreach (VirtualFrame vf in virtualFrames){
							vf.attachmentX = 0;
							vf.attachmentY = 0;
						}
						editingAttachmentPoint = false;
						Repaint();
					}else if(virtualFrames.Count > 0){
						attachmentX = EditorGUILayout.IntField("X: ",attachmentX);
						attachmentY = EditorGUILayout.IntField("Y: ",attachmentY);
						if(attachmentX > virtualFrames[selectedSprite].rect.width){
							attachmentX = (int)Mathf.Round(virtualFrames[selectedSprite].rect.width);
						}
						if(attachmentX < 0){
							attachmentX = 0;
						}
						if(attachmentY > virtualFrames[selectedSprite].rect.height){
							attachmentY = (int)Mathf.Round(virtualFrames[selectedSprite].rect.height);
						}
						if(attachmentY < 0){
							attachmentY = 0;
						}
						if(attachmentX != virtualFrames[selectedSprite].attachmentX){
							virtualFrames[selectedSprite].attachmentX = attachmentX;
						}
						if(attachmentY != virtualFrames[selectedSprite].attachmentY){
							virtualFrames[selectedSprite].attachmentY = attachmentY;
						}
					}
				}else{
					if (GUILayout.Button("Edit Attachment Point")){
						editingAttachmentPoint = true;
						Repaint();
					}
				}
			}

			//Frame Navigator
			showSprites = EditorGUILayout.Foldout(showSprites, "Sprites");

			if (showSprites){
				selectedSprite = GUILayout.SelectionGrid(selectedSprite, gridSpriteNames.ToArray(), 1);
			}

			//Animation Editor
			showAttachments = EditorGUILayout.Foldout(showAttachments, "Animations");

			if (showAttachments){

	
				GUI.backgroundColor = Color.green;
				

				if (popup == null)
					popupActive = false;

				if (GUILayout.Button("Create") && !popupActive){
					popupActive = true;
					popup = GetWindow<EditorPopupEnterString>();
					popup.m_DemandInput = true;
					popup.Focus();
					popup.OnConfirm += (string name) => {
						VirtualClip vc;
						for (int i=0;i<virtualClips.Count;i++){
							vc = virtualClips[i];
							if (vc.name == name){
								Debug.LogError(string.Format("[SAEditor] Animation with name '{0}' already exists.", name));
								return;
							}
						}
						vc = new VirtualClip();
						vc.name = name;
						virtualClips.Add(vc);
						Repaint();
						popupActive = false;
					};
				}


				for(int i=virtualClips.Count-1;i>=0;i--){
					GUI.backgroundColor = Color.red;
					EditorGUILayout.LabelField(virtualClips[i].name);
					if (GUILayout.Button("Delete")){
						virtualClips.RemoveAt(i);
						Repaint();
					}else{
						virtualClips[i].startFrame = EditorGUILayout.IntField("Start: ",virtualClips[i].startFrame);
						virtualClips[i].endFrame = EditorGUILayout.IntField("End: ",virtualClips[i].endFrame);

						if(virtualClips[i].startFrame >= virtualFrames.Count){
							virtualClips[i].startFrame = virtualFrames.Count;
						}
						if(virtualClips[i].startFrame < 0){
							virtualClips[i].startFrame = 0;
						}
						if(virtualClips[i].endFrame >= virtualFrames.Count){
							virtualClips[i].endFrame = virtualFrames.Count;
						}
						if(virtualClips[i].endFrame < 0){
							virtualClips[i].endFrame = 0;
						}
					}
				}
				GUI.backgroundColor = Color.white;
			}
		}else{
			gridSpriteNames.Clear();
			selectedSprite = 0;

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Select Sprite Attachment Sheet Asset");
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
	}

	protected override void OnGUIRightView(){
		if (SpriteSheet != null){
			if (virtualFrames.Count > 0){
				Texture t = virtualFrames[selectedSprite].texture;
				Rect tr = virtualFrames[selectedSprite].rect;
				Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height);

				float sW = virtualFrames[selectedSprite].rect.width * UniScale;
				float sH = virtualFrames[selectedSprite].rect.height * UniScale;

				GUI.DrawTextureWithTexCoords(new Rect(0, 0, sW, sH), t, r);

				if (Event.current.type == EventType.MouseDown){
					Rect circleRect = new Rect(
						attachmentX,
						attachmentY,
						circleTexture.width,
						circleTexture.height);

					Rect mouseRect = new Rect(
						Event.current.mousePosition.x,
						Event.current.mousePosition.y, 1, 1);

					if (circleTexture != null && editingAttachmentPoint){
						GUIStyle grayStyle = GUIStyle.none;
						//grayStyle.normal.textColor = Color.grey;
						grayStyle.normal.textColor = Color.green;

						GUI.DrawTexture(circleRect, circleTexture);
						GUI.color = Color.white;
					}
				}
			}
		}else{
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Select Spritesheet");
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
	}
	private void GetAnimator(string animationName){
		string controllerPath = "";
		string[] temp = AssetDatabase.FindAssets(string.Concat("t:AnimatorController ",animationName));
		if(temp.Length > 0){
			controllerPath = string.Concat("Assets/Resources/Animation/Controllers/"+animationName+".controller");
			animationControl = (AnimatorController)AssetDatabase.LoadAssetAtPath(controllerPath, typeof(AnimatorController));
		}else{
			controllerPath = string.Concat("Assets/Resources/Animation/Controllers/"+animationName+".controller");
			AssetDatabase.CreateAsset(new AnimatorController(), controllerPath);
			animationControl = (AnimatorController)AssetDatabase.LoadAssetAtPath(controllerPath, typeof(AnimatorController));
			animationControl.AddLayer("New Layer");
		}

		for(int i=0;i<animationControl.layers.Length;i++){
			ChildAnimatorState[] states = animationControl.layers[i].stateMachine.states;
			Debug.Log(animationControl.layers[i].stateMachine);
			for(int j=0;j<states.Length;j++){
				AnimationClip ac = (AnimationClip)states[j].state.motion;
				
			}
		}
	}


	private void OnSpriteSheetLoaded(){
		Debug.Log("it's called");
		virtualFrames.Clear();
		gridSpriteNames.Clear();
		string path = AssetDatabase.GetAssetPath(SpriteSheet);
		string[] temp = path.Split('/');
		temp = temp[temp.Length-1].Split('.');
		spriteName = temp[0].ToLower();

		GetAnimator(spriteName);

		Sprite[] foundSprites = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().ToArray();
		sprites = foundSprites.ToList();
		selectedSprite = 0;
		
		foreach (Sprite sprite in sprites){
			VirtualFrame vf = new VirtualFrame();
			vf.texture = sprite.texture;
			vf.name = sprite.name;
			vf.pivot = sprite.pivot;
			vf.rect = sprite.rect;
			virtualFrames.Add(vf);
			gridSpriteNames.Add(vf.name);
		}

		// 	CalculateUniScale(sprites[selectedSprite]);
		Repaint();
	}

	private void createVirtualClip(string name){
		
	}

	private AnimationClip generateClip(VirtualClip vc){
		AnimationClip ac = new AnimationClip();
		ac.name = vc.name;
		if(vc.startFrame < vc.endFrame){
			vc.startFrame = vc.endFrame;
		}
		int l = vc.endFrame - vc.startFrame + 1;
		float timeIncrement = 1f/l;
		AnimationEvent evt;
		EditorCurveBinding curveBinding = new EditorCurveBinding();
		curveBinding.type = typeof(SpriteRenderer);
		curveBinding.propertyName = "m_Sprite";
		ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[l];
		for(int i=vc.startFrame;i<l;i++){
			if(editingAttachmentPoint){
				evt = new AnimationEvent();
				evt.functionName = "AnimationEventCallback";
				evt.intParameter = i;
				ac.AddEvent(evt);
			}
			keyFrames[i] = new ObjectReferenceKeyframe();
			keyFrames[i].time = i * timeIncrement;
			keyFrames[i].value = sprites[i];
		}
		AnimationUtility.SetObjectReferenceCurve(ac, curveBinding, keyFrames);

		return ac;
	}

	private void SaveChanges(){
		if(SpriteSheet == null){return;}

		string path;
		VirtualClip vc;
		AnimationClip ac;
		for(int i=0;i<virtualClips.Count;i++){
			vc = virtualClips[i];
			ac = generateClip(vc);
			path = string.Concat("Assets/Resources/Animation/Clips/Organs/"+vc.name+".anim");
			AssetDatabase.CreateAsset(ac, path);
			animationControl.layers[0].stateMachine.AddState(vc.name);
		}
		path = string.Concat("Assets/Resources/Animation/Controllers/"+spriteName+".controller");
		AssetDatabase.CreateAsset(new AnimatorController(), path);
		if(editingAttachmentPoint){

		}
	}

	private void CalculateUniScale(Sprite sprite){
		if (Screen.width > sprite.rect.width || Screen.height > sprite.rect.height){
			float pixlDiffX = (Screen.width - sprite.rect.width) / (float)sprite.rect.width;
			float pixlDiffY = (Screen.height - sprite.rect.height) / (float)sprite.rect.height;
			UniScale = (1.0f + ((pixlDiffX + pixlDiffY) / 2.0f)) * 0.5f;
			UniScale = 1.0f + (1.0f / ((sprite.rect.width + sprite.rect.height) / 2.0f)) * 500.0f;
			UniScale = 1;
		}
	}
}
