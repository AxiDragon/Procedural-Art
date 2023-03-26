using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ProceduralEditorGUIData : ScriptableObject
{
    [Header("Audio Clips")] public ComboClip[] comboClips;
    public AudioClip comboMissClip;
    [Header("Audio Settings")]
    [Range(0f, 3f)] public float minimumPitch;
    [Range(0f, 3f)] public float maximumPitch;
    [Range(0f, 2f)] public float minimumVolume;
    [Range(0f, 2f)] public float maximumVolume;
    [Header("Rectangles")] public List<Rect> rects = new();
    public int rectCount;
    [Header("Position")] [Range(0f, 1f)] public float xRange;
    [Range(0f, 1f)] public float yRange;
    [Header("Size")] public bool isSquare;
    [Range(0f, 1f)] [HideInInspector] public float defaultWidthRange;
    [Range(0f, 1f)] [HideInInspector] public float defaultHeightRange;
    [Range(0f, 1f)] [HideInInspector] public float defaultSizeRange;
    [Range(0f, 1f)] public float sizeDifferenceRange;
    [Header("Funnies")] [HideInInspector] public bool addButtonOnCombo;
    [HideInInspector] public bool addButtonsBasedOnComboCount;
    [HideInInspector] public bool removeButtonsOnCombo;

    public Rect GenerateRandomRect()
    {
        float x = Random.Range(0f, xRange);
        float y = Random.Range(0f, yRange);
        float width = defaultHeightRange * Random.Range(1f - sizeDifferenceRange, 1f + sizeDifferenceRange);
        float height = defaultWidthRange * Random.Range(1f - sizeDifferenceRange, 1f + sizeDifferenceRange);
        float size = defaultSizeRange * Random.Range(1f - sizeDifferenceRange, 1f + sizeDifferenceRange);

        if (isSquare)
            return new Rect(x, y, size, size);

        return new Rect(x, y, width, height);
    }

    public void GenerateNewRect()
    {
        rects.Add(GenerateRandomRect());
    }
}