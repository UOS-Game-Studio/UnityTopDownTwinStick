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
    
    // Good practice: to avoid silly spelling mistakes, we use constants to store PlayerPref key names.
    private const string SfxKey = "sfxVolume";
    private const string MusicKey = "musicVolume";
    private const string MuteKey = "muted";

    /*
     * leaving this as "plain" properties, we could use auto-properties for them:
     *    public float SfxVolume => { get; private set; } 
     * but properties are never serialised into the Editor Inspector, so even if we
     * set the inspector to Debug, we cannot see the values they represent.
     */
    public float SfxVolume => _sfxVolume;
    public float MusicVolume => _musicVolume;
    public bool IsMuted => _muted;
    
    // we take advantage of the remap function from Unity.Mathematics here to
    // convert the volume range (0 - 100) into a decibel range (-80 - 0).
    public float SfxDb => math.remap(0.0f, 100.0f, -80.0f, 0.0f, _sfxVolume);
    public float MusicDb => math.remap(0.0f, 100.0f, -80.0f, 0.0f, _musicVolume);
    
    private void Awake()
    {
        // SettingsControl should exist for the entire run of the game.
        // we can either destroy it at game over when the player chooses to return to the menu
        // or we can make it a functional singleton... decisions!
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
        onSettingsChanged.RemoveAllListeners();
        
        // save the current values for our settings to PlayerPrefs
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
