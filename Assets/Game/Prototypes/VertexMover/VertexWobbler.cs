using System.Collections.Generic;
using UnityEngine;

public class VertexWobbler : MonoBehaviour
{
    private Mesh mesh;
    private readonly List<Vector3> basePositions = new();
    private Vector3[] vertices;
    public float speed = 1f;
    public float amplitude = 1f;
    public float offset = 1f;
    public Vector3 direction;
    public bool directionViaWave;
    public TrigonometricFunction trigonometricFunction;

    public enum TrigonometricFunction
    {
        Sine,
        Cosine,
        Tangent
    }
    
    private void Awake()
    {
        if (TryGetComponent(out MeshFilter mf))
            mesh = mf.mesh;

        if (TryGetComponent(out SkinnedMeshRenderer smr))
            mesh = smr.sharedMesh;
    }

    private void Start()
    {
        vertices = mesh.vertices;
        
        for (int i = 0; i < vertices.Length; i++)
        {
            basePositions.Add(vertices[i]); 
        }   
    }

    void Update()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            float wave = 0f;
            Vector3 moveDirection = direction;

            switch (trigonometricFunction)
            {
                case TrigonometricFunction.Sine:
                    wave = Mathf.Sin(Time.time * speed + basePositions[i].y * offset);
                    
                    if (directionViaWave)
                        moveDirection = Vector3.one * Mathf.Sin(Time.time * speed);
                    
                    break;
                case TrigonometricFunction.Cosine:
                    wave = Mathf.Cos(Time.time * speed + basePositions[i].y * offset);
                    
                    if (directionViaWave)
                        moveDirection = Vector3.one * Mathf.Cos(Time.time * speed);
                    
                    break;
                case TrigonometricFunction.Tangent:
                    wave = Mathf.Tan(Time.time * speed + basePositions[i].y * offset);
                    
                    if (directionViaWave)
                        moveDirection = Vector3.one * Mathf.Tan(Time.time * speed);
                    
                    break;
            }

            vertices[i] = basePositions[i] + moveDirection * (wave * amplitude);
        }
        
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }
}
