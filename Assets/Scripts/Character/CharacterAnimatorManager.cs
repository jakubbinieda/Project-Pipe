using UnityEngine;

namespace ProjectPipe
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        [Header("Directional Hit Animations")]
        [field: SerializeField] private string hitForwardMedium01 = "Hit_Forward_Medium_01";
        [field: SerializeField] private string hitBackwardMedium01 = "Hit_Backward_Medium_01";
        [field: SerializeField] private string hitLeftMedium01 = "Hit_Left_Medium_01";
        [field: SerializeField] private string hitRightMedium01 = "Hit_Right_Medium_01";

        private CharacterManager _characterManager;
        private int _horizontalHash;
        private int _verticalHash;

        protected virtual void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();

            _horizontalHash = Animator.StringToHash("Horizontal");
            _verticalHash = Animator.StringToHash("Vertical");
        }

        public void UpdateAnimatorLocomotionValues(float horizontalValue, float verticalValue)
        {
            _characterManager.Animator.SetFloat(_horizontalHash, horizontalValue, 0.1f, Time.deltaTime);
            _characterManager.Animator.SetFloat(_verticalHash, verticalValue, 0.1f, Time.deltaTime);
        }

        public void PlayTargetAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion,
            bool canRotate = false, bool canMove = false)
        {
            _characterManager.ApplyRootMotion = applyRootMotion;
            _characterManager.Animator.CrossFade(targetAnimation, 0.2f);
            _characterManager.IsPerformingAction = isPerformingAction;
            _characterManager.CanRotate = canRotate;
            _characterManager.CanMove = canMove;
        }

        public void PlayTargetAttackActionAnimation(AttackType attackType, string targetAnimation,
            bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false)
        {
            _characterManager.CharacterCombatManager.CurrentAttackType = attackType;
            _characterManager.CharacterCombatManager.LastAttackAnimation = targetAnimation;

            _characterManager.ApplyRootMotion = applyRootMotion;
            _characterManager.Animator.CrossFade(targetAnimation, 0.2f);
            _characterManager.IsPerformingAction = isPerformingAction;
            _characterManager.CanRotate = canRotate;
            _characterManager.CanMove = canMove;
        }

        public void PlayTargetDirectionalDamageActionAnimation(float angleHitFrom, bool isPerformingAction,
            bool applyRootMotion, bool canRotate = false, bool canMove = false)
        {
            var damageAnimation = angleHitFrom switch
            {
                >= 145 and <= 180 => hitForwardMedium01,
                <= -145 and >= -180 => hitForwardMedium01,
                >= -45 and <= 45 => hitBackwardMedium01,
                >= -144 and <= -45 => hitLeftMedium01,
                >= 44 and <= 144 => hitRightMedium01,
                _ => ""
            };

            PlayTargetAnimation(damageAnimation, true, true);
        }
    }
}