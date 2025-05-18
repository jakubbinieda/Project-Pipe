using UnityEngine;

namespace ProjectPipe
{
    [System.Serializable]
    public class GameSaveData
    {
        [Header("Time Played")]
        public float secondsPlayed;

        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;
    }
}