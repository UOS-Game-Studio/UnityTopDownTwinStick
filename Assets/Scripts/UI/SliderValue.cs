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

    /// <summary>
    /// SliderValue is something of a restrictive class as it's tightly coupled to the "type" of Slider it represents
    /// that's fine for now, but if we had more sliders we would need to find a better approach.
    /// Events:
    ///   <c>onValueChanged</c> - invoked when the slider value is changed, which then passes the value through to <c>SettingsControl</c>
    /// </summary>
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

        private void OnDestroy()
        {
            onValueChanged.RemoveAllListeners();
        }

        public void OnSliderValueChanged()
        {
            onValueChanged.Invoke(slider.value);
        }
    }
}
