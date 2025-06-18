using UnityEngine;

namespace ProjectPipe
{
    public class ResetActionFlag : StateMachineBehaviour
    {
        private CharacterManager _characterManager;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!_characterManager) _characterManager = animator.GetComponent<CharacterManager>();

            _characterManager.IsPerformingAction = false;
            _characterManager.ApplyRootMotion = false;
            _characterManager.CanMove = true;
            _characterManager.CanRotate = true;
            _characterManager.IsJumping = false;
            _characterManager.CharacterCombatManager.DisableCanDoCombo();
            _characterManager.CharacterCombatManager.DisableCanDoBackstepAttack();
            _characterManager.CharacterCombatManager.DisableCanDoRollAttack();
            _characterManager.CharacterLocomotionManager.IsRolling = false;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}