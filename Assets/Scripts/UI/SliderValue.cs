using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public enum SliderType
    {
        Sfx,
        Music
    }
    
    public class SliderValue : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        public SliderType type;
        public UnityEvent<float> onValueChanged = new();
        
        private void Start()
        {
            SettingsControl settings = GameObject.FindFirstObjectByType<SettingsControl>();
            
            if (settings == null) return;
            
            switch (type)
            {
                case SliderType.Music:
                    onValueChanged.AddListener(settings.OnMusicVolumeChanged);
                    slider.value = settings.MusicVolume;
                    break;
                case SliderType.Sfx:
                    onValueChanged.AddListener(settings.OnSfxVolumeChanged);
                    slider.value = settings.SfxVolume;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnSliderValueChanged()
        {
            onValueChanged.Invoke(slider.value);
        }
    }
}
