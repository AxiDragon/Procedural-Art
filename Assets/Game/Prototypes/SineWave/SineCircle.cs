using System.Collections.Generic;
using UnityEngine;

public class SineCircle : MonoBehaviour
{
    private List<Transform> cubes = new();
    public int length;
    public float cubeOffset = 1f;
    public float offset = .1f;
    public float amplitude = 1f;
    public float speed = 1f;

    void Start()
    {
        for (int i = 0; i < length; i++)
        {
            Transform cube = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            cube.position = Vector3.right * i * cubeOffset;
            cubes.Add(cube);
        }
    }

    void Update()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            cubes[i].position = new Vector3(i * cubeOffset, Mathf.Sin(Time.time * speed + i * offset) * amplitude,
                Mathf.Cos(Time.time * speed + i * offset) * amplitude);
        }
    }
}