using UnityEngine;

namespace ProjectPipe
{
    [CreateAssetMenu(menuName = "AI/States/AttackState")]
    public class AttackState : AIState
    {
        [field: Header("State Flags")]
        [field: SerializeField] protected bool hasPerformedAttack = false;
        [field: SerializeField] protected bool hasPerformedCombo = false;
        
        [field: Header("Pivot After Attack")]
        [field: SerializeField] protected bool pivotAfterAttack = false;
        
        [HideInInspector] public AICharacterAttackAction currentAttack;
        [HideInInspector] public bool willPerformCombo;

        public override AIState Tick(AICharacterManager aiCharacterManager)
        {
            if (aiCharacterManager.AICharacterCombatManager.CurrentTarget == null)
            {
                return SwitchState(aiCharacterManager, aiCharacterManager.IdleState);
            }

            if (aiCharacterManager.AICharacterCombatManager.CurrentTarget.IsDead)
            {
                return SwitchState(aiCharacterManager, aiCharacterManager.IdleState);
            }
            
            aiCharacterManager.AICharacterCombatManager.RotateTowardsTargetWhileAttacking(aiCharacterManager);
            
            aiCharacterManager.CharacterAnimatorManager.UpdateAnimatorLocomotionValues(0, 0);
            
            if (willPerformCombo && !hasPerformedCombo)
            {
                if (currentAttack.comboAction != null)
                {
                    // TODO: Implement combo action
                    
                    // hasPerformedCombo = true;
                    // currentAttack.comboAction.AttempToPerformAction(aiCharacterManager);
                }
            }
            
            if (aiCharacterManager.IsPerformingAction)
                return this;

            if (!hasPerformedAttack)
            {
                if (aiCharacterManager.AICharacterCombatManager.actionRecoveryTime > 0)
                    return this;
                
                PerformAttack(aiCharacterManager);
                return this;
            }

            if (pivotAfterAttack)
                aiCharacterManager.AICharacterCombatManager.PivotTowardsTarget(aiCharacterManager);
            
            return SwitchState(aiCharacterManager, aiCharacterManager.CombatStanceState);
        }

        protected void PerformAttack(AICharacterManager aiCharacterManager)
        {
            hasPerformedAttack = true;
            currentAttack.AttemptToPerformAction(aiCharacterManager);
            aiCharacterManager.AICharacterCombatManager.actionRecoveryTime = currentAttack.actionRecoveryTime;
            
        }

        protected override void ResetStateFlags(AICharacterManager aiCharacterManager)
        {
            base.ResetStateFlags(aiCharacterManager);
            
            hasPerformedAttack = false;
            hasPerformedCombo = false;
        }
    }
}