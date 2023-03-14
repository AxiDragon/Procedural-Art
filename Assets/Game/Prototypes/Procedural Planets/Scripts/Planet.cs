using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;
    [Range(2, 256)] public int resolution = 10;

    [HideInInspector] public bool shapeSettingsFoldout;
    [HideInInspector] public bool colorSettingsFoldout;
    public bool autoUpdate = true;
    public enum FaceRenderMask {All, Top, Bottom, Left, Right, Front, Back}
    public FaceRenderMask faceRenderMask;

    [SerializeField] [HideInInspector] private MeshFilter[] meshFilters;
    private TerrainFace[] terrainFaces;

    private ShapeGenerator shapeGenerator = new ShapeGenerator();
    private ColorGenerator colorGenerator = new ColorGenerator();

    void Initialize()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        colorGenerator.UpdateSettings(colorSettings);
        
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }

        terrainFaces = new TerrainFace[6];
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("Mesh");
                meshObj.transform.parent = transform;

                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    public void OnShapeSettingsUpdated()
    {
        if (!autoUpdate)
            return;
        
        Initialize();
        GenerateMesh();
    }

    public void OnColorSettingsUpdated()
    {
        if (!autoUpdate)
            return;
        
        Initialize();
        GenerateColors();
    }

    void GenerateMesh()
    {
        for (int i = 0; i < terrainFaces.Length; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                TerrainFace face = terrainFaces[i];
                face.ConstructMesh();
            }
        }
        
        colorGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    void GenerateColors()
    {
        for (int i = 0; i < terrainFaces.Length; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                TerrainFace face = terrainFaces[i];
                face.UpdateUVs(colorGenerator);
            }
        }
        
        colorGenerator.UpdateColors();
    }
}