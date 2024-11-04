using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class MixerControl : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        private AudioSource _audioSource;
        private SettingsControl _settings;

        private const string SfxMixer = "SfxVolume";
        private const string MusicMixer = "MusicVolume";
        
        private void Start()
        {
            _settings = FindFirstObjectByType<SettingsControl>();
            // hook into the settings changed event so we update the mixer settings as soon as the 
            // player changes anything.
            _settings.onSettingsChanged.AddListener(UpdateMixer);
            
            _audioSource = gameObject.GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }

            _audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];

            audioMixer.SetFloat(SfxMixer, _settings.SfxDb);
            audioMixer.SetFloat(MusicMixer, _settings.MusicDb);
            _audioSource.mute = _settings.IsMuted;
        }

        private void OnDisable()
        {
            // as this mixer control is being removed, we can't leave it hooked up to SettingsControl.
            _settings.onSettingsChanged.RemoveListener(UpdateMixer);
        }

        private void UpdateMixer()
        {
            audioMixer.SetFloat(SfxMixer, _settings.SfxDb);
            audioMixer.SetFloat(MusicMixer, _settings.MusicDb);
            _audioSource.mute = _settings.IsMuted;
        }
    }
}
