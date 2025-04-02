using System.Collections.Generic;
using UnityEngine;

namespace ProjectPipe
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Damage")]
        [field: SerializeField] public int PhysicalDamage { get; set; }
        protected readonly List<CharacterManager> DamagedCharacters = new();

        protected Collider Collider;
        protected Vector3 ContactPoint;

        protected virtual void Awake()
        {
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            var target = other.GetComponentInParent<CharacterManager>();

            if (!target) return;

            DamageTarget(target);
        }

        protected virtual void DamageTarget(CharacterManager target)
        {
            if (DamagedCharacters.Contains(target)) return;

            DamagedCharacters.Add(target);

            var damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.TakeDamageEffect);
            damageEffect.PhysicalDamage = PhysicalDamage;

            target.CharacterEffectsManager.ProcessInstantEffect(damageEffect);
        }

        public void EnableDamageCollider()
        {
            Collider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            Collider.enabled = false;
            DamagedCharacters.Clear();
        }
    }
}