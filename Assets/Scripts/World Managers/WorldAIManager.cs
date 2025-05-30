using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace ProjectPipe
{
    public class WorldAIManager : MonoBehaviour
    {
        private static WorldAIManager Instance { get; set; }

        [field: Header("AI Characters")]
        [field: SerializeField] private GameObject[] aiCharacters;
        [field: SerializeField] private List<GameObject> aiCharactersSpawned;


        [field: Header("Debug")]
        [field: SerializeField] private bool DespawnAICharacters { get; set; }
        [field: SerializeField] private bool RespawnAICharacters { get; set; }
        
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
            if (DespawnAICharacters)
            {
                DespawnAllCharacters();
                DespawnAICharacters = false;
            }

            if (RespawnAICharacters)
            {
                SpawnAllCharacters();
                RespawnAICharacters = false;
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