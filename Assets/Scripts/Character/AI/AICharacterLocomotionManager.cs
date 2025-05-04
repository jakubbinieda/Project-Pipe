namespace ProjectPipe
{
    public class AICharacterLocomotionManager : CharacterLocomotionManager
    {
        public void RotateTowardsAgent(AICharacterManager aiCharacterManager)
        {
            if (aiCharacterManager.IsMoving)
            {
                aiCharacterManager.transform.rotation = aiCharacterManager.NavMeshAgent.transform.rotation;
            }
        }
    }
}