using UnityEngine;
using UnityEngine.UI;

namespace ProjectPipe
{
    public class TitleScreenManager : MonoBehaviour
    {
        [Header("Menus")]
        [SerializeField] GameObject titleScreenMainMenu;
        [SerializeField] GameObject titleScreenLoadMenu;
        [SerializeField] GameObject titleScreenSettingsMenu;

        [Header("Buttons")]
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button mainMenuLoadGameButton;
        [SerializeField] Button settingsMenuReturnButton;
        [SerializeField] Button mainMenuSettingsButton;

        public void PlayHover()
        {
            UISoundFXManager.Instance.PlayHover();
        }

        public void StartNewGame()
        {
            UISoundFXManager.Instance.PlayClick();
            WorldSaveGameManager.Instance.CreateNewGame();
            StartCoroutine(WorldSaveGameManager.Instance.LoadWorldScene());
        }

        public void OpenLoadGameMenu()
        {
            UISoundFXManager.Instance.PlayClick();
            titleScreenMainMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);
            loadMenuReturnButton.Select();
        }

        public void CloseLoadGameMenu()
        {
            UISoundFXManager.Instance.PlayClick();
            titleScreenLoadMenu.SetActive(false);
            titleScreenMainMenu.SetActive(true);
            mainMenuLoadGameButton.Select();
        }
        
        public void OpenSettingsMenu()
        {
            UISoundFXManager.Instance.PlayClick();
            titleScreenMainMenu.SetActive(false);
            titleScreenSettingsMenu.SetActive(true);
            settingsMenuReturnButton.Select();
        }
        
        public void CloseSettingsMenu()
        {
            UISoundFXManager.Instance.PlayClick();
            titleScreenSettingsMenu.SetActive(false);
            titleScreenMainMenu.SetActive(true);
            mainMenuSettingsButton.Select();
        }

        public void ExitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
