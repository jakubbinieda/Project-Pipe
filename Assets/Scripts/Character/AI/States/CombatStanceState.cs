using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

namespace ProjectPipe
{
    [CreateAssetMenu(menuName = "AI/States/CombatStanceState")]
    public class CombatStanceState : AIState
    {
        [field: Header("Attacks")]
        public List<AICharacterAttackAction> aiCharacterAttacks;
        protected List<AICharacterAttackAction> potentialAttacks;
        private AICharacterAttackAction chosenAttack;
        private AICharacterAttackAction previousAttack;
        protected bool hasAttack = false;
        
        [field: Header("Combo")]
        [field: SerializeField] protected bool canPerformCombo;
        [field: SerializeField] protected bool chanceToPerformCombo;
        [field: SerializeField] private bool hasRolledForComboOnce = false;
        
        [field: Header("Engagement Distance")]
        [field: SerializeField] public float maximumEngagementDistance = 5;

        public override AIState Tick(AICharacterManager aiCharacterManager)
        {
            if (aiCharacterManager.IsPerformingAction)
                return this;
            
            if(!aiCharacterManager.NavMeshAgent.enabled)
                aiCharacterManager.NavMeshAgent.enabled = true;

            if (!aiCharacterManager.IsMoving)
            {
                // TODO: remove hardcoded values
                if (aiCharacterManager.AICharacterCombatManager.ViewableAngle < -30 ||
                    aiCharacterManager.AICharacterCombatManager.ViewableAngle > 30)
                {
                    aiCharacterManager.AICharacterCombatManager.PivotTowardsTarget(aiCharacterManager);
                }
            }
            
            aiCharacterManager.AICharacterCombatManager.RotateTowardsAgent(aiCharacterManager);

            if (aiCharacterManager.AICharacterCombatManager.CurrentTarget == null)
            {
                    return SwitchState(aiCharacterManager, aiCharacterManager.IdleState);
            }

            if (!hasAttack)
            {
                GetNewAttack(aiCharacterManager);
            }
            else
            {
                aiCharacterManager.AttackState.currentAttack = chosenAttack;
                return SwitchState(aiCharacterManager, aiCharacterManager.AttackState);
            }
                
            if (aiCharacterManager.AICharacterCombatManager.DistanceFromTarget > maximumEngagementDistance)
            {
                return SwitchState(aiCharacterManager, aiCharacterManager.PursueTargetState);
            }
            
            NavMeshPath path = new NavMeshPath();
            aiCharacterManager.NavMeshAgent.CalculatePath(aiCharacterManager.AICharacterCombatManager.CurrentTarget.transform.position, path);
            aiCharacterManager.NavMeshAgent.SetPath(path);
            return this; 
        }
        
        protected virtual void GetNewAttack(AICharacterManager aiCharacterManager)
        {
            potentialAttacks = new List<AICharacterAttackAction>();
            foreach (var potentialAttack in aiCharacterAttacks)
            {
                if (potentialAttack.minimumAttackDistance > aiCharacterManager.AICharacterCombatManager.DistanceFromTarget)
                    continue;
                if (potentialAttack.maximumAttackDistance < aiCharacterManager.AICharacterCombatManager.DistanceFromTarget)
                    continue;
                if (-potentialAttack.FOA > aiCharacterManager.AICharacterCombatManager.ViewableAngle)
                    continue;
                if (potentialAttack.FOA < aiCharacterManager.AICharacterCombatManager.ViewableAngle)
                    continue;
                potentialAttacks.Add(potentialAttack);
                
            }
            
            if (potentialAttacks.Count <= 0)
                return;
            
            var totalWeight = 0;
            foreach (var attack in potentialAttacks)
            {
                totalWeight += attack.attackWeight;
            }
            
            var randomWeight = Random.Range(1, totalWeight + 1);
            var processedWeight = 0;
            foreach (var attack in potentialAttacks)
            {
                processedWeight += attack.attackWeight;
                if (randomWeight <= processedWeight)
                {
                    chosenAttack = attack;
                    previousAttack = chosenAttack;
                    hasAttack = true;
                    return;
                }
            }
            
        }

        protected virtual bool RollForOutcomeChance(int outcomeChance)
        {
            bool outcomeWillBePerformed = false;
            int randomPercentage = Random.Range(0, 100);
            if (randomPercentage <= outcomeChance)
            {
                outcomeWillBePerformed = true;
            }

            return outcomeWillBePerformed;
        }

        protected override void ResetStateFlags(AICharacterManager aiCharacterManager)
        {
            base.ResetStateFlags(aiCharacterManager);

            hasRolledForComboOnce = false;
            hasAttack = false;
        }
    }
}