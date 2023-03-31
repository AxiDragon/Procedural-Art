using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ExpoInfo))]
public class ExpoInfoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorStyles.label.wordWrap = true;
        EditorGUILayout.HelpBox(new GUIContent("Click on the cubes!"));
        EditorGUILayout.HelpBox(new GUIContent("Click on regenerate to reset."));
        EditorGUILayout.LabelField("If you want your recording, put your email and time of recording here!");
        base.OnInspectorGUI();
    }
}
