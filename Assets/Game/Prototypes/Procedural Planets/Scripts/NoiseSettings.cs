using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NoiseSettings
{
    public enum FilterType
    {
        Simple,
        Rigid
    };

    public FilterType filterType;
    
    [ConditionalHide("filterType", 0)]
    public SimpleNoiseSettings simpleNoiseSettings;
    [ConditionalHide("filterType", 1)]
    public RigidNoiseSettings rigidNoiseSettings;
    
    [Serializable]
    public class SimpleNoiseSettings
    {
        public float strength = 1f;
        public float roughness = 2f;
        [Range(1, 8)] public int numLayers = 1;
        public float baseRoughness = 1f;
        public float persistence = .5f;
        public float minValue;
        public Vector3 center;
    }

    [Serializable]
    public class RigidNoiseSettings : SimpleNoiseSettings
    {
        [Range(0f, 1f)] public float weightMultiplier = .8f;
    }
}