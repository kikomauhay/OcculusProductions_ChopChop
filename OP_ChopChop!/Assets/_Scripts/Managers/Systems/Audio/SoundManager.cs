using UnityEngine;
using System;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource SoundSource;
    public Sound[] Sounds;
    int i = 0; // testing

#region Unity_Methods

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();

#endregion

#region Public

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

#endregion

#region Testing

    void test()
    {
        // plays all the sounds in order
        if (Input.GetKeyDown(KeyCode.Return)) {
            SoundSource.PlayOneShot(Sounds[i].clip);
            Debug.Log($"{Sounds[i].name}");
        }
        
        // increment index 
        if (Input.GetKeyDown(KeyCode.Space)) i++;

        // random slice variants
        if (Input.GetKeyDown(KeyCode.Escape))
            PlaySound(UnityEngine.Random.value > 0.5f ? "fish slice 01" : "fish slice 02"); 
    }

#endregion

}