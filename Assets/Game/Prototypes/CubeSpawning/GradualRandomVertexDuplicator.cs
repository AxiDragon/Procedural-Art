using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GradualRandomVertexDuplicator : MonoBehaviour
{
    [Range(0f, 1f)] public float chance;
    public float averageChancePerSecond = 1f;
    [Range(0f, 1f)] public float childChanceFalloff;
    [Range(0f, 1f)] public float childScale;
    public int maxSpawns = 6;
    public int spawned;
    public float childOffset = 1f;
    private Mesh mesh;
    private Vector3[] vertices;

    private void Awake()
    {
        if (TryGetComponent(out MeshFilter mf))
            mesh = mf.mesh;
    }

    private void Start()
    {
        vertices = mesh.vertices;
    }

    private void Update()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            if (spawned >= maxSpawns)
            {
                Destroy(this);
                break;
            }
            
            if (Random.value > chance * (Time.deltaTime * averageChancePerSecond))
                continue;

            spawned++;
            
            GradualRandomVertexDuplicator rvd = Instantiate(this, transform);
            Transform t = rvd.transform;
            t.localPosition = vertices[i] * childOffset;
            t.localScale = Vector3.one * childScale;
            rvd.chance *= 1f - childChanceFalloff;
        }
    }
}