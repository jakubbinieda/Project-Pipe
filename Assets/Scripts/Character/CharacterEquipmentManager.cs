using UnityEngine;

namespace ProjectPipe
{
    public class CharacterEquipmentManager : MonoBehaviour
    {
        [field: SerializeField] public WeaponItem EquippedWeapon { get; set; }

        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
        }

        protected virtual void Update()
        {
        }

        public virtual void OpenDamageCollider()
        {
        }

        public virtual void CloseDamageCollider()
        {
        }
    }
}