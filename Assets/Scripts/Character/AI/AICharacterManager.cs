using UnityEngine;
using UnityEngine.AI;

namespace ProjectPipe
{
    public class AICharacterManager : CharacterManager
    {
        [field: Header("AI Specific Flags")]
        [field: SerializeField] public bool IsMoving { get; private set; }

        [field: Header("Current State")]
        [field: SerializeField] private AIState CurrentState { get; set; }

        [field: Header("Nav Mesh Agent")]
        // [field: SerializeField] public bool hasNavMeshAgent; // TEMPORARY
        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }

        [field: Header("States")]
        [field: SerializeField] public IdleState IdleState { get; private set; }

        [field: SerializeField] public PursueTargetState PursueTargetState { get; private set; }

        // Maybe should be moved to AICharacterAnimatorManager
        private int _isMovingHash;
        public AICharacterCombatManager AICharacterCombatManager { get; private set; }
        public AICharacterLocomotionManager AICharacterLocomotionManager { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            AICharacterCombatManager = GetComponent<AICharacterCombatManager>();
            AICharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();

            NavMeshAgent = GetComponentInChildren<NavMeshAgent>();
            NavMeshAgent.enabled = false; // STUPID UNITY

            IdleState = Instantiate(IdleState);
            PursueTargetState = Instantiate(PursueTargetState);
            CurrentState = IdleState;
            _isMovingHash = Animator.StringToHash("isMoving");
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            ProcessStateMachine();
        }

        private void ProcessStateMachine()
        {
            var nextState = CurrentState?.Tick(this);
            if (nextState != null) CurrentState = nextState;

            // This fixes rotation issues
            NavMeshAgent.transform.localPosition = Vector3.zero;
            NavMeshAgent.transform.localRotation = Quaternion.identity;

            if (NavMeshAgent.enabled)
            {
                var agentDestination = NavMeshAgent.destination;
                var agentRemainingDistance = Vector3.Distance(agentDestination, transform.position);

                Debug.Log("DISTANCE TO TARGET: " + agentRemainingDistance);

                if (agentRemainingDistance > NavMeshAgent.stoppingDistance)
                {
                    Debug.Log("I AM COMING FOR YOU");
                    IsMoving = true;
                    Animator.SetBool(_isMovingHash, true);
                    ApplyRootMotion = true; // This should keep the agent stick to root
                }
                else
                {
                    Debug.Log("I GOT YOU");
                    IsMoving = false;
                    Animator.SetBool(_isMovingHash, false);
                }
            }
            else
            {
                IsMoving = false;
                Animator.SetBool(_isMovingHash, false);
            }
        }
    }
}