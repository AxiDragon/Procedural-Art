using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProceduralPiano : EditorWindow
{
    private ProceduralEditorGUIData selectedData;
    private GUIStyle nodeStyle;
    private GUIStyle comboStyle;
    private GUIStyle verticalStyle;
    private static double allowedComboTime = 3;
    private static double timeSinceLastCombo = 0f;
    private static int combo;
    private bool unprocessedInput;
    private bool buttonHeld;
    private bool hoveringOverButton;
    private bool hitCombo;
    private Event e;

    [MenuItem("Procedural/Procedural Piano")]
    public static void OpenWindow()
    {
        GetWindow(typeof(ProceduralPiano), false, "Procedural Piano");
    }

    [OnOpenAsset(1)]
    public static bool OnOpenAsset(int id, int line)
    {
        ProceduralEditorGUIData data = EditorUtility.InstanceIDToObject(id) as ProceduralEditorGUIData;

        if (data)
        {
            GetWindow(typeof(ProceduralPiano), false, "Procedural Piano");
        }

        return false;
    }

    private void OnEnable()
    {
        Selection.selectionChanged += OnSelectionChanged;
        EditorApplication.update += EditorUpdate;

        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
        nodeStyle.normal.textColor = Color.white;
        nodeStyle.fontStyle = FontStyle.Bold;
        nodeStyle.fontSize = 20;
        nodeStyle.alignment = TextAnchor.MiddleCenter;
        nodeStyle.padding = new RectOffset(20, 20, 20, 20);
        nodeStyle.border = new RectOffset(12, 12, 12, 12);

        comboStyle = new GUIStyle();
        comboStyle.normal.textColor = Color.white;
        comboStyle.fontStyle = FontStyle.Bold;
        comboStyle.fontSize = 50;
        comboStyle.padding = new RectOffset(10, 10, 10, 10);
        comboStyle.alignment = TextAnchor.MiddleCenter;

        verticalStyle = new GUIStyle();
        verticalStyle.alignment = TextAnchor.LowerCenter;
    }

    private void EditorUpdate()
    {
        if (hitCombo)
        {
            timeSinceLastCombo = EditorApplication.timeSinceStartup;
            hitCombo = false;
        }

        if (combo > 0 && timeSinceLastCombo + allowedComboTime < EditorApplication.timeSinceStartup)
        {
            LoseCombo();
        }
    }

    private void OnSelectionChanged()
    {
        ProceduralEditorGUIData newData = Selection.activeObject as ProceduralEditorGUIData;

        if (newData != null)
        {
            selectedData = newData;
            Repaint();
        }
    }

    private void OnGUI()
    {
        if (!selectedData)
        {
            EditorGUI.LabelField(new Rect(Vector2.zero, new Vector2(position.width, 50f)), "No Data Selected!");
            return;
        }

        e = Event.current;

        if (e.isMouse && e.button is 0 or 1 or 2)
            unprocessedInput = true;

        hoveringOverButton = false;

        GenerateRects();

        if (unprocessedInput && !buttonHeld && !hoveringOverButton)
        {
            LoseCombo();
        }

        if (e.isMouse && e.button is 0 or 1 or 2)
            buttonHeld = true;
        else
            buttonHeld = false;

        EditorGUILayout.BeginHorizontal("box");

        EditorGUI.BeginChangeCheck();
        float sliderWidth = (EditorGUIUtility.currentViewWidth) / 8f;
        float labelWidth = (EditorGUIUtility.currentViewWidth) / 8f;

        Rect controlRect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(new Rect(controlRect.position, new Vector2(labelWidth, controlRect.height)),
            "Max Pitch");

        controlRect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
        selectedData.minimumPitch =
            GUI.HorizontalSlider(new Rect(controlRect.position, new Vector2(sliderWidth, controlRect.height)),
                selectedData.minimumPitch, 0f, selectedData.maximumPitch);


        controlRect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(new Rect(controlRect.position, new Vector2(labelWidth, controlRect.height)),
            "Min Pitch");

        controlRect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
        selectedData.maximumPitch =
            GUI.HorizontalSlider(new Rect(controlRect.position, new Vector2(sliderWidth, controlRect.height)),
                selectedData.maximumPitch, selectedData.minimumPitch, 3f);

        controlRect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(new Rect(controlRect.position, new Vector2(labelWidth, controlRect.height)),
            "Max Volume");

        controlRect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
        selectedData.minimumVolume =
            GUI.HorizontalSlider(
                new Rect(controlRect.position, new Vector2(sliderWidth, controlRect.height)),
                selectedData.minimumVolume, 0f, selectedData.maximumVolume);

        controlRect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(new Rect(controlRect.position, new Vector2(labelWidth, controlRect.height)),
            "Min Volume");

        controlRect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
        selectedData.maximumVolume =
            GUI.HorizontalSlider(
                new Rect(controlRect.position, new Vector2(sliderWidth, controlRect.height)),
                selectedData.maximumVolume, selectedData.minimumVolume, 1f);


        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(selectedData);

        Rect progressBarRect = new Rect(0f, 30f, position.width, 10f);
        GUI.BeginGroup(progressBarRect);

        EditorGUI.ProgressBar(new Rect(0f, 0f, position.width, position.height), GetComboTimerValue(), String.Empty);
        Repaint();

        GUI.EndGroup();

        EditorGUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical(verticalStyle, new GUILayoutOption[] { GUILayout.Width(position.width / 5f) });
        GUILayout.FlexibleSpace();
        GUILayout.Label(combo.ToString(), comboStyle);
        if (GUILayout.Button("Reset Combo", nodeStyle))
        {
            combo = 0;
        }

        EditorGUILayout.EndVertical();

        //some stuff to experiment with:
        //- random cube count DONE
        //- window resizing DONE
        //- sounds? somewhere? somehow? dunno if thats possible in the editor DONE
        //- in any case, I guess I'm kind of creating a harmless virus? DONE?
    }

    private float GetComboTimerValue()
    {
        double remainingTime = timeSinceLastCombo + allowedComboTime - EditorApplication.timeSinceStartup;
        return Mathf.InverseLerp(0f, (float)allowedComboTime, (float)remainingTime);
    }

    private void LoseCombo()
    {
        if (combo != 0)
            GenerateAudioInstance(selectedData.comboMissClip);

        combo = 0;
        Repaint();
    }

    private void GenerateRects()
    {
        bool buttonHit = false;

        for (int i = selectedData.rects.Count - 1; i >= 0; i--)
        {
            Rect modifiedRect = GetModifiedRect(selectedData.rects[i]);

            if (GUI.Button(modifiedRect, "▢", nodeStyle))
            {
                GenerateAudioInstances(i);
                buttonHit = true;
            }

            if (modifiedRect.Contains(e.mousePosition))
                hoveringOverButton = true;
        }

        if (buttonHit)
            ProcessComboLogic();
    }

    private void GenerateAudioInstances(int i)
    {
        for (int j = 0; j < selectedData.comboClips.Length; j++)
        {
            ComboClip cc = selectedData.comboClips[j];
            if (combo >= cc.comboRequirement)
                GenerateAudioInstance(cc.clip, i);
        }

        selectedData.rects[i] = selectedData.GenerateRandomRect();
    }

    private void ProcessComboLogic()
    {
        unprocessedInput = false;
        hitCombo = true;
        combo++;

        if (!selectedData.addButtonOnCombo)
            return;

        int count = selectedData.addButtonsBasedOnComboCount ? combo : 1;


        if (!selectedData.removeButtonsOnCombo)
        {
            selectedData.rectCount += count;

            for (int i = 0; i < count; i++)
                selectedData.GenerateNewRect();
        }
        else
        {
            count = Mathf.Min(count, selectedData.rects.Count);

            selectedData.rectCount -= count;

            for (int i = 0; i < count; i++)
            {
                int random = Random.Range(0, selectedData.rects.Count);
                selectedData.rects.RemoveAt(random);
            }
        }
    }

    private void GenerateAudioInstance(AudioClip clip, int i)
    {
        GameObject instance = new GameObject();
        instance.name = clip.name + " Instance";
        EditorAudio audioInstance = instance.AddComponent<EditorAudio>();

        audioInstance.Play(clip, GetRectPitch(selectedData.rects[i]), GetRectVolume(selectedData.rects[i]));
    }

    private void GenerateAudioInstance(AudioClip clip)
    {
        GameObject instance = new GameObject();
        instance.name = clip.name + " Instance";
        EditorAudio audioInstance = instance.AddComponent<EditorAudio>();

        audioInstance.Play(clip);
    }

    private float GetRectPitch(Rect rect)
    {
        return Mathf.Lerp(selectedData.maximumPitch, selectedData.minimumPitch, rect.y);
    }

    private float GetRectVolume(Rect rect)
    {
        return Mathf.Lerp(selectedData.minimumVolume, selectedData.maximumVolume, rect.x);
    }

    private Rect GetModifiedRect(Rect selectedDataRect)
    {
        float x = selectedDataRect.x * position.width;
        float y = selectedDataRect.y * position.height;
        float width = 0;
        float height = 0;
        if (selectedData.isSquare)
        {
            float sizeModifier = Mathf.Min(position.height, position.width);
            width = selectedDataRect.width * sizeModifier;
            height = selectedDataRect.height * sizeModifier;
        }
        else
        {
            width = selectedDataRect.width * position.width;
            height = selectedDataRect.height * position.height;
        }

        return new Rect(x, y, width, height);
    }
}