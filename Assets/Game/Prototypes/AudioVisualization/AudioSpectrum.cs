using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpectrum : MonoBehaviour
{
    private float[] audioSpectrum;

    [Header("Make sure that this is a power of 2")]
    public int audioSpectrumWidth;

    public float timeScale = 1f;
    public bool additive;
    private List<Transform> cubes = new();
    public static float spectrumValue { get; private set; }
    public MovementType movementType;
    [SerializeField] private float intensity = 1f;

    public enum MovementType
    {
        Vertical,
        Circular,
        One
    }

    private void Start()
    {
        audioSpectrum = new float[audioSpectrumWidth];

        for (int i = 0; i < audioSpectrum.Length; i++)
        {
            Transform cube = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            cube.position = new Vector3(i * (128f / audioSpectrumWidth), 0f, 0f);
            cubes.Add(cube);
        }
    }

    void Update()
    {
        Time.timeScale = timeScale;

        AudioListener.GetSpectrumData(audioSpectrum, 0, FFTWindow.Hamming);

        if (audioSpectrum is { Length: > 0 })
        {
            for (int i = 0; i < cubes.Count; i++)
            {
                float f = audioSpectrum[i] * intensity * 100f;
                Vector3 v = Vector3.zero;

                switch (movementType)
                {
                    case MovementType.Vertical:
                        v.y = f;
                        break;
                    case MovementType.One:
                        v = Vector3.one * f;
                        break;
                    case MovementType.Circular:
                        v = new Vector3(0f, Mathf.Sin(f) * intensity, Mathf.Cos(f) * intensity);
                        break;
                }

                v.x = i * (128f / audioSpectrumWidth);

                cubes[i].position = additive ? new Vector3(0f, cubes[i].position.y, cubes[i].position.z) + v : v;
            }
        }
    }
}