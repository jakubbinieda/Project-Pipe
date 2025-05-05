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

        public float GetAngleOfTarget(Transform characterTransform, Vector3 targetsDirection)
        {
            targetsDirection.y = 0;
            float viewableAnge = Vector3.Angle(characterTransform.forward, targetsDirection);
            Vector3 cross = Vector3.Cross(characterTransform.forward, targetsDirection);
            if (cross.y < 0)
            {
                viewableAnge = -viewableAnge;
            }
            return viewableAnge;
        }
    }
}
