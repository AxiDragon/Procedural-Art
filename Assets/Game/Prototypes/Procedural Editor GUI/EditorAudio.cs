using UnityEditor;
using UnityEngine;

public class EditorAudio : MonoBehaviour
{
    private double delay = double.MaxValue;

    private void Start()
    {
        Destroy(gameObject);
    }
#if UNITY_EDITOR
    private void OnEnable()
    {
        Debug.Log("Attached.");
    }

    private void OnDisable()
    {
        Debug.Log("Disabled.");
        EditorApplication.update -= OnEditorUpdate;
    }

    private void OnEditorUpdate()
    {
        if (EditorApplication.timeSinceStartup > delay)
        {
            EditorApplication.update -= OnEditorUpdate;
            DestroyImmediate(gameObject);
        }
    }
#endif

    public void Play(AudioClip clip, float pitch = 1f, float volume = 1f, bool destroyOnEnd = true)
    {
        EditorApplication.update += OnEditorUpdate;

        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f;
        audioSource.clip = clip;
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.Play();

        if (destroyOnEnd)
        {
            float addedDelay = Mathf.Min(clip.length / pitch, 5f);
            delay = addedDelay + EditorApplication.timeSinceStartup;
        }
    }
}