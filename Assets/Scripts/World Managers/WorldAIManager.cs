using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace ProjectPipe
{
    public class WorldAIManager : MonoBehaviour
    {
        [HideInInspector] public static WorldAIManager Instance;

        [Header("AI Characters")]
        [SerializeField] private GameObject[] aiCharacters;
        [SerializeField] private List<GameObject> aiCharactersSpawned;
        
        [Header("Debug")]
        [SerializeField] private bool despawnAICharacters = false;
        [SerializeField] private bool respawnAICharacters = false;
        
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

        private void Start()
        {
            StartCoroutine(WaitForSceneToLoadThenSpawnCharacters());
        }
        
        private void Update()
        {
            if (despawnAICharacters)
            {
                DespawnAllCharacters();
                despawnAICharacters = false;
            }

            if (respawnAICharacters)
            {
                SpawnAllCharacters();
                respawnAICharacters = false;
            }
        }

        private IEnumerator WaitForSceneToLoadThenSpawnCharacters()
        {
            while (!SceneManager.GetActiveScene().isLoaded)
            {
                yield return null;
            }

            SpawnAllCharacters();
        }

        private void SpawnAllCharacters()
        {
            foreach (var character in aiCharacters) 
            {
                var characterSpawned = Instantiate(character);
                aiCharactersSpawned.Add(characterSpawned);
            }
        }
        
        private void DespawnAllCharacters()
        {
            foreach (var character in aiCharactersSpawned)
            {
                Destroy(character);
            }
            
            aiCharactersSpawned.Clear();
        }

        private void DisableAllCharacters()
        {
            // TODO - to save resources
        }
    }
}