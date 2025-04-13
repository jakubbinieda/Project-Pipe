using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace ProjectPipe
{
    public class AIState : ScriptableObject
    {
        public virtual AIState Tick(AICharacterManager aiCharacterManager)
        {
            return this;
        }

        protected virtual AIState SwitchState(AICharacterManager aiCharacterManager, AIState newState)
        {
            ResetStateFlags(aiCharacterManager);
            return newState;
        }

        protected virtual void ResetStateFlags(AICharacterManager aiCharacterManager)
        {
        }
    }
}