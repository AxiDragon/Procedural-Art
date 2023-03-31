using System;
using UnityEngine;

[CreateAssetMenu]
public class ExpoInfo : ScriptableObject
{
    public Entry[] entries;

    [Serializable]
    public struct Entry
    {
        [Header("Email")]
        public string email;
        [Header("Hour + Minute!")]
        public string time;
    }
}
