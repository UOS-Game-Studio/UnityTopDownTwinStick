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
    
    /*
     * This is a restrictive class as I've tightly coupled it to the type of slider it represents.
     * That's fine for this particular implementation, but if we had more sliders that used it, we would
     * need to find a cleaner way of doing this.
     *
     * Fundamentally, this is only here as a connector between SettingsControl and the actual Slider object
     * if we didn't need the slider to be updated based on saved values, we wouldn't need this.
    */
    
    /// <summary>
    /// SliderValue is something of a restrictive class as it's tightly coupled to the "type" of Slider it represents
    /// that's fine for now, but if we had more sliders we would need to find a better approach.
    /// Events:
    ///   onValueChanged - invoked when the slider value is changed, which then passes the value through to <c>SettingsControl<c>
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
