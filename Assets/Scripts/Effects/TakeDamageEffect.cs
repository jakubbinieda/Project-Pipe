using UnityEngine;

namespace ProjectPipe
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("Damage")]
        [field: SerializeField] public int PhysicalDamage { get; set; }

        [Header("Total Damage")]
        [field: SerializeField] public int TotalDamage { get; private set; }

        public override void ProcessEffect(CharacterManager characterManager)
        {
            base.ProcessEffect(characterManager);

            if (characterManager.IsDead) return;

            CalculateDamage(characterManager);
        }

        private void CalculateDamage(CharacterManager characterManager)
        {
            TotalDamage = Mathf.RoundToInt(PhysicalDamage);

            if (TotalDamage <= 0) TotalDamage = 1;

            characterManager.CharacterStatsManager.ReduceHealth(TotalDamage);
        }
    }
}