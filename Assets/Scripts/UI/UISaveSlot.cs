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
        public TextMeshProUGUI timePlayed;

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
                // TODO: update slot info
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
   
        public void LoadGameFromGameSlot()
        {
            WorldSaveGameManager.Instance.currentGameSlotBeingUsed = gameSlot;
            WorldSaveGameManager.Instance.LoadGame();
        }
    }
}