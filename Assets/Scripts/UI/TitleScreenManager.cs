using UnityEngine;

namespace ProjectPipe
{
    public class TitleScreenManager : MonoBehaviour
    {
        public void StartNewGame()
        {
            StartCoroutine(WorldSaveGameManager.Instance.LoadNewGame());
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
