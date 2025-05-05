using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace ProjectPipe
{
    [CreateAssetMenu(menuName = "AI/States/IdleState")]
    public class IdleState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacterManager)
        {
            if (aiCharacterManager.AICharacterCombatManager.CurrentTarget != null)
            {
                return SwitchState(aiCharacterManager, aiCharacterManager.PursueTargetState);
            }
          
            aiCharacterManager.AICharacterCombatManager.FindTargetViaLineOfSight(aiCharacterManager);
            return this;
        }
        
    }
}