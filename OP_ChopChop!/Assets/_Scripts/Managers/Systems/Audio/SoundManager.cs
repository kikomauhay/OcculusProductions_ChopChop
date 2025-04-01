using UnityEngine;
using System;

public class SoundManager : Singleton<SoundManager>
{
#region Members
    
    public AudioSource SoundSource, MusicSource;
    public Sound[] EquipmentSounds, ApplianceSounds, FoodSounds;
    public Sound[] GameSounds, VFXSounds, CustomerSounds;

#endregion

#region Unity

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();

#endregion

    /* public void PlaySound(string title)
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
    */

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
    
#endregion
}