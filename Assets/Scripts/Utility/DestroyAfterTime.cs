using UnityEngine;

namespace ProjectPipe
{
    public class DestroyAfterTime : MonoBehaviour
    {
        [SerializeField] private float timeUntilDestroyed = 5;

        private void Awake()
        {
            Destroy(gameObject, timeUntilDestroyed);
        }
    }
}