using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    private ShapeSettings settings;
    private INoiseFilter[] noiseFilters;
    public MinMax elevationMinMax;
    
    public void UpdateSettings(ShapeSettings settings)
    {
        this.settings = settings;
        
        noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
        for (int i = 0; i < settings.noiseLayers.Length; i++)
        {
            var noiseFilter = settings.noiseLayers[i].noiseSettings;
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(noiseFilter);
        }

        elevationMinMax = new MinMax();
    }

    public float CalculateUnscaledElevation(Vector3 pointOnUnitSphere)
    {
        float elevation = 0;
        float firstLayerValue = 0f;

        if (noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (settings.noiseLayers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }

        for (int i = 1; i < noiseFilters.Length; i++)
        {
            if (settings.noiseLayers[i].enabled)
            {
                float mask = settings.noiseLayers[i].useFirstLayerAsMask ? firstLayerValue : 1;
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }

        elevationMinMax.AddValue(elevation);
        
        return elevation;
    }

    public float GetScaledElevation(float unscaledElevation)
    {
        float elevation = Mathf.Max(0f, unscaledElevation);
        elevation = settings.planetRadius * (1 + elevation);
        return elevation;
    }
}