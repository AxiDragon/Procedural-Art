using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWave : MonoBehaviour
{
    public float offset = 0f;
    public float amplitude = 1f;
    public float speed = 1f;
    public Vector3 direction;
    private Vector3 basePosition;

    private void Start()
    {
        basePosition = transform.position;
    }

    void Update()
    {
        transform.position = basePosition + direction * (Mathf.Sin(Time.time * speed + offset) * amplitude);
    }
}
