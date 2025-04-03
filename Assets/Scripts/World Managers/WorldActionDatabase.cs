using System.Collections.Generic;
using UnityEngine;

namespace ProjectPipe
{
    public class WorldActionDatabase : MonoBehaviour
    {
        [field: SerializeField] public List<WeaponItemAction> Actions { get; set; } = new();

        public static WorldActionDatabase Instance { get; private set; }

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

        private void Start()
        {
            foreach (var action in Actions) action.ActionID = Actions.IndexOf(action);
        }
    }
}