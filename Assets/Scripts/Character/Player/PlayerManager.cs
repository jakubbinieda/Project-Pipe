namespace ProjectPipe
{
    public class PlayerManager : CharacterManager
    {
        public PlayerAnimatorManager PlayerAnimatorManager { get; private set; }
        public PlayerLocomotionManager PlayerLocomotionManager { get; private set; }
        public PlayerStatsManager PlayerStatsManager { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            PlayerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            PlayerAnimatorManager = GetComponent<PlayerAnimatorManager>();
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