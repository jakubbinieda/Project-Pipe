using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
            
            NavMeshPath path = new NavMeshPath();
            aiCharacterManager.NavMeshAgent.CalculatePath(aiCharacterManager.AICharacterCombatManager.CurrentTarget.transform.position, path);
            aiCharacterManager.NavMeshAgent.SetPath(path);
            return this;
        }
    }
}