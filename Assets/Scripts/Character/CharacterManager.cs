using UnityEngine;

namespace ProjectPipe
{
    public class CharacterManager : MonoBehaviour
    {
        [field: SerializeField] public bool ApplyRootMotion { get; set; }
        [field: SerializeField] public bool CanRotate { get; set; } = true;
        [field: SerializeField] public bool CanMove { get; set; } = true;
        [field: SerializeField] public bool IsGrounded { get; set; } = true;
        [field: SerializeField] public bool IsJumping { get; set; }
        [field: SerializeField] public bool IsPerformingAction { get; set; }
        [field: SerializeField] public bool IsSprinting { get; set; }

        public CharacterController CharacterController { get; private set; }
        public Animator Animator { get; private set; }

        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);

            Animator = GetComponent<Animator>();
            CharacterController = GetComponent<CharacterController>();
        }

        protected virtual void Start()
        {
        }
    }
}