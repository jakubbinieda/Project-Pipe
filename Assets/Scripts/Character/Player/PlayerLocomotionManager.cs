using UnityEngine;

namespace ProjectPipe
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        [SerializeField] private float walkSpeed = 1.5f;
        [SerializeField] private float runSpeed = 4.5f;
        [SerializeField] private float sprintSpeed = 7f;
        [SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private float jumpHeight = 1.5f;
        [SerializeField] private float jumpForwardSpeed = 6.5f;
        [SerializeField] private float freeFallSpeed = 1.5f;

        private Vector3 _jumpDirection;
        private float _moveAmount;
        private PlayerManager _playerManager;

        protected override void Awake()
        {
            base.Awake();

            _playerManager = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (PlayerInputManager.Instance.DodgeInput)
            {
                PlayerInputManager.Instance.DodgeInput = false;
                AttemptToDodge();
            }

            if (PlayerInputManager.Instance.JumpInput)
            {
                PlayerInputManager.Instance.JumpInput = false;
                AttemptToJump();
            }

            HandleGroundedLocomotion();
            HandleJumpingMovement();
            HandleRotation();
        }

        private void HandleGroundedLocomotion()
        {
            if (!_playerManager.CanMove) return;

            if (_playerManager.IsPerformingAction) _playerManager.IsSprinting = false;

            _playerManager.IsSprinting = PlayerInputManager.Instance.MovementInput.magnitude != 0 &&
                                         PlayerInputManager.Instance.SprintInput;

            var moveDirection = PlayerCamera.Instance.transform.forward * PlayerInputManager.Instance.MovementInput.y;
            moveDirection += PlayerCamera.Instance.transform.right * PlayerInputManager.Instance.MovementInput.x;
            moveDirection.Normalize();
            moveDirection.y = 0;

            _moveAmount = Mathf.Clamp01(PlayerInputManager.Instance.MovementInput.magnitude);
            // Clamp it so it becomes 0.5 or 1
            _moveAmount = Mathf.Ceil(_moveAmount * 2) / 2f;

            if (_playerManager.IsSprinting)
            {
                _playerManager.CharacterController.Move(sprintSpeed * Time.deltaTime * moveDirection);
                _moveAmount = 2;
            }
            else if (_moveAmount > 0.5)
            {
                _playerManager.CharacterController.Move(runSpeed * Time.deltaTime * moveDirection);
            }
            else
            {
                _playerManager.CharacterController.Move(walkSpeed * Time.deltaTime * moveDirection);
            }

            if (_playerManager) _playerManager.PlayerAnimatorManager.UpdateAnimatorLocomotionValues(0, _moveAmount);
        }

        private void HandleJumpingMovement()
        {
            if (_playerManager.IsJumping)
                _playerManager.CharacterController.Move(jumpForwardSpeed * Time.deltaTime * _jumpDirection);

            if (!_playerManager.IsGrounded)
            {
                var freeFallDirection =
                    PlayerCamera.Instance.transform.forward * PlayerInputManager.Instance.MovementInput.y;
                freeFallDirection +=
                    PlayerCamera.Instance.transform.right * PlayerInputManager.Instance.MovementInput.x;

                _playerManager.CharacterController.Move(freeFallSpeed * Time.deltaTime * freeFallDirection);
            }
        }

        private void HandleRotation()
        {
            if (!_playerManager.CanRotate) return;

            var targetRotationDirection =
                PlayerCamera.Instance.transform.forward * PlayerInputManager.Instance.MovementInput.y;
            targetRotationDirection +=
                PlayerCamera.Instance.transform.right * PlayerInputManager.Instance.MovementInput.x;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;

            if (targetRotationDirection == Vector3.zero) targetRotationDirection = transform.forward;

            var turnRotation = Quaternion.LookRotation(targetRotationDirection);
            var newRotation = Quaternion.Slerp(transform.rotation, turnRotation, Time.deltaTime * rotationSpeed);

            transform.rotation = newRotation;
        }

        private void AttemptToDodge()
        {
            if (_playerManager.IsPerformingAction) return;

            if (!_playerManager.IsGrounded) return;

            if (PlayerInputManager.Instance.MovementInput.magnitude > 0)
            {
                // Roll
                var rollDirection = PlayerCamera.Instance.CameraObject.transform.forward *
                                    PlayerInputManager.Instance.MovementInput.y;
                rollDirection += PlayerCamera.Instance.CameraObject.transform.right *
                                 PlayerInputManager.Instance.MovementInput.x;

                rollDirection.y = 0;
                rollDirection.Normalize();

                var playerRotation = Quaternion.LookRotation(rollDirection);
                _playerManager.transform.rotation = playerRotation;

                _playerManager.PlayerAnimatorManager.PlayTargetAnimation("Roll_Forward_01", true, true);
            }
            else
            {
                // Backstep
                _playerManager.PlayerAnimatorManager.PlayTargetAnimation("Backstep_01", true, true);
            }
        }

        private void AttemptToJump()
        {
            if (_playerManager.IsPerformingAction) return;

            if (_playerManager.IsJumping) return;

            if (!_playerManager.IsGrounded) return;

            _jumpDirection = PlayerCamera.Instance.CameraObject.transform.forward *
                             PlayerInputManager.Instance.MovementInput.y;
            _jumpDirection += PlayerCamera.Instance.CameraObject.transform.right *
                              PlayerInputManager.Instance.MovementInput.x;
            _jumpDirection.y = 0;

            if (_jumpDirection != Vector3.zero)
            {
                if (_playerManager.IsSprinting) _jumpDirection *= 1;
                else if (_moveAmount > 0.5) _jumpDirection *= 0.5f;
                else if (_moveAmount <= 0.5) _jumpDirection *= 0.25f;
            }

            _playerManager.PlayerAnimatorManager.PlayTargetAnimation("Main_Jump_Start_01", false, true);
            _playerManager.IsJumping = true;
        }

        public void UpdateJumpVelocity()
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }
    }
}