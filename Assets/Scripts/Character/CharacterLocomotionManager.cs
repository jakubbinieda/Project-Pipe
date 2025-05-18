using UnityEngine;

namespace ProjectPipe
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        [Header("Fall Settings")]
        [SerializeField] protected Vector3 yVelocity;
        [SerializeField] protected float fallStartVelocity = -5f;
        [SerializeField] protected float gravityForce = -9.81f;
        [SerializeField] private float groundCheckSphereRadius = 0.2f;
        [SerializeField] private LayerMask groundLayer;

        [Header("Flags")]
        [field: SerializeField] public bool IsRolling { get; set; }

        private CharacterManager _characterManager;
        private bool _fallVelocitySet;
        private float _inAirTimer;
        private int _inAirTimerHash;
        private int _isGroundedHash;

        protected virtual void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();

            _isGroundedHash = Animator.StringToHash("IsGrounded");
            _inAirTimerHash = Animator.StringToHash("InAirTimer");
        }

        protected virtual void Update()
        {
            HandleGroundCheck();

            if (_characterManager.IsGrounded)
            {
                if (yVelocity.y < 0)
                {
                    _inAirTimer = 0;
                    _fallVelocitySet = false;
                    yVelocity.y = gravityForce;
                }
            }
            else
            {
                if (!_characterManager.IsJumping && !_fallVelocitySet)
                {
                    _fallVelocitySet = true;
                    yVelocity.y = fallStartVelocity;
                }

                _inAirTimer += Time.deltaTime;
                _characterManager.Animator.SetFloat(_inAirTimerHash, _inAirTimer);
                yVelocity.y += gravityForce * Time.deltaTime;
            }

            _characterManager.CharacterController.Move(yVelocity * Time.deltaTime);
            _characterManager.Animator.SetBool(_isGroundedHash, _characterManager.IsGrounded);
        }

        protected virtual void OnAnimatorMove()
        {
            if (!_characterManager.ApplyRootMotion) return;

            var velocity = _characterManager.Animator.deltaPosition;
            _characterManager.CharacterController.Move(velocity);
            _characterManager.transform.rotation *= _characterManager.Animator.deltaRotation;
        }

        protected virtual void OnDrawGizmosSelected()
        {
            if (!_characterManager) return;

            Gizmos.DrawWireSphere(_characterManager.transform.position, groundCheckSphereRadius);
        }

        private void HandleGroundCheck()
        {
            _characterManager.IsGrounded = Physics.CheckSphere(_characterManager.transform.position,
                groundCheckSphereRadius, groundLayer);
        }

        public void EnableRotate()
        {
            _characterManager.CanRotate = true;
        }
        
        public void DisableRotate()
        {
            _characterManager.CanRotate = false;
        }
        
        private void EnableCanRotate()
        {
            _characterManager.CanRotate = true;
        }

        private void DisableCanRotate()
        {
            _characterManager.CanRotate = false;
        }
    }
}