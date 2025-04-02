using UnityEngine;

namespace ProjectPipe
{
    public class WeaponModelInstantiationSlot : MonoBehaviour
    {
        [field: SerializeField] public WeaponModelSlot Slot { get; set; }
        public GameObject CurrentWeapon { get; private set; }

        public void UnloadWeapon()
        {
            if (CurrentWeapon) Destroy(CurrentWeapon);
        }

        public void LoadWeapon(GameObject weaponModel)
        {
            CurrentWeapon = weaponModel;
            weaponModel.transform.SetParent(transform);
            weaponModel.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            weaponModel.transform.localScale = Vector3.one;
        }
    }
}