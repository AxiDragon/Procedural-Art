using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Color Settings")]
public class ColorSettings : ScriptableObject
{
    public Material planetMaterial;
    public Gradient oceanGradient;
    public BiomeColorSettings biomeColorSettings;

    [Serializable]
    public class BiomeColorSettings
    {
        public NoiseSettings noise;
        public float noiseOffset;
        public float noiseStrength;
        [Range(0, 1)]
        public float blendAmount;
        public Biome[] biomes;

        
        [Serializable]
        public class Biome
        {
            public Gradient biomeGradient;
            public Color tint;
            [Range(0, 1)] public float tintPercent;
            [Range(0, 1)] public float startHeight;
        }
    }
}