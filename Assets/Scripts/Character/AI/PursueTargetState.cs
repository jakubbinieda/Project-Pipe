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
                return SwitchState(aiCharacterManager, aiCharacterManager.idleState);
            
            if (!aiCharacterManager.navMeshAgent.enabled)
                aiCharacterManager.navMeshAgent.enabled = true;
            
            NavMeshPath path = new NavMeshPath();
            aiCharacterManager.navMeshAgent.CalculatePath(aiCharacterManager.AICharacterCombatManager.CurrentTarget.transform.position, path);
            aiCharacterManager.navMeshAgent.SetPath(path);
            return this;
        }
    }
}