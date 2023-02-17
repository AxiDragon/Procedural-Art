using UnityEngine;
using Random = UnityEngine.Random;

public class RandomVertexDuplicator : MonoBehaviour
{
    [Range(0, 1)] public float chance;
    [Range(0, 1)] public float childChanceFalloff;
    [Range(0, 1)] public float childScale;
    public float childOffset = 1f;
    private Mesh mesh;

    private void Awake()
    {
        if (TryGetComponent(out MeshFilter mf))
            mesh = mf.mesh;
    }

    private void Start()
    {
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            if (CubeSpawnerLimiter.cubesSpawned >= CubeSpawnerLimiter.cubeLimit)
                break;
            
            if (Random.value > chance)
                continue;

            CubeSpawnerLimiter.cubesSpawned++;
            
            RandomVertexDuplicator rvd = Instantiate(this, transform);
            Transform t = rvd.transform;
            t.localPosition = vertices[i] * childOffset;
            t.localScale = Vector3.one * childScale;
            rvd.chance *= 1f - childChanceFalloff;
        }
    }
}