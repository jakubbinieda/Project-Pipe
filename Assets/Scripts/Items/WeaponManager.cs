using UnityEngine;

namespace ProjectPipe
{
    public class WeaponManager : MonoBehaviour
    {
        [field: SerializeField] private WeaponDamageCollider damageCollider;

        private void Awake()
        {
            damageCollider = GetComponentInChildren<WeaponDamageCollider>();
        }

        public void Initialize(CharacterManager wielder, WeaponItem weapon)
        {
            damageCollider.attacker = wielder;
            damageCollider.PhysicalDamage = weapon.PhysicalDamage;
            damageCollider.LightAttackDamageMultiplier = weapon.LightAttackDamageMultiplier;
            damageCollider.HeavyAttackDamageMultiplier = weapon.HeavyAttackDamageMultiplier;
            damageCollider.ChargedAttackDamageMultiplier = weapon.ChargedAttackDamageMultiplier;
        }

        public void EnableDamageCollider()
        {
            damageCollider.EnableDamageCollider();
        }

        public void DisableDamageCollider()
        {
            damageCollider.DisableDamageCollider();
        }
    }
}