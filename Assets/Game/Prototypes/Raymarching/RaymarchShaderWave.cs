using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaymarchShaderWave : MonoBehaviour
{
    private Material mat;
    public float speed;
    public float torusWidthOffset;
    public float torusRingWidthOffset;
    public float holeWidthOffset;
    public float sphereRadiusOffset;
    private static readonly int torusWidth = Shader.PropertyToID("_Torus_Width");
    private static readonly int torusHoleWidth = Shader.PropertyToID("_Torus_Hole_Width");
    private static readonly int boxHoleWidth = Shader.PropertyToID("_Box_Hole_Width");
    private static readonly int sphereRadius = Shader.PropertyToID("_Sphere_Radius");

    private void Awake()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        mat.SetFloat(torusWidth, Mathf.Sin(Time.time * speed + torusWidthOffset).Remap(-1f,1f, .3f, .7f));
        mat.SetFloat(torusHoleWidth, Mathf.Sin(Time.time * speed + torusRingWidthOffset).Remap(-1f,1f, .05f, .2f));
        mat.SetFloat(boxHoleWidth, Mathf.Sin(Time.time * speed + holeWidthOffset).Remap(-1f,1f, 1f, 1f));
        mat.SetFloat(sphereRadius, Mathf.Sin(Time.time * speed + sphereRadiusOffset).Remap(-1f,1f, .05f, .4f));
    }
}
