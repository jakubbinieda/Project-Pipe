using UnityEngine;

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
            }

            _playerControls.Enable();
        }
    }
}