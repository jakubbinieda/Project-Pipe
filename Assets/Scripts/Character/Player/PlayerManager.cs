namespace ProjectPipe
{
    public class PlayerManager : CharacterManager
    {
        public PlayerAnimatorManager PlayerAnimatorManager { get; private set; }
        public PlayerLocomotionManager PlayerLocomotionManager { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            PlayerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            PlayerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        }

        protected override void Start()
        {
            base.Start();

            PlayerCamera.Instance.PlayerManager = this;
        }
    }
}