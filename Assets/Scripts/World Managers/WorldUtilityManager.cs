using UnityEngine;

namespace ProjectPipe
{
    public class WorldUtilityManager : MonoBehaviour
    {
        public static WorldUtilityManager Instance { get; private set; }
        
        [field: Header("Layers")]
        [field: SerializeField] public LayerMask CharacterLayers { get; private set; }
        [field: SerializeField] public LayerMask EnvironmentLayers { get; private set; }

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
    }
}
