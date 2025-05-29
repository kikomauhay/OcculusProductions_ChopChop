using UnityEngine;
using System;

public class SoundManager : Singleton<SoundManager> {

#region Members

    [SerializeField] private AudioSource _soundSource, _musicSource, _onboardingSource;
    [SerializeField] private Sound[] _sfx, _bgm, _onb;

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

        // Debug.Log($"Sound index: {_soundIndex}");
        if (_isDeveloperMode)
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
            Debug.LogError($"{title} not found!");
            return;
        }

        // adds the properties of the Sound to the AudioSource
        _soundSource.volume = s.Volume;
        _soundSource.pitch = s.Pitch;
        _soundSource.loop = s.Loop;
        _soundSource.clip = s.Clip;
        _soundSource.spatialBlend = 1f;

        // loops the sound if needed
        if (s.Loop)
            _soundSource.Play();

        else
            _soundSource.PlayOneShot(s.Clip);
    }
    public void PlayMusic(string title) 
    {
        Sound s = Array.Find(_bgm, i => i.name == title);

        if (s == null) 
        {
            Debug.LogError($"{title} not found!");
            return;
        }

        // adds the properties of the Sound to the AudioSource
        _musicSource.volume = s.Volume;
        _musicSource.pitch = s.Pitch;
        _musicSource.loop = s.Loop;
        _musicSource.clip = s.Clip;
        _musicSource.spatialBlend = 1f;

        _musicSource.Play();
    }
    public void PlayOnboarding(string title) 
    {
        Sound s = Array.Find(_onb, i => i.name == title);

        if (s == null) 
        {
            Debug.LogError($"{title} not found!");
            return;
        }

        // adds the properties of the Sound to the AudioSource
        _onboardingSource.volume = s.Volume;
        _onboardingSource.pitch = s.Pitch;
        _onboardingSource.loop = s.Loop;
        _onboardingSource.clip = s.Clip;
        _onboardingSource.spatialBlend = 1f;

        _onboardingSource.PlayOneShot(s.Clip);
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