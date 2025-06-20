using UnityEngine;
using UnityEngine.UI;

namespace ProjectPipe
{
    public class PauseScreenManager : MonoBehaviour
    {
        [Header("Menus")]
        [SerializeField] private GameObject pauseScreenMenu;
        [SerializeField] private GameObject pauseScreenSettings;

        [Header("Buttons")]
        [SerializeField] Button resumeButton;
        [SerializeField] Button settingsButton;
        [SerializeField] Button closeSettingsButton;

        private void Update()
        {
            if (PlayerInputManager.Instance.PauseInput)
            {
                PlayerInputManager.Instance.ExitGameplayMode();
                ShowPauseMenu();
                Time.timeScale = 0;
            }
        }

        public void ShowPauseMenu()
        {
            pauseScreenMenu.SetActive(true);
            resumeButton.Select();
        }

        public void Resume()
        {
            UISoundFXManager.Instance.PlayClick();
            pauseScreenMenu.SetActive(false);
            PlayerInputManager.Instance.EnterGameplayMode();
            Time.timeScale = 1;
        }

        public void SaveGame()
        {
            UISoundFXManager.Instance.PlayClick();
            WorldSaveGameManager.Instance.SaveGame();
        }

        public void OpenSettings()
        {
            UISoundFXManager.Instance.PlayClick();
            pauseScreenMenu.SetActive(false);
            pauseScreenSettings.SetActive(true);
            closeSettingsButton.Select();
        }

        public void CloseSettings()
        {
            UISoundFXManager.Instance.PlayClick();
            pauseScreenSettings.SetActive(false);
            pauseScreenMenu.SetActive(true);
            settingsButton.Select();
        }

        public void BackToMainMenu()
        {
            UISoundFXManager.Instance.PlayClick();
            pauseScreenMenu.SetActive(false);
            Time.timeScale = 1;
            WorldSaveGameManager.Instance.BackToMainMenu();
        }

        public void PlayHover()
        {
            UISoundFXManager.Instance.PlayHover();
        }
    }
}
