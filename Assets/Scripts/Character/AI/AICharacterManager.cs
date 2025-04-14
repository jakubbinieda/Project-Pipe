using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace ProjectPipe
{
    public class AICharacterManager : CharacterManager
    {
        public AICharacterCombatManager AICharacterCombatManager { get; private set; }

        [field: Header("Current State")]
        [field: SerializeField] private AIState CurrentState { get; set; }

        [field: Header("Navmesh Agent")]
        [field: SerializeField] public bool isNavMeshAgentEnabled; // TEMPORARY

        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }

        [field: Header("States")]
        [field: SerializeField] public IdleState IdleState { get; private set; }
        [field: SerializeField] public PursueTargetState PursueTargetState { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();

            AICharacterCombatManager = GetComponent<AICharacterCombatManager>();
            if (isNavMeshAgentEnabled)
            {
                NavMeshAgent = GetComponent<NavMeshAgent>();
            }
            IdleState = Instantiate(IdleState);
            PursueTargetState = Instantiate(PursueTargetState);
            CurrentState = IdleState;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            ProcessStateMachine();
        }
        
        private void ProcessStateMachine()
        {
            AIState nextState = CurrentState?.Tick(this);
            if (nextState != null)
            {
                CurrentState = nextState;
            }
        }

    }
}
