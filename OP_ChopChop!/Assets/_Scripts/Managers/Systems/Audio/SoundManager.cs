using UnityEngine;
using System;

public class SoundManager : Singleton<SoundManager>
{
#region Members
    
    public AudioSource SoundSource, MusicSource;
    
    public Sound[] EquipmentSounds, ApplianceSounds, FoodSounds;
    public Sound[] GameSounds, VFXSounds, CustomerSounds;
    public Sound[] TutorialSounds;

#endregion

#region Unity

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();

#endregion

    public void PlaySound(string title, SoundGroup type) 
    {
        Sound s = null;

        switch (type)
        {
            case SoundGroup.EQUIPMENT:
                s = Array.Find(EquipmentSounds, i => i.name == title);
                break;

            case SoundGroup.APPLIANCES:
                s = Array.Find(ApplianceSounds, i => i.name == title);
                break;

            case SoundGroup.FOOD:
                s = Array.Find(FoodSounds, i => i.name == title);
                break;

            case SoundGroup.GAME:
                s = Array.Find(GameSounds, i => i.name == title);
                break;

            case SoundGroup.VFX:
                s = Array.Find(VFXSounds, i => i.name == title);
                break;

            case SoundGroup.CUSTOMER:
                s = Array.Find(CustomerSounds, i => i.name == title);
                break;

            case SoundGroup.TUTORIAL:
                s = Array.Find(TutorialSounds, i => i.name == title);
                break;

            default:
                Debug.LogError("Wrong SoundGroup!");
                break;
        }

        if (s != null)
        {
            SoundSource.volume = s.volume;
            SoundSource.clip = s.clip;
            SoundSource.loop = s.loop;
            SoundSource.spatialBlend = 1f;
            SoundSource.PlayOneShot(s.clip);

            if (s.loop) 
            { 
                SoundSource.Play();
                return;
            }
            SoundSource.PlayOneShot(s.clip);
        }
        else Debug.LogError("Sound not found!");
    }

#region Audio_Balancing

    public void ToggleMute() 
    {
        SoundSource.mute = !SoundSource.mute;
        MusicSource.mute = !MusicSource.mute;
    }
    public void SetSoundVolume(float v) => SoundSource.volume = v;
    public void SetMusicVolume(float v) => MusicSource.volume = v;
    public void StopMusic() => MusicSource.Stop();
    public void StopAllAudio()
    {
        StopMusic();
        SoundSource.Stop();
    }

    public bool SoundPlaying() => SoundSource.isPlaying;
    public bool MusicPlaying() => MusicSource.isPlaying;
    public bool AudioPlaying() => SoundPlaying() && MusicPlaying();
    
#endregion
}
