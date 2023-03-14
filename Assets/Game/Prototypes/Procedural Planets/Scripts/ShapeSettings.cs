using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shape Settings")]
public class ShapeSettings : ScriptableObject
{
    public float planetRadius = 1f;
    public NoiseLayer[] noiseLayers;

    [Serializable]
    public class NoiseLayer
    {
        public NoiseSettings noiseSettings;
        public bool useFirstLayerAsMask;
        public bool enabled;
    }
}
