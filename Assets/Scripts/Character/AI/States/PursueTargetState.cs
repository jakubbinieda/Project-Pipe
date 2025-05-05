using UnityEngine;
using UnityEngine.AI;

namespace ProjectPipe
{
    [CreateAssetMenu(menuName = "AI/States/PursueTargetState")]
    public class PursueTargetState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacterManager)
        {
            if (aiCharacterManager.IsPerformingAction)
                return this;
            
            if (aiCharacterManager.AICharacterCombatManager.CurrentTarget == null)
                return SwitchState(aiCharacterManager, aiCharacterManager.IdleState);
            
            if (!aiCharacterManager.NavMeshAgent.enabled)
                aiCharacterManager.NavMeshAgent.enabled = true;

            // This is optional
            if (aiCharacterManager.AICharacterCombatManager.ViewableAngle <
                aiCharacterManager.AICharacterCombatManager.fov
                || aiCharacterManager.AICharacterCombatManager.ViewableAngle >
                aiCharacterManager.AICharacterCombatManager.fov)
            {
                aiCharacterManager.AICharacterCombatManager.PivotTowardsTarget(aiCharacterManager);
            }
            
            aiCharacterManager.AICharacterLocomotionManager.RotateTowardsAgent(aiCharacterManager);

            if (aiCharacterManager.AICharacterCombatManager.DistanceFromTarget <=
                aiCharacterManager.NavMeshAgent.stoppingDistance)
            {
                return SwitchState(aiCharacterManager, aiCharacterManager.CombatStanceState);
            }
            
            
            var path = new NavMeshPath();
            aiCharacterManager.NavMeshAgent.CalculatePath(aiCharacterManager.AICharacterCombatManager.CurrentTarget.transform.position, path);
            aiCharacterManager.NavMeshAgent.SetPath(path);
            return this;
        }
    }
}