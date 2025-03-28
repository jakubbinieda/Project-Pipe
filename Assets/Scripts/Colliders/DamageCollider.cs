using UnityEngine;

namespace ProjectPipe
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Damage")]
        [field: SerializeField] public int PhysicalDamage { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            var target = other.GetComponent<CharacterManager>();

            if (!target) return;

            DamageTarget(target);
        }


        private void DamageTarget(CharacterManager target)
        {
            var damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.TakeDamageEffect);
            damageEffect.PhysicalDamage = PhysicalDamage;

            target.CharacterEffectsManager.ProcessInstantEffect(damageEffect);
        }
    }
}