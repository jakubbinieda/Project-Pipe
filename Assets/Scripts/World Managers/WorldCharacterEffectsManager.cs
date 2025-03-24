using System.Collections.Generic;
using UnityEngine;

namespace ProjectPipe
{
    public class WorldCharacterEffectsManager : MonoBehaviour
    {
        [Header("Instant Effects")]
        [SerializeField] private List<InstantCharacterEffect> instantCharacterEffects = new();

        [Header("Damage Effect")]
        [field: SerializeField] public TakeDamageEffect TakeDamageEffect { get; set; }

        public static WorldCharacterEffectsManager Instance { get; private set; }

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
            for (var i = 0; i < instantCharacterEffects.Count; ++i) instantCharacterEffects[i].InstantEffectID = i;
        }
    }
}