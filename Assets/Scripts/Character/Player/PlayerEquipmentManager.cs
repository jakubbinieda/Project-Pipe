using UnityEngine;

namespace ProjectPipe
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        private GameObject _equippedWeaponModel;
        private WeaponItem _noWeapon;
        private PlayerManager _playerManager;
        private GameObject _sheathedWeaponModel;
        private WeaponModelInstantiationSlot _slotHand;
        private WeaponModelInstantiationSlot _slotHips;

        protected override void Awake()
        {
            base.Awake();

            _playerManager = GetComponent<PlayerManager>();

            foreach (var slot in GetComponentsInChildren<WeaponModelInstantiationSlot>())
                switch (slot.Slot)
                {
                    case WeaponModelSlot.Hand: _slotHand = slot; break;
                    case WeaponModelSlot.Hips: _slotHips = slot; break;
                }
        }

        protected override void Start()
        {
            base.Start();

            EquippedWeapon = !_playerManager.PlayerInventoryManager.CurrentWeapon
                ? Instantiate(WorldItemDatabase.Instance.NoWeapon)
                : Instantiate(_playerManager.PlayerInventoryManager.CurrentWeapon);

            _noWeapon = Instantiate(WorldItemDatabase.Instance.NoWeapon);

            _equippedWeaponModel = Instantiate(EquippedWeapon.Prefab);
            _equippedWeaponModel.GetComponent<WeaponManager>().Initialize(_playerManager, EquippedWeapon);
            _slotHand.LoadWeapon(_equippedWeaponModel);

            _sheathedWeaponModel = Instantiate(_noWeapon.Prefab);
            _sheathedWeaponModel.GetComponent<WeaponManager>().Initialize(_playerManager, _noWeapon);
            _slotHips.LoadWeapon(_sheathedWeaponModel);
        }

        protected override void Update()
        {
            if (PlayerInputManager.Instance.ToggleWeaponInput)
            {
                PlayerInputManager.Instance.ToggleWeaponInput = false;
                ToggleWeapon();
            }
        }

        private void ToggleWeapon()
        {
            if (!_playerManager.PlayerInventoryManager.CurrentWeapon) return;

            if (_playerManager.IsPerformingAction) return;

            if (!_playerManager.IsGrounded) return;

            _playerManager.PlayerAnimatorManager.PlayTargetAnimation("Swap_Weapon_01", false, false, true, true);
        }

        public void MoveWeaponHandToHip()
        {
            // This fires on animation so all the checks were already successful

            if (_slotHand.CurrentWeapon == _equippedWeaponModel)
            {
                _slotHand.LoadWeapon(_sheathedWeaponModel);
                _slotHips.LoadWeapon(_equippedWeaponModel);
            }
            else
            {
                _slotHips.LoadWeapon(_sheathedWeaponModel);
                _slotHand.LoadWeapon(_equippedWeaponModel);
            }

            (EquippedWeapon, _noWeapon) = (_noWeapon, EquippedWeapon);
            (_sheathedWeaponModel, _equippedWeaponModel) = (_equippedWeaponModel, _sheathedWeaponModel);
        }

        public override void OpenDamageCollider()
        {
            _equippedWeaponModel.GetComponent<WeaponManager>().EnableDamageCollider();
        }

        public override void CloseDamageCollider()
        {
            _equippedWeaponModel.GetComponent<WeaponManager>().DisableDamageCollider();
        }
    }
}