using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(AudioSource))]
public class AudioRecorder : MonoBehaviour
{
    public int length = 256;
    private static List<float> outputValuesList = new();
    public int channel = 0;
    public bool sendDebug;
    private static bool recording;
    // private float[] previousOutputData;
    [HideInInspector] [TextArea] public static string savingLocation = "";

    [HideInInspector] public AudioClip recordedClip;
    private AudioSource audioSource;

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        EditorApplication.update += EditorUpdate;
    }

    private void Start()
    {
        if (Application.IsPlaying(this))
            Destroy(this);
    }

    private void EditorUpdate()
    {
        if (!recording)
            return;

        float[] outputData = new float[length];

        AudioListener.GetOutputData(outputData, channel);

        // if (previousOutputData.Length > 0 && CheckIfSameSamples(outputData))
        //     return;

        // previousOutputData = outputData;

        for (int i = 0; i < outputData.Length; i++)
        {
            float outputValue = outputData[i];
            outputValuesList.Add(outputValue);
        }

        if (sendDebug)
            Debug.Log(outputData[0]);
    }

    // private bool CheckIfSameSamples(float[] outputData)
    // {
    //     for (int i = 0; i < length; i++)
    //     {
    //         if (!Mathf.Approximately(previousOutputData[i], outputData[i]))
    //             return false;
    //     }
    //     
    //     if (sendDebug)
    //         Debug.Log("Same Samples Found!");
    //     return true;
    // }

    [ContextMenu("Start Recording")]
    public void StartRecording()
    {
        recording = true;
        outputValuesList.Clear();
    }

    [ContextMenu("Stop Recording")]
    public void StopRecording()
    {
        recording = false;
        recordedClip = AudioClip.Create("Recording", outputValuesList.Count, 1, AudioSettings.outputSampleRate, false);
        recordedClip.SetData(outputValuesList.ToArray(), 0);
        
        SaveRecording();
    }

    [ContextMenu("Play Recording")]
    public void PlayRecording()
    {
        audioSource.clip = recordedClip;
        audioSource.Play();
    }

    [ContextMenu("Save Recording")]
    private void SaveRecording()
    {
        if (recordedClip)
        {
            SavWav.Save("Procedural Piano " + GetConvertedTimeString(), recordedClip, savingLocation);
        }
        else
        {
            Debug.LogWarning("There's no recorded audio clip!");
        }
    }

    private string GetConvertedTimeString()
    {
        string rawDate = DateTime.Now.ToShortDateString() + "-" + DateTime.Now.ToLongTimeString();
        string date = String.Join("-", rawDate.Split(System.IO.Path.GetInvalidFileNameChars()));

        return date;
    }
}