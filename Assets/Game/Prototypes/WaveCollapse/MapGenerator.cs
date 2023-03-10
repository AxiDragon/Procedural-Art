using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Vector2Int mapDimensions;
    [SerializeField] private MapColorModule[] mapModules;
    [SerializeField] private float offset = 8f;

    private void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        for (int x = 0; x < mapDimensions.x; x++)
        {
            for (int z = 0; z < mapDimensions.y; z++)
            {
                MapColorModule randomColorModule = mapModules[Random.Range(0, mapModules.Length)];
                Instantiate(randomColorModule, new Vector3(x * offset, 0f,z * offset), Quaternion.identity);
            }
        }
    }
}
