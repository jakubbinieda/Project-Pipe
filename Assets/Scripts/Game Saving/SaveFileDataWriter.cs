using UnityEngine;
using System;
using System.IO;

namespace ProjectPipe
{
    public class SaveFileDataWriter
    {
        public string SaveDataDirectoryPath = "";
        public string SaveFileName = "";

        public bool CheckToSeeIfFileExists()
        {
            if (File.Exists(Path.Combine(SaveDataDirectoryPath, SaveFileName)))
                return true;
            return false;
        }

        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(SaveDataDirectoryPath, SaveFileName));
        }

        public void CreateNewGameSaveFile(GameSaveData gameData)
        {
            string savePath = Path.Combine(SaveDataDirectoryPath, SaveFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));

                string dataToStore = JsonUtility.ToJson(gameData, true);

                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter fileWriter = new StreamWriter(stream))
                    {
                        fileWriter.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public GameSaveData LoadSaveFile()
        {
            GameSaveData gameData = null;
            string loadPath = Path.Combine(SaveDataDirectoryPath, SaveFileName);

            if (File.Exists(loadPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                    {
                        using (StreamReader fileReader = new StreamReader(stream))
                        {
                            dataToLoad = fileReader.ReadToEnd();
                        }
                    }

                    gameData = JsonUtility.FromJson<GameSaveData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }

            return gameData;
        }
    }
}