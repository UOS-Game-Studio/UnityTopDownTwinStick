using System;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Events;

public class SettingsControl : MonoBehaviour
{
    public UnityEvent onSettingsChanged = new UnityEvent();
    
    private float _sfxVolume = 100.0f;
    private float _musicVolume = 100.0f;
    private bool _muted;

    private const string SfxKey = "sfxVolume";
    private const string MusicKey = "musicVolume";
    private const string MuteKey = "muted";

    public float SfxVolume => _sfxVolume;
    public float MusicVolume => _musicVolume;
    
    public float SfxDb => math.remap(0.0f, 100.0f, -80.0f, 0.0f, _sfxVolume);
    public float MusicDb => math.remap(0.0f, 100.0f, -80.0f, 0.0f, _musicVolume);
    
    public bool IsMuted => _muted;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (PlayerPrefs.HasKey(SfxKey))
        {
            _sfxVolume = PlayerPrefs.GetFloat(SfxKey);
            _musicVolume = PlayerPrefs.GetFloat(MusicKey);
            _muted = PlayerPrefs.GetInt(MuteKey) == 1;
        }
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt(MuteKey, _muted ? 1 : 0);
        PlayerPrefs.SetFloat(SfxKey, _sfxVolume);
        PlayerPrefs.SetFloat(MusicKey, _musicVolume);
        PlayerPrefs.Save();
    }

    public void OnToggleMute()
    {
        _muted = !_muted;
        onSettingsChanged.Invoke();
    }
    
    public void OnSfxVolumeChanged(float value)
    {
        _sfxVolume = value;
        onSettingsChanged.Invoke();
    }

    public void OnMusicVolumeChanged(float value)
    {
        _musicVolume = value;
        onSettingsChanged.Invoke();
    }
}
