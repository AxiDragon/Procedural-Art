using UnityEngine;

public class CubeSpawnerLimiter : MonoBehaviour
{
    public int cubesAllowed;
    public static int cubeLimit;
    public static int cubesSpawned;

    private void Awake()
    {
        cubeLimit = cubesAllowed;
        cubesSpawned = 0;
    }
}
