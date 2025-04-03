using UnityEngine;

namespace ProjectPipe
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        [field: SerializeField] public GameObject Prefab { get; set; }

        [Header("Weapon Stats")]
        [field: SerializeField] public int PhysicalDamage { get; set; }
        [field: SerializeField] public float LightAttackDamageMultiplier { get; set; } = 1;
        [field: SerializeField] public float HeavyAttackDamageMultiplier { get; set; } = 1.5f;
        [field: SerializeField] public float ChargedAttackDamageMultiplier { get; set; } = 2;

        [Header("Stamina Costs")]
        [field: SerializeField] public float BaseStaminaCost { get; set; }
        [field: SerializeField] public float LightAttackStaminaMultiplier { get; set; } = 1;
        [field: SerializeField] public float HeavyAttackStaminaMultiplier { get; set; } = 1.5f;

        [Header("Actions")]
        [field: SerializeField] public WeaponItemAction LightAttackAction { get; set; }
        [field: SerializeField] public WeaponItemAction HeavyAttackAction { get; set; }
    }
}