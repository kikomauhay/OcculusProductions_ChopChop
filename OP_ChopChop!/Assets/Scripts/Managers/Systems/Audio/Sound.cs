using UnityEngine;

[System.Serializable, CreateAssetMenu(menuName = "Sound")]
public class Sound : ScriptableObject
{
    public AudioClip clip;
    public bool loop;

    [Range(0f, 1f)] public float volume = 0.8f;
    [HideInInspector] public AudioSource source;
}