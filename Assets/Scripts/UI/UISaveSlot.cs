using UnityEngine;
using TMPro;

namespace ProjectPipe
{
    public class UISaveSlot : MonoBehaviour
    {
        private SaveFileDataWriter _saveFileDataWriter;

        [Header("Game Slot")]
        public GameSlot gameSlot;

        [Header("Game Info")]
        public TextMeshProUGUI saveDateTime;

        private void OnEnable()
        {
            LoadSaveSlots();
        }

        private void LoadSaveSlots()
        {
            _saveFileDataWriter = new SaveFileDataWriter();
            _saveFileDataWriter.SaveDataDirectoryPath = Application.persistentDataPath;
            _saveFileDataWriter.SaveFileName = WorldSaveGameManager.Instance.DecideGameFileNameBasedOnGameSlotBeingUsed(gameSlot);

            if (_saveFileDataWriter.CheckToSeeIfFileExists())
            {
                var data = _saveFileDataWriter.LoadSaveFile();
                saveDateTime.text = data.saveTimestamp;
            }
            else
            {
                saveDateTime.text = "Empty Slot";
                // gameObject.SetActive(false);
            }
        }

        public void LoadGameFromGameSlot()
        {
            _saveFileDataWriter = new SaveFileDataWriter();
            _saveFileDataWriter.SaveDataDirectoryPath = Application.persistentDataPath;
            _saveFileDataWriter.SaveFileName = WorldSaveGameManager.Instance.DecideGameFileNameBasedOnGameSlotBeingUsed(gameSlot);

            if (_saveFileDataWriter.CheckToSeeIfFileExists())
            {
                WorldSaveGameManager.Instance.currentGameSlotBeingUsed = gameSlot;
                WorldSaveGameManager.Instance.LoadGame();
            }
        }

        public void StartNewGameFromSlot()
        {
            WorldSaveGameManager.Instance.CreateNewGame(gameSlot);
        }
    }
}