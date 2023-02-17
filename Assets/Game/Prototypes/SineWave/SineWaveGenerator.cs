using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveGenerator : MonoBehaviour
{
    public int waveCount;
    public float waveSpeed = 1f;
    public Vector3 positionOffsetPerWave;
    public float offsetPerWave = .2f;
    public float amplitudePerWave = .2f;
    public float amplitudePerWaveOverTime = 0f;
    public float speedPerWave = .2f;
    public Vector3 direction;
    public Vector3 waveScale;
    private List<SineWave> sineWaves = new();
    
    void Start()
    {
        GenerateWaves();
    }

    private void Update()
    {
        for (int i = sineWaves.Count - 1; i >= 0; i--)
        {
            SineWave instance = sineWaves[i];
            instance.direction = direction;
            instance.offset = offsetPerWave * i;
            instance.amplitude = 1 + amplitudePerWave * i;
            instance.amplitude += amplitudePerWaveOverTime * Time.deltaTime;
            instance.speed = waveSpeed + speedPerWave * i;
            instance.transform.localScale = waveScale;
        }
    }

    private void GenerateWaves()
    {
        for (int i = 0; i < waveCount; i++)
        {
            GenerateWave(i, positionOffsetPerWave * i);
        }
    }

    private void GenerateWave(int i, Vector3 positionOffset)
    {
        SineWave instance = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<SineWave>();
        instance.transform.Translate(positionOffset);
        instance.gameObject.AddComponent<Rigidbody>().mass = 50f;
        sineWaves.Add(instance);
    }

    [ContextMenu("Regenerate Waves")]
    public void RegenerateWaves()
    {
        for (int i = sineWaves.Count - 1; i >= 0; i--)
        {
            Destroy(sineWaves[i].gameObject);
        }
        
        sineWaves.Clear();
        
        GenerateWaves();
    }
}
