using UnityEngine;
using System;

public class SoundManager : Singleton<SoundManager> {

#region Members

    [SerializeField] private AudioSource _soundSource, _musicSource;
    [SerializeField] private Sound[] _sfx, _bgm;

    [Header("Debugging")]
    [SerializeField] private bool _isDeveloperMode;
    private int _soundIndex = 0;

#endregion 

#region Methods

#region Unity

    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    protected override void Awake() 
    {
        base.Awake();
        
        Debug.Log(_soundIndex);
        Debug.Log($"{name} developer mode: {_isDeveloperMode}");
    }

#region Testing

    private void test()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && _isDeveloperMode)
        {
            _soundIndex++;
            Debug.Log(_soundIndex);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _isDeveloperMode)
            PlaySound(_sfx[_soundIndex].name);
    }

    private void Update() => test();

#endregion

#endregion
#region Public

#region Audio Playback

    public void TogglePauseMusic() 
    {
        if (GameManager.Instance.IsPaused)   
            _musicSource.Pause();

        else  
            _musicSource.UnPause();
    }
    public void StopAllSounds()
    {
        _soundSource.Stop();
        _musicSource.Stop();
    }
    public void PlaySound(string title) 
    {    
        Sound s = Array.Find(_sfx, i => i.name == title);

        if (s == null) 
        {
            Debug.LogError("Sound not found!");
            return;
        }
        else 

        // loops the sound if needed
        if (s.Loop) _soundSource.Play();
        else        _soundSource.PlayOneShot(s.Clip);
    }
    public void PlayMusic(string title) 
    {
        Sound s = Array.Find(_bgm, i => i.name == title);

        if (s == null) 
        {
            Debug.LogError("Music not found!");
            return;
        }

        _musicSource.Play();
    }
    public void StopMusic() => _musicSource.Stop();
    public void StopSound() => _soundSource.Stop();

#endregion
#region Audio Balancing

    public void ToggleMute() 
    {
        _soundSource.mute = !_soundSource.mute;
        _musicSource.mute = !_musicSource.mute;
    }
    public void SetSoundVolume(float v) => _soundSource.volume = v;
    public void SetSoundPitch(float p) => _soundSource.pitch= p;
    public void SetMusicVolume(float v) => _musicSource.volume = v;
    public void SetMusicPitch(float p) => _musicSource.pitch= p;

#endregion

#endregion

#endregion
}