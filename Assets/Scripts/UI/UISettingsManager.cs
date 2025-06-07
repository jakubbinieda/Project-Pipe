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
            WorldSettingsManager.Instance.SetFullscreen(value);
        }

        public void OnMouseSensitivityChanged(float value)
        {
            WorldSettingsManager.Instance.SetMouseSensitivity(value);
        }

        public void OnMusicVolumeChanged(float value)
        {
            WorldSettingsManager.Instance.SetMusicVolume(value);
        }
    }
}

