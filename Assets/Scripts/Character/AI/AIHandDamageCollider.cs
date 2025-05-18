using UnityEngine;

namespace ProjectPipe
{
    public class AIHandDamageCollider : DamageCollider
    {
        public float LightAttackDamageMultiplier { get; set; } = 1f;
        public float HeavyAttackDamageMultiplier { get; set; }
        public float ChargedAttackDamageMultiplier { get; set; }
        
        [HideInInspector] private AICharacterManager _aiCharacterManager;
        
        protected  override void Awake() 
        {
            base.Awake();
            Collider = GetComponent<Collider>();
            _aiCharacterManager = GetComponentInParent<AICharacterManager>();
        }
        
        protected override void DamageTarget(CharacterManager target)
        {
            if (DamagedCharacters.Contains(target)) return;

            DamagedCharacters.Add(target);

            var damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.TakeDamageEffect);
            damageEffect.PhysicalDamage = PhysicalDamage;
            damageEffect.ContactPoint = ContactPoint;
            damageEffect.AngleHitFrom =
                Vector3.SignedAngle(_aiCharacterManager.transform.forward, target.transform.forward, Vector3.up);

            var damageModifier = _aiCharacterManager.CharacterCombatManager.CurrentAttackType switch
            {
                AttackType.LightAttack01 => LightAttackDamageMultiplier,
                AttackType.LightAttack02 => LightAttackDamageMultiplier,
                AttackType.HeavyAttack01 => HeavyAttackDamageMultiplier,
                AttackType.HeavyAttack02 => HeavyAttackDamageMultiplier,
                AttackType.ChargedAttack01 => ChargedAttackDamageMultiplier,
                AttackType.ChargedAttack02 => ChargedAttackDamageMultiplier,
                _ => 1
            };

            damageEffect.PhysicalDamage = Mathf.RoundToInt(damageEffect.PhysicalDamage * damageModifier);
            target.CharacterEffectsManager.ProcessInstantEffect(damageEffect);
        }
    }
}
