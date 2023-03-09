using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshGenerator))]
public class MeshGenSoundInfluence : MonoBehaviour
{
    private float[] audioSpectrum;
    private MeshGenerator meshGen;

    [Header("Make sure that this is a power of 2")]
    public int audioSpectrumWidth;

    [SerializeField] private float intensity = 1f;

    void Awake()
    {
        meshGen = GetComponent<MeshGenerator>();
    }

    private void Start()
    {
        audioSpectrum = new float[audioSpectrumWidth];
    }

    void Update()
    {
        AudioListener.GetSpectrumData(audioSpectrum, 0, FFTWindow.Hamming);

        if (audioSpectrum is { Length: > 0 })
        {
            print(audioSpectrum.Average() * intensity);
            meshGen.xScroll += audioSpectrum.Average() * intensity;
        }
    }
}