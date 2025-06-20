using UnityEngine;
using UnityEngine.UI;

namespace ProjectPipe
{
    public class UISettingsManager : MonoBehaviour
    {
        public Toggle fullscreenToggle;
        public Slider mouseSensitivitySlider;
        public Slider musicVolumeSlider;

        private void Start()
        {
            var settings = WorldSettingsManager.Instance;
            fullscreenToggle.isOn = settings.IsFullscreen;
            mouseSensitivitySlider.value = settings.MouseSensitivity;
            musicVolumeSlider.value = settings.MusicVolume;
        }

        public void OnFullscreenChanged(bool value)
        {
            UISoundFXManager.Instance.PlayClick();
            WorldSettingsManager.Instance.SetFullscreen(value);
        }

        public void OnMouseSensitivityChanged(float value)
        {
            UISoundFXManager.Instance.PlayClick();
            WorldSettingsManager.Instance.SetMouseSensitivity(value);
        }

        public void OnMusicVolumeChanged(float value)
        {
            UISoundFXManager.Instance.PlayClick();
            WorldSettingsManager.Instance.SetMusicVolume(value);
        }
    }
}

