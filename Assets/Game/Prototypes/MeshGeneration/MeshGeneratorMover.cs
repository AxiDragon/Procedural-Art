using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshGenerator))]
public class MeshGeneratorMover : MonoBehaviour
{
    private MeshGenerator meshGen;
    public float zSpeed = 1f;
    public float xSpeed = 1f;
    
    void Awake()
    {
        meshGen = GetComponent<MeshGenerator>();
    }

    void Update()
    {
        meshGen.zScroll = Time.time * zSpeed;
        meshGen.xScroll = Time.time * xSpeed;
    }
}
