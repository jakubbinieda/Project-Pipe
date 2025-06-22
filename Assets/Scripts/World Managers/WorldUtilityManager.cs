using System.Collections.Generic;
using UnityEngine;

namespace ProjectPipe
{
    public class WorldUtilityManager : MonoBehaviour
    {
        public static WorldUtilityManager Instance { get; private set; }

        [field: Header("Layers")]
        [field: SerializeField] public LayerMask CharacterLayers;
        [field: SerializeField] public LayerMask EnvironmentLayers;
        [field: SerializeField] public LayerMask AILayers;
        
        [field: Header("Patrol Paths")]
        [field: SerializeField] List<AIPatrolPath> PatrolPaths = new List<AIPatrolPath>();

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
            var viewableAnge = Vector3.Angle(characterTransform.forward, targetsDirection);
            var cross = Vector3.Cross(characterTransform.forward, targetsDirection);
            if (cross.y < 0)
            {
                viewableAnge = -viewableAnge;
            }
            return viewableAnge;
        }
        
        public void AddPatrolPath(AIPatrolPath path)
        {
            if (!PatrolPaths.Contains(path))
            {
                PatrolPaths.Add(path);
            }
        }
        
        public AIPatrolPath GetPatrolPathByIndex(int index)
        {
            AIPatrolPath path = null;
            for (int i = 0; i < PatrolPaths.Count; i++)
            {
                Debug.Log(PatrolPaths[i].patrolPathIndex);
                if (PatrolPaths[i].patrolPathIndex == index)
                {
                    path = PatrolPaths[i];
                }
            }
            return path;
        }
    }
}
