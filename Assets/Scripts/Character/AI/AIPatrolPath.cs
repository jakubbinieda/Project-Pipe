using System.Collections.Generic;
using UnityEngine;

namespace ProjectPipe
{
    public class AIPatrolPath : MonoBehaviour
    {
        public int patrolPathIndex;
        public List<Vector3> path = new List<Vector3>();
        
        private void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                path.Add(transform.GetChild(i).position);
            }
            
            WorldUtilityManager.Instance.AddPatrolPath(this);
        }
    }
}
