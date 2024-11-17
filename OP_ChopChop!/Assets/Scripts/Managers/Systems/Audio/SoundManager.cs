using UnityEngine;
using System;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource SoundSource;
    public Sound[] Sounds;


    int i = 0;

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();

    void Update() {

        // test 
        if (Input.GetKeyDown(KeyCode.Return)) {
            SoundSource.PlayOneShot(Sounds[i].clip);
            Debug.Log($"{Sounds[i].name}");
        }
        
        if (Input.GetKeyDown(KeyCode.Space)) i++;
    }

    public void PlaySound(string title)
    {
        Sound s = Array.Find(Sounds, i => i.name == title);

        if (s != null)
        {
            SoundSource.volume = s.volume;
            SoundSource.clip = s.clip;
            SoundSource.loop = s.loop;
            SoundSource.spatialBlend = 1f;
            SoundSource.PlayOneShot(s.clip);
        }
        else Debug.LogError("Sound not found!");
    }

    public void ToggleMute() => SoundSource.mute = !SoundSource.mute;
    public void SetSoundVolume(float v) => SoundSource.volume = v;
}