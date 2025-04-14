using UnityEngine;

namespace ProjectPipe
{
    public class WorldUtilityManager : MonoBehaviour
    {
        [HideInInspector] public static WorldUtilityManager Instance;
        
        [Header("Layers")]
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
