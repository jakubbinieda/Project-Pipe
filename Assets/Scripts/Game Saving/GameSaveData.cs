using UnityEngine;
using System.Collections.Generic;

namespace ProjectPipe
{
    [System.Serializable]
    public class GameSaveData
    {
        [Header("Save Metadata")]
        public string saveTimestamp; // Format: "dd MMM yyyy, HH:mm""

        [Header("Player")]
        public float xPosition, yPosition, zPosition;
        public float xRotation, yRotation, zRotation, wRotation;
        public int currentHealth;
        public float currentStamina;

        [Header("Enemies")]
        public List<EnemySaveData> enemies = new List<EnemySaveData>();
    }

    [System.Serializable]
    public class EnemySaveData
    {
        public string enemyId;

        public float xPosition, yPosition, zPosition;
        public float xRotation, yRotation, zRotation, wRotation;

        public int currentHealth;
        public bool isDead;
    }

}