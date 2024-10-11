using UnityEngine;
using System;

public enum SoundType { FOOD, MISC, GAME, BGM }

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource SoundSource, MusicSource;
    public Sound[] Food, Game, BGM, Misc;

    protected override void Awake() { base.Awake(); }

    public void PlaySound(string name, SoundType type)
    {
        Sound s = null;

        switch (type)
        {
            case SoundType.MISC:
                s = Array.Find(Misc, i => i.name == name);
                break;
            case SoundType.GAME:
                s = Array.Find(Game, i => i.name == name);
                break;
            case SoundType.FOOD:
                s = Array.Find(Food, i => i.name == name);
                break;
            case SoundType.BGM:
                Debug.LogError("Wrong mode for the method!");
                break;
            default:
                Debug.LogError("Invalid option");
                break;
        }

        if (s != null)
        {
            SoundSource.volume = s.volume;
            SoundSource.clip = s.clip;
            SoundSource.loop = s.loop;
            SoundSource.PlayOneShot(s.clip);
        }
        else Debug.LogError("Sound not found!");
    }

    public void ToggleMute() 
    { 
        SoundSource.mute = !SoundSource.mute;
        MusicSource.mute = !MusicSource.mute;
    }

    public void SetSoundVolume(float v) { SoundSource.volume = v; }
    public void SetMusicVolume(float v) { MusicSource.volume = v; }
}

[Serializable] 
public class Sound
{
    public string name;
    public AudioClip clip;
    public bool loop;

    [Range(0f, 1f)] public float volume = 0.8f;
    [HideInInspector] public AudioSource source;
}