using System.Collections;
using UnityEngine;

namespace ProjectPipe
{
    public class CharacterManager : MonoBehaviour
    {
        [field: Header("Flags")]
        [field: SerializeField] public bool ApplyRootMotion { get; set; }
        [field: SerializeField] public bool CanRotate { get; set; } = true;
        [field: SerializeField] public bool CanMove { get; set; } = true;
        [field: SerializeField] public bool IsGrounded { get; set; } = true;
        [field: SerializeField] public bool IsJumping { get; set; }
        [field: SerializeField] public bool IsPerformingAction { get; set; }
        [field: SerializeField] public bool IsSprinting { get; set; }

        [field: Header("Status")]
        [field: SerializeField] public bool IsDead { get; private set; }

        public CharacterController CharacterController { get; private set; }
        public CharacterStatsManager CharacterStatsManager { get; private set; }
        public Animator Animator { get; private set; }
        public CharacterEffectsManager CharacterEffectsManager { get; private set; }
        public CharacterAnimatorManager CharacterAnimatorManager { get; private set; }

        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);

            Animator = GetComponent<Animator>();
            CharacterController = GetComponent<CharacterController>();
            CharacterStatsManager = GetComponent<CharacterStatsManager>();
            CharacterEffectsManager = GetComponent<CharacterEffectsManager>();
            CharacterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        }

        protected virtual void Start()
        {
        }

        public IEnumerator ProcessDeathEvent()
        {
            IsDead = true;

            CharacterAnimatorManager.PlayTargetAnimation("Death_01", true, true);

            yield return new WaitForSeconds(5f);
        }
    }
}