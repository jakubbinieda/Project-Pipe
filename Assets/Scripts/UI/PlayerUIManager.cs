using UnityEngine;

namespace ProjectPipe
{
    public class PlayerUIManager : MonoBehaviour
    {
        [field: SerializeField] public PlayerUIHudManager PlayerUIHudManager { get; private set; }
        [field: SerializeField] public PlayerUIPopUpManager PlayerUIPopUpManager { get; private set; }

        public static PlayerUIManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            PlayerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
            PlayerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
        }
    }
}
