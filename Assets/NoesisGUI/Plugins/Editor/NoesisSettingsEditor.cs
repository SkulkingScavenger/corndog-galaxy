using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NoesisSettings))]
public class NoesisSettingsEditor: Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(25);
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if(GUILayout.Button("Reimport All", GUILayout.MaxWidth(175), GUILayout.MinHeight(20)))
        {
            NoesisPostprocessor.ImportAllAssets();
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }
}
