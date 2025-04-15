using UnityEngine;

namespace ProjectPipe
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        [Header("Locomotion Settings")]
        [SerializeField] private float walkSpeed = 1.5f;
        [SerializeField] private float runSpeed = 4.5f;
        [SerializeField] private float sprintSpeed = 7f;
        [SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private float jumpHeight = 1.5f;
        [SerializeField] private float jumpForwardSpeed = 6.5f;
        [SerializeField] private float freeFallSpeed = 1.5f;

        [Header("Locomotion Stamina Costs")]
        [SerializeField] private int sprintStaminaCost = 2;
        [SerializeField] private int jumpStaminaCost = 15;
        [SerializeField] private int dodgeStaminaCost = 15;

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
            HandlePlayerRotation();
        }

        private void HandleGroundedLocomotion()
        {
            if (!_playerManager.CanMove) return;

            _playerManager.IsSprinting = PlayerInputManager.Instance.MovementInput.magnitude != 0 &&
                                         PlayerInputManager.Instance.SprintInput;

            if (_playerManager.IsPerformingAction) _playerManager.IsSprinting = false;

            if (!_playerManager.PlayerStatsManager.CanAffordStaminaCost(sprintStaminaCost * Time.deltaTime))
                _playerManager.IsSprinting = false;

            var moveDirection = PlayerCamera.Instance.transform.forward * PlayerInputManager.Instance.MovementInput.y;
            moveDirection += PlayerCamera.Instance.transform.right * PlayerInputManager.Instance.MovementInput.x;
            moveDirection.Normalize();
            moveDirection.y = 0;

            _moveAmount = ClampInput(PlayerInputManager.Instance.MovementInput.magnitude);

            if (_playerManager.IsSprinting)
            {
                _playerManager.CharacterController.Move(sprintSpeed * Time.deltaTime * moveDirection);
                _moveAmount = 2;
                _playerManager.PlayerStatsManager.SpendStamina(sprintStaminaCost * Time.deltaTime);
            }
            else if (_moveAmount > 0.5)
            {
                _playerManager.CharacterController.Move(runSpeed * Time.deltaTime * moveDirection);
            }
            else
            {
                _playerManager.CharacterController.Move(walkSpeed * Time.deltaTime * moveDirection);
            }

            if (!PlayerCamera.Instance.IsLockedOn || _playerManager.IsSprinting)
                _playerManager.PlayerAnimatorManager.UpdateAnimatorLocomotionValues(0, _moveAmount);
            else
                _playerManager.PlayerAnimatorManager.UpdateAnimatorLocomotionValues(
                    ClampInput(PlayerInputManager.Instance.MovementInput.x),
                    ClampInput(PlayerInputManager.Instance.MovementInput.y));
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

        private void HandlePlayerRotation()
        {
            if (_playerManager.IsDead || !_playerManager.CanRotate) return;

            Vector3 targetPlayerDirection;

            if (PlayerCamera.Instance.IsLockedOn && !_playerManager.IsSprinting && !IsRolling)
            {
                targetPlayerDirection = _playerManager.PlayerCombatManager.CurrentTarget.transform.position -
                                        _playerManager.transform.position;
                targetPlayerDirection.y = 0;
                targetPlayerDirection.Normalize();
            }
            else
            {
                targetPlayerDirection = PlayerCamera.Instance.CameraObject.transform.forward *
                                        PlayerInputManager.Instance.MovementInput.y;
                targetPlayerDirection += PlayerCamera.Instance.CameraObject.transform.right *
                                         PlayerInputManager.Instance.MovementInput.x;
                targetPlayerDirection.y = 0;
                targetPlayerDirection.Normalize();

                if (targetPlayerDirection == Vector3.zero) targetPlayerDirection = _playerManager.transform.forward;
            }

            var targetPlayerRotation = Quaternion.LookRotation(targetPlayerDirection);
            var newRotation = Quaternion.Slerp(_playerManager.transform.rotation, targetPlayerRotation,
                Time.deltaTime * rotationSpeed);

            _playerManager.transform.rotation = newRotation;
        }

        private void AttemptToDodge()
        {
            if (_playerManager.IsPerformingAction) return;

            if (!_playerManager.IsGrounded) return;

            if (!_playerManager.PlayerStatsManager.CanAffordStaminaCost(dodgeStaminaCost)) return;

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
                IsRolling = true;
            }
            else
            {
                // Backstep
                _playerManager.PlayerAnimatorManager.PlayTargetAnimation("Backstep_01", true, true);
            }

            _playerManager.PlayerStatsManager.SpendStamina(dodgeStaminaCost);
        }

        private void AttemptToJump()
        {
            if (_playerManager.IsPerformingAction) return;

            if (_playerManager.IsJumping) return;

            if (!_playerManager.IsGrounded) return;

            if (!_playerManager.PlayerStatsManager.CanAffordStaminaCost(jumpStaminaCost)) return;

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

            _playerManager.PlayerStatsManager.SpendStamina(jumpStaminaCost);
        }

        private float ClampInput(float input)
        {
            return input switch
            {
                > 0.5f => 1f,
                > 0f => 0.5f,
                < -0.5f => -1f,
                < 0f => -0.5f,
                _ => 0f
            };
        }

        public void UpdateJumpVelocity()
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }
    }
}