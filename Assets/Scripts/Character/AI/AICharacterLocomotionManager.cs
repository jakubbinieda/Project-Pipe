namespace ProjectPipe
{
    public class AICharacterLocomotionManager : CharacterLocomotionManager
    {
        public void RotateTowardsAgent(AICharacterManager aiCharacterManager)
        {
            if (aiCharacterManager.isMoving)
            {
                aiCharacterManager.transform.rotation = aiCharacterManager.NavMeshAgent.transform.rotation;
            }
        }
    }
}