using UnityEngine;

namespace ProjectPipe
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
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
    }
}