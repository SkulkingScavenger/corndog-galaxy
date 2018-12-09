using UnityEngine.UI;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EditorPopupEnterOrganData : EditorWindow 
{
	public static EditorPopupEnterString m_Window   { get; private set; }
	public string m_Name							{ get; private set; }
	public bool m_DemandInput					   { get; set; }
	public event System.Action<string,string,string,string> OnConfirm = delegate { };
	private string tag;
	private string[] organTypes = {"Segment", "Limb", "Appendage"};
	private string[][] organSubtypes = {new []{"thorax","abdomen","torso"}, new []{"segmented","tentacle","leg","arm","pseudopod"}, new []{"appendage"}};
	private int organIndex = 0;
	private int organSubtypeIndex = 0;

	private static void Init()
	{
		m_Window = new EditorPopupEnterString();
		m_Window.ShowPopup();
	}

	private void OnGUI()
	{
		EditorGUILayout.LabelField("Enter Name");
		m_Name = EditorGUILayout.TextField(m_Name);

		EditorGUILayout.LabelField("Enter Tag (no spaces");
		tag = EditorGUILayout.TextField(tag);

		EditorGUILayout.BeginHorizontal();
		for(int i=0;i<organTypes.Length;i++){
			if(i==organIndex){
				GUI.backgroundColor = Color.green;
			}
			if(GUILayout.Button(organTypes[i])){
				organIndex = i;
				organSubtypeIndex = 0;
			}
			GUI.backgroundColor = Color.white;
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUI.backgroundColor = Color.white;
		for(int i=0;i<organSubtypes[organIndex].Length;i++){
			if(i==organSubtypeIndex){
				GUI.backgroundColor = Color.green;
			}
			if(GUILayout.Button(organSubtypes[organIndex][i])){
				organSubtypeIndex = i;
			}
			GUI.backgroundColor = Color.white;
		}
		EditorGUILayout.EndHorizontal();


		if(GUILayout.Button("Confirm") || Event.current.keyCode == KeyCode.Return)
		{
			if (!m_DemandInput || m_DemandInput && !string.IsNullOrEmpty(m_Name) && !string.IsNullOrEmpty(tag))
			{
				OnConfirm(m_Name, tag, organTypes[organIndex], organSubtypes[organIndex][organSubtypeIndex]);
				Close();
			}
		}
	}
}
