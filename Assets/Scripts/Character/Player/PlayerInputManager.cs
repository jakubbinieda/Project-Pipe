using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectPipe
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance;

        [field: SerializeField] public Vector2 CameraInput { get; private set; }
        [field: SerializeField] public Vector2 MovementInput { get; private set; }
        [field: SerializeField] public bool DodgeInput { get; set; }
        [field: SerializeField] public bool JumpInput { get; set; }
        [field: SerializeField] public bool SprintInput { get; set; }
        [field: SerializeField] public bool LightAttackInput { get; set; }
        [field: SerializeField] public bool HeavyAttackInput { get; set; }
        [field: SerializeField] public bool ChargedAttackInput { get; set; }
        [field: SerializeField] public bool ToggleWeaponInput { get; set; }
        [SerializeField] private bool attackToggleComposite;

        private PlayerControls _playerControls;

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
            }

            _playerControls.Enable();
        }
    }
}