using UnityEngine;

namespace ProjectPipe
{
    public class WeaponDamageCollider : DamageCollider
    {
        public CharacterManager attacker;

        public float LightAttackDamageMultiplier { get; set; }
        public float HeavyAttackDamageMultiplier { get; set; }
        public float ChargedAttackDamageMultiplier { get; set; }

        protected override void Awake()
        {
            base.Awake();

            Collider = GetComponent<Collider>();
            Collider.enabled = false;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            var target = other.GetComponentInParent<CharacterManager>();

            if (!target) return;

            if (target == attacker) return;

            ContactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            DamageTarget(target);
        }

        protected override void DamageTarget(CharacterManager target)
        {
            if (DamagedCharacters.Contains(target)) return;

            DamagedCharacters.Add(target);

            var damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.TakeDamageEffect);
            damageEffect.PhysicalDamage = PhysicalDamage;
            damageEffect.ContactPoint = ContactPoint;
            damageEffect.AngleHitFrom =
                Vector3.SignedAngle(attacker.transform.forward, target.transform.forward, Vector3.up);

            var damageModifier = attacker.CharacterCombatManager.CurrentAttackType switch
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