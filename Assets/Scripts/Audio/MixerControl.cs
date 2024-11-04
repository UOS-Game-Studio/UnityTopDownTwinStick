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
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _settings = FindFirstObjectByType<SettingsControl>();
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

        private void UpdateMixer()
        {
            audioMixer.SetFloat(SfxMixer, _settings.SfxDb);
            audioMixer.SetFloat(MusicMixer, _settings.MusicDb);
            _audioSource.mute = _settings.IsMuted;
        }
    }
}
