namespace ProjectPipe
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        private PlayerManager _playerManager;

        protected override void Awake()
        {
            base.Awake();

            _playerManager = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (PlayerInputManager.Instance.LightAttackInput) HandleLightAttack();
            if (PlayerInputManager.Instance.HeavyAttackInput) HandleHeavyAttack();
            HandleChargedAttack();
        }

        private void HandleLightAttack()
        {
            PlayerInputManager.Instance.LightAttackInput = false;

            _playerManager.PlayerEquipmentManager.EquippedWeapon.LightAttackAction.AttemptToPerformAction(
                _playerManager, _playerManager.PlayerEquipmentManager.EquippedWeapon);
        }

        private void HandleHeavyAttack()
        {
            PlayerInputManager.Instance.HeavyAttackInput = false;

            _playerManager.PlayerEquipmentManager.EquippedWeapon.HeavyAttackAction.AttemptToPerformAction(
                _playerManager, _playerManager.PlayerEquipmentManager.EquippedWeapon);
        }

        private void HandleChargedAttack()
        {
            if (_playerManager.PlayerCombatManager.IsChargingHeavyAttack ==
                PlayerInputManager.Instance.ChargedAttackInput)
                return;

            _playerManager.PlayerCombatManager.IsChargingHeavyAttack.Value =
                PlayerInputManager.Instance.ChargedAttackInput;

            if (_playerManager.PlayerCombatManager.IsChargingHeavyAttack.Value)
                _playerManager.PlayerEquipmentManager.EquippedWeapon.HeavyAttackAction.AttemptToPerformAction(
                    _playerManager, _playerManager.PlayerEquipmentManager.EquippedWeapon);
        }
    }
}