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

public class AttachmentPoint {
	public int[] x;
	public int[] y;
	public float[] rotation;
}

public class VirtualClip {
	public int startFrame = 0;
	public int endFrame = 0;
	public string name;
}

public class VirtualFrame {
	public Vector3 pivot;
	public Rect rect;
	public string name;
	public Texture2D texture;
}

public class SAEditor : EditorSplitWindowVertical{
	public Texture2D circleTexture;

	public static SAEditor Window { get; private set; }

	public Texture2D SpriteSheet;

	private string spriteName = "";

	private string organName = "";
	private string organTag = "";
	private string organType = "";
	private string organSubtype = "";
	
	//attachment stuff
	int selectedAttachmentIndex = 0;
	bool showAttachmentPointMenu = false;
	List<AttachmentPoint> attachmentPoints = new List<AttachmentPoint>();

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
		Window = GetWindow<SAEditor>("Spritesheet Processor");
	}

	protected override void Awake(){
		base.Awake();
		selectedSprite = 0;
		oldSelection = null;
		attachmentPoints.Clear();
		virtualFrames.Clear();
		sprites.Clear();
		virtualClips.Clear();
		gridSpriteNames.Clear();
		circleTexture = Resources.Load("Editor/Circle32") as Texture2D;
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

	}

	protected override void OnGUI(){
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("", EditorStyles.toolbar, GUILayout.MaxWidth(Screen.width));
		EditorGUILayout.EndHorizontal();
		base.OnGUI();
	}

	protected override void OnGUILeftView(){
		EditorGUIUtility.labelWidth = 120;
		EditorGUILayout.LabelField("");

		if (SpriteSheet != null){
			GUILayout.Space(30);
			if (popup == null){popupActive = false;}
			if (GUILayout.Button("Finalize Import")){
				SaveAs();
			}
			DrawAttachmentPointMenu();
			DrawFrameMenu();
			DrawAnimationMenu();		
		}else{
			gridSpriteNames.Clear();
			selectedSprite = 0;
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Select Sprite");
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
	}

	private void DrawAttachmentPointMenu(){
		showAttachmentPointMenu = EditorGUILayout.Foldout(showAttachmentPointMenu, "Attachment Point");
		
		if (showAttachmentPointMenu){
			EditorGUILayout.BeginHorizontal();
			GUI.backgroundColor = Color.green;
			if (GUILayout.Button("Create")){
				CreateAttachmentPoint();
			}
			GUI.backgroundColor = Color.red;
			if (GUILayout.Button("Delete")){
				attachmentPoints.RemoveAt(selectedAttachmentIndex);
				selectedAttachmentIndex = 0;
				Repaint();
			}
			EditorGUILayout.EndHorizontal();
			GUI.backgroundColor = Color.white;
			EditorGUIUtility.labelWidth = 20;
			AttachmentPoint p;
			for(int i=0;i<attachmentPoints.Count;i++){
				EditorGUILayout.BeginHorizontal();
				p = attachmentPoints[i];
				if(EditorGUILayout.Toggle(selectedAttachmentIndex == i)){
					selectedAttachmentIndex = i;
					Repaint();
				}
				p.x[selectedSprite] = EditorGUILayout.IntField("X: ",p.x[selectedSprite]);
				p.y[selectedSprite] = EditorGUILayout.IntField("Y: ",p.y[selectedSprite]);
				p.rotation[selectedSprite] = EditorGUILayout.FloatField("θ: ",p.rotation[selectedSprite]);
				EditorGUILayout.EndHorizontal();
				if(p.x[selectedSprite] > virtualFrames[selectedSprite].rect.width){
					p.x[selectedSprite] = (int)Mathf.Round(virtualFrames[selectedSprite].rect.width);
				}
				if(p.x[selectedSprite] < 0){
					p.x[selectedSprite] = 0;
				}
				if(p.y[selectedSprite] > virtualFrames[selectedSprite].rect.height){
					p.y[selectedSprite] = (int)Mathf.Round(virtualFrames[selectedSprite].rect.height);
				}
				if(p.y[selectedSprite] < 0){
					p.y[selectedSprite] = 0;
				}
			}
		}
	}

	private void CreateAttachmentPoint(){
		AttachmentPoint p = new AttachmentPoint();
		p.x = new int[virtualFrames.Count];
		p.y = new int[virtualFrames.Count];
		p.rotation = new float[virtualFrames.Count];
		for(int i=0;i<virtualFrames.Count;i++){
			p.x[i] = 0;
			p.y[i] = 0;
			p.rotation[i] = 0;
		}
		attachmentPoints.Add(p);
	}

	private void DrawFrameMenu(){
		showSprites = EditorGUILayout.Foldout(showSprites, "Frames");

		if (showSprites){
			selectedSprite = GUILayout.SelectionGrid(selectedSprite, gridSpriteNames.ToArray(), 1);
		}
	}

	private void DrawAnimationMenu(){
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
			EditorGUIUtility.labelWidth = 40;
			GUI.backgroundColor = Color.white;
			for(int i=0;i<virtualClips.Count;i++){
				EditorGUILayout.LabelField(virtualClips[i].name);
				EditorGUILayout.BeginHorizontal();
				virtualClips[i].startFrame = EditorGUILayout.IntField("Start: ", virtualClips[i].startFrame);
				virtualClips[i].endFrame = EditorGUILayout.IntField("End: ", virtualClips[i].endFrame);

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
				GUI.backgroundColor = Color.red;
				if (GUILayout.Button("Delete")){
					virtualClips.RemoveAt(i);
					Repaint();
				}
				GUI.backgroundColor = Color.white;
				EditorGUILayout.EndHorizontal();
			}
		}
	}

	protected override void OnGUIRightView(){
		if (SpriteSheet != null){
			if (virtualFrames.Count > 0){
				AttachmentPoint p;
				Vector2 pivot;
				GUIStyle textStyle = GUIStyle.none;
				Rect textRect;
				Texture t = virtualFrames[selectedSprite].texture;
				Rect tr = virtualFrames[selectedSprite].rect;
				Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height);

				float sW = virtualFrames[selectedSprite].rect.width;
				float sH = virtualFrames[selectedSprite].rect.height;

				GUI.DrawTextureWithTexCoords(new Rect(0, 0, sW, sH), t, r);

				if (Event.current.type == EventType.MouseDown){
					p = attachmentPoints[selectedAttachmentIndex];
					p.x[selectedSprite] = (int)Mathf.Floor(Event.current.mousePosition.x);
					p.y[selectedSprite] = (int)Mathf.Floor(Event.current.mousePosition.y);
				}
  
				for(int i=0;i<attachmentPoints.Count;i++){
					p = attachmentPoints[i];
					if(selectedAttachmentIndex == i){
						GUI.color = Color.green;
						textStyle.normal.textColor = Color.green;
					}else{
						GUI.color = Color.white;
						textStyle.normal.textColor = Color.grey;
					}

					textRect = new Rect(p.x[selectedSprite], p.y[selectedSprite] - 20f, 400, 100);
					GUI.Label(textRect, i.ToString(), textStyle);

					pivot = new Vector2(p.x[selectedSprite], p.y[selectedSprite]);
					Matrix4x4 matrixBackup = GUI.matrix;
				    GUIUtility.RotateAroundPivot(p.rotation[selectedSprite], pivot);
					GUI.DrawTexture(new Rect(p.x[selectedSprite]-16,p.y[selectedSprite]-16,32,32), circleTexture);
					GUI.matrix = matrixBackup;
				}
				GUI.color = Color.white;
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
	private bool GetAnimator(string animationName){
		bool alreadyExists;
		string controllerPath = "";
		string[] temp = AssetDatabase.FindAssets(string.Concat("t:AnimatorController ",animationName));
		controllerPath = string.Concat("Assets/Resources/Animation/Controllers/"+animationName+".controller");
		if(temp.Length > 0){
			animationControl = (AnimatorController)AssetDatabase.LoadAssetAtPath(controllerPath, typeof(AnimatorController));
			alreadyExists = true;
		}else{
			AssetDatabase.CreateAsset(new AnimatorController(), controllerPath);
			animationControl = (AnimatorController)AssetDatabase.LoadAssetAtPath(controllerPath, typeof(AnimatorController));
			animationControl.AddLayer("New Layer");
			alreadyExists = false;
		}

		for(int i=0;i<animationControl.layers.Length;i++){
			ChildAnimatorState[] states = animationControl.layers[i].stateMachine.states;
			for(int j=0;j<states.Length;j++){
				AnimationClip ac = (AnimationClip)states[j].state.motion;
				
			}
		}
		return alreadyExists;
	}


	private void OnSpriteSheetLoaded(){
		virtualFrames.Clear();
		gridSpriteNames.Clear();
		string path = AssetDatabase.GetAssetPath(SpriteSheet);
		string[] temp = path.Split('/');
		temp = temp[temp.Length-1].Split('.');
		spriteName = temp[0].ToLower();

		Sprite[] foundSprites = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().ToArray();
		sprites = foundSprites.ToList();
		PreprocessSpriteSheet(sprites,path);
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

		attachmentPoints.Clear();
		Repaint();
	}

	private void PreprocessSpriteSheet(List<Sprite> sprites, string path){
		TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(path);
		TextureImporterSettings importerSettings = new TextureImporterSettings();
		bool isSpriteUnsliced = sprites.Count == 1 || importerSettings.spriteMode == (int)SpriteImportMode.Single;
		bool isSpriteSliceable = sprites[0].texture.width > sprites[0].texture.height && sprites[0].texture.width % sprites[0].texture.height == 0;
		if(isSpriteUnsliced && isSpriteSliceable){
			//set import metadata
			
			ti.ReadTextureSettings(importerSettings);
			importerSettings.spritePixelsPerUnit = 128;
			importerSettings.filterMode = FilterMode.Point;
			importerSettings.mipmapEnabled = false;
			importerSettings.alphaIsTransparency = true;
			importerSettings.spriteMode = (int)SpriteImportMode.Multiple;
			ti.SetTextureSettings(importerSettings);

			//slice the sprite
			int frameCount = (int)Mathf.Floor(sprites[0].texture.width / sprites[0].texture.height);
			SpriteMetaData[] frames = new SpriteMetaData[frameCount];
			float cellWidth = sprites[0].texture.height;
			for(int i=0;i<frameCount;i++){
				frames[i] = new SpriteMetaData();
				frames[i].alignment = 0;
				frames[i].name = sprites[0].name + "_" + i.ToString();
				frames[i].rect = new Rect(i*cellWidth,0f,cellWidth,cellWidth);
				frames[i].border = new Vector4(cellWidth,cellWidth,cellWidth,cellWidth);
				frames[i].pivot = new Vector2(cellWidth/2,cellWidth/2);
			}
			ti.spritesheet = frames;

			AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
		}
	}

	private AnimationClip GenerateClip(VirtualClip vc){
		AnimationClip ac = new AnimationClip();
		ac.name = vc.name;
		if(vc.startFrame > vc.endFrame){
			int tempint = vc.startFrame;
			vc.startFrame = vc.endFrame;
			vc.endFrame = tempint;
		}
		int l = Mathf.Abs(vc.endFrame - vc.startFrame) + 1;
		float timeIncrement = 1f/l;
		AnimationEvent evt;
		EditorCurveBinding curveBinding = new EditorCurveBinding();
		curveBinding.type = typeof(SpriteRenderer);
		curveBinding.propertyName = "m_Sprite";
		ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[l];
		AnimationEvent[] events = new AnimationEvent[l];
		for(int i=0;i<l;i++){

			//event hooks for animations
			evt = new AnimationEvent();
			evt.time = i * timeIncrement;
			evt.functionName = "AnimationEventCallback";
			evt.intParameter = i + vc.startFrame;
			events[i] = evt;//ac.AddEvent(evt);

			keyFrames[i] = new ObjectReferenceKeyframe();
			keyFrames[i].time = i * timeIncrement;
			keyFrames[i].value = sprites[i + vc.startFrame];
		}
		AnimationUtility.SetObjectReferenceCurve(ac, curveBinding, keyFrames);
		AnimationUtility.SetAnimationEvents(ac, events);

		return ac;
	}

	private void SaveAs(){
		if (SpriteSheet != null && !popupActive){
			EditorPopupEnterOrganData organPopup;
			popupActive = true;
			organPopup = GetWindow<EditorPopupEnterOrganData>();
			organPopup.m_DemandInput = true;
			organPopup.Focus();
			organPopup.OnConfirm += (string name, string tag, string type, string subtype) => {
				organName = name;
				organTag = tag;
				organType = type;
				organSubtype = subtype;
				popupActive = false;
				SaveChanges();
			};
		}
	}

	private void WriteOrganToXml(){
		XmlWriter xml = new XmlWriter();
		XmlNode node = new XmlNode();
		XmlNode subNode;
		node.name = "Organ" + organType + "Node";
		node.attributes.Add(new XmlAttribute("name",organName));
		node.attributes.Add(new XmlAttribute(organType.ToLower()+"Type",organSubtype));
		node.attributes.Add(new XmlAttribute("animationControllerName","Animation/Controllers/"+organTag));
		for(int i=0; i<virtualClips.Count;i++){
			subNode = new XmlNode();
			subNode.name = "AnimationNode";
			subNode.attributes.Add(new XmlAttribute("name",organTag + "_" + virtualClips[i].name));
			subNode.subnodes.Add(new XmlNode());
			subNode.subnodes[0].name = "TagNode";
			subNode.subnodes[0].attributes.Add(new XmlAttribute("value", virtualClips[i].name));
			node.subnodes.Add(subNode);
		}
		for(int i=0; i<attachmentPoints.Count;i++){
			subNode = new XmlNode();
			subNode.name = "AttachmentPointNode";
			subNode.attributes.Add(new XmlAttribute("id", i.ToString()));
			for(int j=0;j<attachmentPoints[i].x.Length;j++){
				subNode.subnodes.Add(new XmlNode());
				subNode.subnodes[j].name = "OffsetNode";
				subNode.subnodes[j].attributes.Add(new XmlAttribute("x", attachmentPoints[i].x[j].ToString()));
				subNode.subnodes[j].attributes.Add(new XmlAttribute("y", attachmentPoints[i].y[j].ToString()));
				subNode.subnodes[j].attributes.Add(new XmlAttribute("z", "0"));
				subNode.subnodes[j].attributes.Add(new XmlAttribute("rotation",  attachmentPoints[i].rotation[j].ToString()));
			}
			node.subnodes.Add(subNode);
		}
		string path = "Assets/Resources/Text/Organs/" + organTag + ".xml";
		xml.writeToFile(path, node);
	}


	private void SaveChanges(){
		string path;
		VirtualClip vc;
		AnimationClip ac;
		bool AnimatorAlreadyExists;
		AnimatorAlreadyExists = GetAnimator(organTag);
		for(int i=0;i<virtualClips.Count;i++){
			vc = virtualClips[i];
			ac = GenerateClip(vc);
			path = string.Concat("Assets/Resources/Animation/Clips/organs/"+vc.name+".anim");
			AssetDatabase.CreateAsset(ac, path);
			animationControl.layers[0].stateMachine.AddState(vc.name);
		}
		path = string.Concat("Assets/Resources/Animation/Controllers/"+spriteName+".controller");
		AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

		WriteOrganToXml();
	}
}
