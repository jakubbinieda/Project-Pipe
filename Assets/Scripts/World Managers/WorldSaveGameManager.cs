using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace ProjectPipe 
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        [Header("World Scene Index")]
        [SerializeField] private int worldSceneIndex = 1;

        [Header("SAVE/LOAD")]
        [SerializeField] bool saveGame;
        [SerializeField] bool loadGame;

        [Header("Save Data Writer")]
        private SaveFileDataWriter saveFileDataWriter;

        [Header("Current Game Data")]
        public GameSlot currentGameSlotBeingUsed;
        public GameSaveData currentGameData;
        private string saveFileName;

        [Header("Game Slots")]
        public GameSaveData gameSlot01;
        public GameSaveData gameSlot02;
        public GameSaveData gameSlot03;

        public static WorldSaveGameManager Instance { get; private set; }

        public PlayerManager player;

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
            LoadAllGameSaves();
        }

        private void Update()
        {
            if (saveGame)
            {
                saveGame = false;
                SaveGame();
            }
            
            if (loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        public string DecideGameFileNameBasedOnGameSlotBeingUsed(GameSlot gameSlot)
        {
            string fileName = "";
            switch (gameSlot)
            {
                case GameSlot.GameSlot_01:
                    fileName = "gameSlot_01";
                    break;
                case GameSlot.GameSlot_02:
                    fileName = "gameSlot_02";
                    break;
                case GameSlot.GameSlot_03:
                    fileName = "gameSlot_03";
                    break;
                default:
                    break;
            }
            return fileName;
        }

        public void CreateNewGame(GameSlot slot)
        {
            currentGameSlotBeingUsed = slot;
            saveFileName = DecideGameFileNameBasedOnGameSlotBeingUsed(currentGameSlotBeingUsed);

            currentGameData = new GameSaveData();

            DeleteSaveGame(slot);

            StartCoroutine(LoadWorldScene(true));
        }

        public void LoadGame()
        {
            saveFileName = DecideGameFileNameBasedOnGameSlotBeingUsed(currentGameSlotBeingUsed);

            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.SaveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.SaveFileName = saveFileName;

            currentGameData = saveFileDataWriter.LoadSaveFile();

            StartCoroutine(LoadWorldScene(false));
        }

        public void SaveGame()
        {
            saveFileName = DecideGameFileNameBasedOnGameSlotBeingUsed(currentGameSlotBeingUsed);

            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.SaveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.SaveFileName = saveFileName;

            currentGameData.saveTimestamp = System.DateTime.Now.ToString("dd MMM yyyy, HH:mm");

            player.SaveGame(ref currentGameData);

            currentGameData.enemies.Clear();
            var allEnemies = FindObjectsByType<AICharacterManager>(FindObjectsSortMode.None);
            foreach (var enemy in allEnemies)
            {
                enemy.SaveGame(ref currentGameData);
            }

            saveFileDataWriter.CreateNewGameSaveFile(currentGameData);
        }

        public void DeleteSaveGame(GameSlot slot)
        {
            string fileName = DecideGameFileNameBasedOnGameSlotBeingUsed(slot);

            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.SaveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.SaveFileName = fileName;

            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                saveFileDataWriter.DeleteSaveFile();

                switch (slot)
                {
                    case GameSlot.GameSlot_01:
                        gameSlot01 = null;
                        break;
                    case GameSlot.GameSlot_02:
                        gameSlot02 = null;
                        break;
                    case GameSlot.GameSlot_03:
                        gameSlot03 = null;
                        break;
                }
            }
        }

        private void LoadAllGameSaves()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.SaveDataDirectoryPath = Application.persistentDataPath;

            saveFileDataWriter.SaveFileName = DecideGameFileNameBasedOnGameSlotBeingUsed(GameSlot.GameSlot_01);
            gameSlot01 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.SaveFileName = DecideGameFileNameBasedOnGameSlotBeingUsed(GameSlot.GameSlot_02);
            gameSlot02 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.SaveFileName = DecideGameFileNameBasedOnGameSlotBeingUsed(GameSlot.GameSlot_03);
            gameSlot03 = saveFileDataWriter.LoadSaveFile();
        }

        public IEnumerator LoadWorldScene(bool isNewGame)
        {
            PlayerInputManager.Instance.EnterGameplayMode();

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
            yield return new WaitUntil(() => loadOperation.isDone);

            player = FindFirstObjectByType<PlayerManager>();
            player.LoadGame(ref currentGameData, isNewGame);

            var allEnemies = Object.FindObjectsByType<AICharacterManager>(FindObjectsSortMode.None);
            foreach (var enemy in allEnemies)
            {
                enemy.LoadGame(currentGameData);
            }
        }   

        public void BackToMainMenu()
        {
            SceneManager.LoadScene("Scene_Main_Menu");
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}