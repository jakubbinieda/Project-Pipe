using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace ProjectPipe 
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        [SerializeField] private int worldSceneIndex = 1;

        public static WorldSaveGameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public IEnumerator LoadNewGame()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

            yield return null;
        }
    }
}
