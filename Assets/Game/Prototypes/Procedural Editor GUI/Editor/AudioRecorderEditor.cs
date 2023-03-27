using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioRecorder))]
public class AudioRecorderEditor : Editor
{
    private AudioRecorder ar;

    private void OnEnable()
    {
        ar = (AudioRecorder)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        
        if (GUILayout.Button("Set Saving Location"))
        {
            AudioRecorder.savingLocation = EditorUtility.OpenFolderPanel("Select Saving Location",
                String.IsNullOrEmpty(AudioRecorder.savingLocation) ? "" : AudioRecorder.savingLocation, "");
            
            EditorUtility.SetDirty(ar);
        }

        GUI.enabled = false;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("recordedClip"));
    }
}