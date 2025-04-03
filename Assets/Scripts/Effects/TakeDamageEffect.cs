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

        public Vector3 ContactPoint { get; set; }
        public float AngleHitFrom { get; set; }

        public override void ProcessEffect(CharacterManager characterManager)
        {
            base.ProcessEffect(characterManager);

            if (characterManager.IsDead) return;

            CalculateDamage(characterManager);
            PlayDirectionalDamageAnimation(characterManager);
            PlayDamageVFX(characterManager);
        }

        private void CalculateDamage(CharacterManager characterManager)
        {
            TotalDamage = Mathf.RoundToInt(PhysicalDamage);

            if (TotalDamage <= 0) TotalDamage = 1;

            characterManager.CharacterStatsManager.ReduceHealth(TotalDamage);
        }

        private void PlayDamageVFX(CharacterManager characterManager)
        {
            characterManager.CharacterEffectsManager.PlayBloodSplatterVFX(ContactPoint);
        }

        private void PlayDirectionalDamageAnimation(CharacterManager characterManager)
        {
            if (characterManager.IsDead) return;

            characterManager.CharacterAnimatorManager.PlayTargetDirectionalDamageActionAnimation(AngleHitFrom, true,
                true);
        }
    }
}