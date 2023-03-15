using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    private ShapeGenerator shapeGenerator;
    private Mesh mesh;
    private int resolution;
    private Vector3 localUp;
    private Vector3 axisA;
    private Vector3 axisB;

    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        //squares per 'edge' = res - 1, each square has 2 triangles, each triangle has 3 point (2 * 3 = 6)
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triangleIndex = 0;
        Vector2[] uv = (mesh.uv.Length == vertices.Length) ? mesh.uv : new Vector2[vertices.Length];

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2f * axisA + (percent.y - .5f) * 2f * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                float unscaledElevation = shapeGenerator.CalculateUnscaledElevation(pointOnUnitSphere);
                vertices[i] = pointOnUnitSphere * shapeGenerator.GetScaledElevation(unscaledElevation);
                uv[i].y = unscaledElevation;

                if (x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triangleIndex] = i;
                    triangles[triangleIndex + 1] = i + resolution + 1;
                    triangles[triangleIndex + 2] = i + resolution;

                    triangles[triangleIndex + 3] = i;
                    triangles[triangleIndex + 4] = i + 1;
                    triangles[triangleIndex + 5] = i + resolution + 1;

                    triangleIndex += 6;
                }
            }
        }

        mesh.Clear();

        mesh.vertices = vertices;
        mesh.normals = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        mesh.uv = uv;
    }

    public void UpdateUVs(ColorGenerator colorGenerator)
    {
        Vector2[] uv = mesh.uv;
        
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;

                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2f * axisA + (percent.y - .5f) * 2f * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                uv[i].x = colorGenerator.BiomePercentFromPoint(pointOnUnitSphere);
            }
        }

        mesh.uv = uv;
    }
}