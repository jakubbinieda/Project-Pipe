using UnityEngine;

namespace ProjectPipe
{
    public class WorldSettingsManager : MonoBehaviour
    {
        private const string MusicVolumeKey = "MusicVolume";
        private const string FullscreenKey = "Fullscreen";
        private const string MouseSensitivityKey = "MouseSensitivity";

        private const float DefaultMusicVolume = 1f;
        private const bool DefaultFullscreen = true;
        private const float DefaultMouseSensitivity = 1f;

        public float MusicVolume { get; private set; }
        public bool IsFullscreen { get; private set; }
        public float MouseSensitivity { get; private set; }
        
        public static WorldSettingsManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadSettings();
            ApplyFullscreen();
            ApplyMusicVolume();
        }

        public void LoadSettings()
        {
            MusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, DefaultMusicVolume);
            IsFullscreen = PlayerPrefs.GetInt(FullscreenKey, DefaultFullscreen ? 1 : 0) == 1;
            MouseSensitivity = PlayerPrefs.GetFloat(MouseSensitivityKey, DefaultMouseSensitivity);
        }

        public void SaveSettings()
        {
            PlayerPrefs.SetFloat(MusicVolumeKey, MusicVolume);
            PlayerPrefs.SetInt(FullscreenKey, IsFullscreen ? 1 : 0);
            PlayerPrefs.SetFloat(MouseSensitivityKey, MouseSensitivity);
            PlayerPrefs.Save();
        }

        public void SetMusicVolume(float volume)
        {
            MusicVolume = Mathf.Clamp01(volume);
            ApplyMusicVolume();
            SaveSettings();
        }

        public void SetFullscreen(bool fullscreen)
        {
            IsFullscreen = fullscreen;
            ApplyFullscreen();
            SaveSettings();
        }

        public void SetMouseSensitivity(float sensitivity)
        {
            MouseSensitivity = Mathf.Clamp(sensitivity, 0.1f, 1f);
            SaveSettings();
        }

        private void ApplyFullscreen()
        {
            Screen.fullScreen = IsFullscreen;
        }

        private void ApplyMusicVolume()
        {
            AudioListener.volume = MusicVolume;
        }
    }
}
