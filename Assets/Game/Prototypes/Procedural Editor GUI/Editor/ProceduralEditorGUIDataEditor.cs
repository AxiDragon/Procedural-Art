using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProceduralEditorGUIData))]
public class ProceduralEditorGUIDataEditor : Editor
{
    private ProceduralEditorGUIData data;
    private SerializedProperty width;
    private SerializedProperty height;
    private SerializedProperty size;
    private SerializedProperty addButtonOnCombo;
    private SerializedProperty addButtonsBasedOnComboCount;
    private SerializedProperty removeButtonsOnCombo;
    
    private void OnEnable()
    {
        data = (ProceduralEditorGUIData)target;
        width = serializedObject.FindProperty("defaultWidthRange");
        height = serializedObject.FindProperty("defaultHeightRange");
        size = serializedObject.FindProperty("defaultSizeRange");
        addButtonOnCombo = serializedObject.FindProperty("addButtonOnCombo");
        addButtonsBasedOnComboCount = serializedObject.FindProperty("addButtonsBasedOnComboCount");
        removeButtonsOnCombo = serializedObject.FindProperty("removeButtonsOnCombo");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (data.isSquare)
        {
            EditorGUILayout.PropertyField(size);
            EditorUtility.SetDirty(data);
        }
        else
        {
            EditorGUILayout.PropertyField(width);
            EditorGUILayout.PropertyField(height);
            EditorUtility.SetDirty(data);
        }

        EditorGUILayout.PropertyField(addButtonOnCombo);

        if (data.addButtonOnCombo)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(addButtonsBasedOnComboCount);
            EditorGUILayout.PropertyField(removeButtonsOnCombo);
            EditorGUI.indentLevel--;
        }
        
        if (GUILayout.Button("Generate Rects"))
        {
            data.GenerateRects();
            EditorUtility.SetDirty(data);
        }
        
        serializedObject.ApplyModifiedProperties();
    }

}