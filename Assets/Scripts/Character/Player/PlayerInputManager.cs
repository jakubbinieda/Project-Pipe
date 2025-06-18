using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectPipe
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance;

        [Header("Camera")]
        [field: SerializeField] public Vector2 CameraInput { get; private set; }
        [field: SerializeField] public bool LockOnInput { get; set; }
        [field: SerializeField] public bool ChangeLockOnToLeft { get; set; }
        [field: SerializeField] public bool ChangeLockOnToRight { get; set; }

        [Header("Movement")]
        [field: SerializeField] public Vector2 MovementInput { get; private set; }
        [field: SerializeField] public bool DodgeInput { get; set; }
        [field: SerializeField] public bool JumpInput { get; set; }
        [field: SerializeField] public bool SprintInput { get; set; }

        [Header("Actions")]
        [field: SerializeField] public bool LightAttackInput { get; set; }
        [field: SerializeField] public bool HeavyAttackInput { get; set; }
        [field: SerializeField] public bool ChargedAttackInput { get; set; }
        [field: SerializeField] public bool ToggleWeaponInput { get; set; }
        [SerializeField] private bool attackToggleComposite;

        [Header("Queued Inputs")]
        [SerializeField] private float queInputTimer;
        [SerializeField] private float queInputMaxTime = 0.35f;
        [SerializeField] private bool isQueInputActive;
        [SerializeField] private bool queLightAttackInput;
        [SerializeField] private bool queHeavyAttackInput;

        [Header("UI")]
        [field: SerializeField] public bool PauseInput { get; private set; }

        private PlayerControls _playerControls;

        public PlayerManager PlayerManager { get; set; }

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            HandleQueuedInputs();
        }

        private void OnEnable()
        {
            if (_playerControls == null)
            {
                _playerControls = new PlayerControls();

                _playerControls.PlayerMovement.Locomotion.performed += ctx => MovementInput = ctx.ReadValue<Vector2>();
                _playerControls.PlayerMovement.Dodge.performed += ctx => DodgeInput = true;
                _playerControls.PlayerMovement.Sprint.performed += ctx => SprintInput = true;
                _playerControls.PlayerMovement.Sprint.canceled += ctx => SprintInput = false;
                _playerControls.PlayerMovement.Jump.performed += ctx => JumpInput = true;

                _playerControls.PlayerCamera.Movement.performed += ctx => CameraInput = ctx.ReadValue<Vector2>();
                _playerControls.PlayerCamera.LockOn.performed += ctx => LockOnInput = true;
                _playerControls.PlayerCamera.ChangeLockOnToLeft.performed += ctx => ChangeLockOnToLeft = true;
                _playerControls.PlayerCamera.ChangeLockOnToRight.performed += ctx => ChangeLockOnToRight = true;

                _playerControls.PlayerActions.AttackTypeToggle.performed += ctx => attackToggleComposite = true;
                _playerControls.PlayerActions.AttackTypeToggle.canceled += ctx => attackToggleComposite = false;

                _playerControls.PlayerActions.LightAttack.performed += ctx =>
                {
                    if (!attackToggleComposite) LightAttackInput = true;
                };

                _playerControls.PlayerActions.HeavyAttack.performed += ctx =>
                {
                    if (ctx.action.activeControl.device is Gamepad || attackToggleComposite) HeavyAttackInput = true;
                };

                _playerControls.PlayerActions.ChargedAttack.performed += ctx => ChargedAttackInput = true;
                _playerControls.PlayerActions.ChargedAttack.canceled += ctx => ChargedAttackInput = false;
                _playerControls.PlayerActions.ToggleWeapon.performed += ctx => ToggleWeaponInput = true;

                _playerControls.PlayerActions.QueLightAttack.performed += ctx =>
                {
                    if (!attackToggleComposite) EnqueueInput(ref queLightAttackInput);
                };

                _playerControls.PlayerActions.QueHeavyAttack.performed += ctx =>
                {
                    if (ctx.action.activeControl.device is Gamepad || attackToggleComposite)
                        EnqueueInput(ref queHeavyAttackInput);
                };
                
                _playerControls.UI.Pause.performed += ctx => PauseInput = true;
                _playerControls.UI.Pause.canceled += ctx => PauseInput = false;
            }

            _playerControls.Enable();
        }

        public void EnterGameplayMode()
        {
            ResetAllInputs();
            _playerControls.Enable();
            DisableCursor();
        }

        public void ExitGameplayMode()
        {
            ResetAllInputs();
            _playerControls.Disable();
            EnableCursor();
        }

        private void ResetAllInputs()
        {
            CameraInput = Vector2.zero;
            LockOnInput = false;
            ChangeLockOnToLeft = false;
            ChangeLockOnToRight = false;

            MovementInput = Vector2.zero;
            DodgeInput = false;
            JumpInput = false;
            SprintInput = false;

            LightAttackInput = false;
            HeavyAttackInput = false;
            ChargedAttackInput = false;
            ToggleWeaponInput = false;
            attackToggleComposite = false;

            PauseInput = false;
        }

        private void EnableCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        private void DisableCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        private void EnqueueInput(ref bool queInput)
        {
            // TODO: Check for open UI

            if (!PlayerManager) return;

            if (!PlayerManager.IsPerformingAction && !PlayerManager.IsJumping) return;

            queInput = true;
            queInputTimer = queInputMaxTime;
            isQueInputActive = true;
        }

        private void ProcessQueuedInputs()
        {
            if (queLightAttackInput) LightAttackInput = true;
            if (queHeavyAttackInput) HeavyAttackInput = true;
        }

        private void HandleQueuedInputs()
        {
            if (!isQueInputActive) return;

            if (queInputTimer > 0)
            {
                queInputTimer -= Time.deltaTime;
                ProcessQueuedInputs();
            }
            else
            {
                queLightAttackInput = false;
                queHeavyAttackInput = false;
                isQueInputActive = false;
                queInputTimer = 0;
            }
        }
    }
}
