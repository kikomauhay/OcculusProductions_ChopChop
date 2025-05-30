using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "Scriptable Objects/Sound")]
public class Sound : ScriptableObject
{
#region Properties

    public AudioSource Source { get; set; }
    public AudioClip Clip => _clip;
    public bool Loop => _loop;
    public float Pitch => _pitch; 
    public float Volume => _volume; 

#endregion

#region SerializeField
    
    [SerializeField] private AudioClip _clip;
    [SerializeField] private bool _loop;
    [SerializeField, Range(0f, 1f)] public float _volume = 0.8f;
    [SerializeField, Range(1f, 3f)] private float _pitch = 1f;

#endregion
}