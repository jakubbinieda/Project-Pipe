using UnityEngine;
using UnityEngine.AI;

namespace ProjectPipe
{
    public class AICharacterManager : CharacterManager
    {
        [HideInInspector] public AICharacterCombatManager AICharacterCombatManager { get; private set; }
        
        [field: Header("Current State")]
        [field: SerializeField] private AIState currentState;
        
        [field: Header("Navmesh Agent")]
        [field: SerializeField] public bool isNavMeshAgentEnabled = false;
        [field: SerializeField] public NavMeshAgent navMeshAgent;
        
        [field: Header("States")]
        public IdleState idleState;
        public PursueTargetState pursueTargetState;
        
        protected override void Awake()
        {
            base.Awake();

            AICharacterCombatManager = GetComponent<AICharacterCombatManager>();
            if (isNavMeshAgentEnabled)
            {
                navMeshAgent = GetComponent<NavMeshAgent>();
            }
            navMeshAgent = GetComponent<NavMeshAgent>();
            idleState = Instantiate(idleState);
            pursueTargetState = Instantiate(pursueTargetState);
            currentState = idleState;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            ProcessStateMachine();
        }
        
        private void ProcessStateMachine()
        {
            AIState nextState = currentState?.Tick(this);
            if (nextState != null)
            {
                currentState = nextState;
            }
        }

    }
}
