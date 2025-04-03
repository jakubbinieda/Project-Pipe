namespace ProjectPipe
{
    public class PlayerManager : CharacterManager
    {
        public PlayerAnimatorManager PlayerAnimatorManager { get; private set; }
        public PlayerCombatManager PlayerCombatManager { get; private set; }
        public PlayerEquipmentManager PlayerEquipmentManager { get; private set; }
        public PlayerInventoryManager PlayerInventoryManager { get; private set; }
        public PlayerLocomotionManager PlayerLocomotionManager { get; private set; }
        public PlayerStatsManager PlayerStatsManager { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            PlayerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            PlayerCombatManager = GetComponent<PlayerCombatManager>();
            PlayerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            PlayerInventoryManager = GetComponent<PlayerInventoryManager>();
            PlayerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            PlayerStatsManager = GetComponent<PlayerStatsManager>();
        }

        protected override void Start()
        {
            base.Start();

            PlayerCamera.Instance.PlayerManager = this;

            // TODO: Think of something better
            PlayerStatsManager.SetMaxStamina(100);
            PlayerStatsManager.SetMaxHealth(100);
        }
    }
}