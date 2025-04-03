using UnityEngine;

namespace ProjectPipe
{
    public class CharacterInventoryManager : MonoBehaviour
    {
        [field: SerializeField] public WeaponItem CurrentWeapon { get; set; }
    }
}