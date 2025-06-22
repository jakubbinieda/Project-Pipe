using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectPipe
{
    [CreateAssetMenu(menuName = "AI/States/IdleState")]
    public class IdleState : AIState
    {
        [field: Header("Idle State Mode")]
        [field: SerializeField] public IdleStateMode IdleStateMode { get; set; } = IdleStateMode.Idle;
        
        [field: Header("Patrol Options")]
        [field: SerializeField] public AIPatrolPath patrolPath;
        [field: SerializeField] public int patrolPathIndex = 0;
        [field: SerializeField] private bool hasFoundClosestPointNearCharacterSpawn = false;
        [field: SerializeField] private bool patrolComplete = false;
        [field: SerializeField] private bool repeatPatrol = false;
        [field: SerializeField] private int patrolDestinationIndex = 0;
        [field: SerializeField] private bool hasPatrolDestination = false;
        [field: SerializeField] private Vector3 currentPatrolDestination;
        [field: SerializeField] private float distanceFromCurrentPatrolDestination;
        [field: SerializeField] private float timeBetweenPatrols = 15f;
        [field: SerializeField] private float restTimer = 0f;
        
        public override AIState Tick(AICharacterManager aiCharacterManager)
        {
            aiCharacterManager.AICharacterCombatManager.FindTargetViaLineOfSight(aiCharacterManager);
            switch (IdleStateMode)
            {
                case IdleStateMode.Idle:
                    return Idle(aiCharacterManager);
                case IdleStateMode.Patrol:
                    return Patrol(aiCharacterManager);
                default:
                    return this;
            }
        }

        protected virtual AIState Idle(AICharacterManager aiCharacterManager)
        {
            if (aiCharacterManager.AICharacterCombatManager.CurrentTarget != null)
            {
                return SwitchState(aiCharacterManager, aiCharacterManager.PursueTargetState);
            }
          
            return this;
        }
        
        protected virtual AIState Patrol(AICharacterManager aiCharacterManager)
        {
            if (patrolPath == null)
            {
                patrolPath = WorldUtilityManager.Instance.GetPatrolPathByIndex(patrolPathIndex);
            }
            
            if (!aiCharacterManager.IsGrounded)
            {
                return this;
            }

            if (aiCharacterManager.IsPerformingAction)
            {
                aiCharacterManager.NavMeshAgent.enabled = true;
                aiCharacterManager.isMoving = true;
                return this;
            }

            if (!aiCharacterManager.NavMeshAgent.enabled)
            {
                aiCharacterManager.NavMeshAgent.enabled = true;
            }
            
            if (aiCharacterManager.AICharacterCombatManager.CurrentTarget != null)
            {
                return SwitchState(aiCharacterManager, aiCharacterManager.PursueTargetState);
            }

            if (patrolComplete && repeatPatrol)
            {
                if (timeBetweenPatrols > restTimer)
                {
                    aiCharacterManager.NavMeshAgent.enabled = false;
                    aiCharacterManager.isMoving = false;
                    restTimer += Time.deltaTime;
                }
                else
                {
                    patrolDestinationIndex = -1;
                    hasPatrolDestination = false;
                    patrolComplete = false;
                    restTimer = 0f;
                }
            } 
            else if (patrolComplete && !repeatPatrol)
            {
                aiCharacterManager.NavMeshAgent.enabled = false;
                aiCharacterManager.isMoving = false;
                return this;
            }

            if (hasPatrolDestination)
            {
                distanceFromCurrentPatrolDestination = Vector3.Distance(
                    aiCharacterManager.transform.position, 
                    currentPatrolDestination);

                if (distanceFromCurrentPatrolDestination > 2)
                {
                    aiCharacterManager.NavMeshAgent.enabled = true;
                    aiCharacterManager.AICharacterLocomotionManager.RotateTowardsAgent(aiCharacterManager);
                }
                else
                {
                    currentPatrolDestination = aiCharacterManager.transform.position;
                    hasPatrolDestination = false;
                }
            }
            else
            {
                patrolDestinationIndex += 1;
                if (patrolDestinationIndex > patrolPath.path.Count - 1)
                {
                    patrolComplete = true;
                    return this;
                }

                if (!hasFoundClosestPointNearCharacterSpawn)
                {
                    hasFoundClosestPointNearCharacterSpawn = true;
                    float closestDistance = Mathf.Infinity;
                    
                    for (int i = 0; i < patrolPath.path.Count; i++)
                    {
                        float distance = Vector3.Distance(aiCharacterManager.transform.position, patrolPath.path[i]);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            patrolDestinationIndex = i;
                            currentPatrolDestination = patrolPath.path[patrolDestinationIndex];
                        }
                    }
                }
                else
                {
                    currentPatrolDestination = patrolPath.path[patrolDestinationIndex];
                }
                hasPatrolDestination = true;
            }
            NavMeshPath path = new NavMeshPath();
            aiCharacterManager.NavMeshAgent.CalculatePath(currentPatrolDestination, path);
            aiCharacterManager.NavMeshAgent.SetPath(path);

            return this;
        }
    }
}