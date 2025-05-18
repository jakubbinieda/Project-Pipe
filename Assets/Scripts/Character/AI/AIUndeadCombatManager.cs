using UnityEngine;

namespace ProjectPipe
{
    public class AIUndeadCombatManager : AICharacterCombatManager
    {
        [Header("Hand Damage Colliders")]
        [field: SerializeField] private AIHandDamageCollider RightHandDamageCollider { get; set;  }

        [field: SerializeField] private AIHandDamageCollider LeftHandDamageCollider { get; set; }

        [Header("Damage")]
        public float BaseDamage { get; set; } = 25;
        public float Attack01DamageMultiplier { get; set; } = 1.0f;
        public float Attack02DamageMultiplier { get; set; } = 2.0f;

        public void SetAttack01Damage()
        {
            RightHandDamageCollider.PhysicalDamage = Mathf.RoundToInt(BaseDamage * Attack01DamageMultiplier);
            LeftHandDamageCollider.PhysicalDamage = Mathf.RoundToInt(BaseDamage * Attack01DamageMultiplier);
        }
        
        public void SetAttack02Damage()
        {
            RightHandDamageCollider.PhysicalDamage = Mathf.RoundToInt(BaseDamage * Attack02DamageMultiplier);
            LeftHandDamageCollider.PhysicalDamage = Mathf.RoundToInt(BaseDamage * Attack02DamageMultiplier);
        }
        
        public void OpenRightHandDamageCollider()
        {
            RightHandDamageCollider.EnableDamageCollider();
        }
        
        public void CloseRightHandDamageCollider()
        {
            RightHandDamageCollider.DisableDamageCollider();
        }
        
        public void OpenLeftHandDamageCollider()
        {
            LeftHandDamageCollider.EnableDamageCollider();
        }
        
        public void CloseLeftHandDamageCollider()
        {
            LeftHandDamageCollider.DisableDamageCollider();
        }
    }
}