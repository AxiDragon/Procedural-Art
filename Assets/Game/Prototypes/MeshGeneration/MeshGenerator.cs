using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    private Mesh mesh;

    private Vector3[] vertices;
    private Color[] colors;
    public Gradient gradient;
    private int[] triangles;

    public bool enableGizmos;
    public int xSize = 20;
    public int zSize = 20;
    public float xScroll;
    public float zScroll;
    public float xPerlinNoiseZoom = 0.3f;
    public float zPerlinNoiseZoom = 0.3f;
    public float perlinNoiseIntensity = .2f;

    private float minTerrainHeight = Mathf.Infinity;
    private float maxTerrainHeight = Mathf.NegativeInfinity;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
    }

    private void Update()
    {
        UpdateVertices();
        UpdateMesh();
    }

    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }
    
    private void CreateShape()
    {
        UpdateVertices();

        triangles = new int[xSize * zSize * 6];

        for (int vert = 0, tris = 0, z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[0 + tris] = vert;
                triangles[1 + tris] = vert + xSize + 1;
                triangles[2 + tris] = vert + 1;
                triangles[3 + tris] = vert + 1;
                triangles[4 + tris] = vert + xSize + 1;
                triangles[5 + tris] = vert + xSize + 2;

                vert++;
                tris += 6;
            }

            vert++;
        }

        colors = new Color[vertices.Length]; 
        
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
    }

    private void UpdateVertices()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * xPerlinNoiseZoom / 10f + xScroll, z * zPerlinNoiseZoom / 10f + zScroll) *
                          perlinNoiseIntensity;
                vertices[i] = new Vector3(x, y, z);

                if (y > maxTerrainHeight)
                    maxTerrainHeight = y;

                if (y < minTerrainHeight)
                    minTerrainHeight = y;
                
                i++;
            }
        }
        
        colors = new Color[vertices.Length]; 
        
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (vertices == null || vertices.Length == 0 || !enableGizmos)
            return;

        Gizmos.color = Color.red;

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
}