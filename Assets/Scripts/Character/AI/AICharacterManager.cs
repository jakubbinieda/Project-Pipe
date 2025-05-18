using UnityEngine;
using UnityEngine.AI;

namespace ProjectPipe
{
    public class AICharacterManager : CharacterManager
    {
        [field: Header("AI Specific Flags")]
        [field: SerializeField] public bool isMoving;
        
        [field: Header("Nav Mesh Agent")]
        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
        
        [field: Header("Current State")]
        [field: SerializeField] public AIState CurrentState { get; private set; }

        [field: Header("States")]
        [field: SerializeField] public IdleState IdleState { get; private set; }
        [field: SerializeField] public PursueTargetState PursueTargetState { get; private set; }
        [field: SerializeField] public CombatStanceState CombatStanceState { get; private set; }
        [field: SerializeField] public AttackState AttackState { get; private set; }
        
        public AICharacterCombatManager AICharacterCombatManager { get; private set; }
        public AICharacterLocomotionManager AICharacterLocomotionManager { get; private set; }

        // Maybe should be moved to AICharacterAnimatorManager
        private int _isMovingHash;

        protected override void Awake()
        {
            base.Awake();

            AICharacterCombatManager = GetComponent<AICharacterCombatManager>();
            AICharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();

            NavMeshAgent = GetComponentInChildren<NavMeshAgent>();
            NavMeshAgent.enabled = false; // STUPID UNITY

            IdleState = Instantiate(IdleState);
            PursueTargetState = Instantiate(PursueTargetState);
            CombatStanceState = Instantiate(CombatStanceState);
            AttackState = Instantiate(AttackState);
            CurrentState = IdleState;
            
            _isMovingHash = Animator.StringToHash("isMoving");
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            ProcessStateMachine();
        }

        protected override void Update()
        {
            base.Update();
            AICharacterCombatManager.HandleActionRecovery(this);
        }

        private void ProcessStateMachine()
        {
            var nextState = CurrentState?.Tick(this);
            if (nextState != null) CurrentState = nextState;

            // This fixes rotation issues
            NavMeshAgent.transform.localPosition = Vector3.zero;
            NavMeshAgent.transform.localRotation = Quaternion.identity;

            if (AICharacterCombatManager.CurrentTarget != null)
            {
                AICharacterCombatManager.TargetsDirection =
                    AICharacterCombatManager.CurrentTarget.transform.position - transform.position;
                AICharacterCombatManager.ViewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(
                    transform,
                    AICharacterCombatManager.TargetsDirection);
                AICharacterCombatManager.DistanceFromTarget =
                    Vector3.Distance(transform.position, AICharacterCombatManager.CurrentTarget.transform.position);
            }

            
            if (NavMeshAgent.enabled)
            {
                var agentDestination = NavMeshAgent.destination;
                var agentRemainingDistance = Vector3.Distance(agentDestination, transform.position);

                Debug.Log("DISTANCE TO TARGET: " + agentRemainingDistance);

                if (agentRemainingDistance > NavMeshAgent.stoppingDistance)
                {
                    Debug.Log("I AM COMING FOR YOU");
                    isMoving = true;
                    ApplyRootMotion = true; // This should keep the agent stick to root
                    Animator.SetBool(_isMovingHash, true);
                }
                else
                {
                    Debug.Log("I GOT YOU");
                    isMoving = false;
                    Animator.SetBool(_isMovingHash, false);
                }
            }
            else
            {
                isMoving = false;
                Animator.SetBool(_isMovingHash, false);
            }
        }
    }
}