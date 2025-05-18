using UnityEngine;

namespace ProjectPipe
{
    public class AICharacterCombatManager : CharacterCombatManager
    {
        [field: Header("Target Info")]
        [field: SerializeField] public float ViewableAngle { get; set; }
        [field: SerializeField] public Vector3 TargetsDirection { get; set; }
        [field: SerializeField] public float DistanceFromTarget { get; set; }
        
        [field: Header("Action Recovery")]
        public float actionRecoveryTime = 0;
        
        [field: Header("Attack Rotation Speed")]
        [field: SerializeField] public float attackRotationSpeed = 25f;
        
        [field: Header("Sight")]
        [field: SerializeField] private float sightRange = 17f;
        public float fov  = 20f;

        [field: Header("Debug")]
        [field: SerializeField] private bool drawVisionCone = true;
        [field: SerializeField] private bool drawSightRays = true;

        protected override void Awake()
        {
            base.Awake();
            LockOnTransform = GetComponentInChildren<AILockOnTransform>().transform;
        }

        protected override void Update()
        {
            base.Update();
            if (drawVisionCone) DrawVisionCone(transform.position, transform.forward);
        }

        // This is the debug-only function
       private void DrawVisionCone(Vector3 origin, Vector3 forward, int rayCount = 20)
       {
           for (int i = 0; i < rayCount; i++)
           {
               float lerpFactor = (float)i / (rayCount - 1);
               float angle = Mathf.Lerp(-fov, fov, lerpFactor);
       
               Quaternion rayRotation = Quaternion.Euler(0, angle, 0);
               Vector3 rayDirection = rayRotation * forward * sightRange;
       
               Debug.DrawLine(origin, origin + rayDirection, Color.cyan);
           }
       }

        public void FindTargetViaLineOfSight(AICharacterManager aiCharacterManager)
        {
            if (CurrentTarget != null) return;

            var colliders = Physics.OverlapSphere(
                aiCharacterManager.transform.position,
                sightRange,
                WorldUtilityManager.Instance.CharacterLayers);

            for (var i = 0; i < colliders.Length; i++)
            {
                var targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();
                if (targetCharacter == null) continue;
                if (targetCharacter == aiCharacterManager) continue;
                if (targetCharacter.IsDead) continue;
                
                var targetDirection = 
                    targetCharacter.transform.position - aiCharacterManager.transform.position;
                var angleOfPotentialTarget = Vector3.Angle(targetDirection, aiCharacterManager.transform.forward);

                if (angleOfPotentialTarget < fov && angleOfPotentialTarget > -fov)
                {
                    if (Physics.Linecast(aiCharacterManager.transform.position,
                            targetCharacter.transform.position,
                            WorldUtilityManager.Instance.EnvironmentLayers))
                    {
                        if (drawSightRays)
                        {
                            Debug.DrawLine(aiCharacterManager.transform.position,
                                targetCharacter.transform.position,
                                Color.red, 0.5f);
                        }
                    }
                    else
                    {
                        if (drawSightRays)
                        {
                            Debug.DrawLine(aiCharacterManager.transform.position,
                                targetCharacter.transform.position,
                                Color.green, 20f);
                        }
                        
                        CurrentTarget = targetCharacter;
                        this.TargetsDirection =
                            CurrentTarget.transform.position - transform.position;
                        this.ViewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(transform, TargetsDirection);
                        PivotTowardsTarget(targetCharacter);
                    }
                }
                else
                {
                    if (drawSightRays)
                    {
                        Debug.DrawLine(aiCharacterManager.transform.position,
                            targetCharacter.transform.position,
                            Color.yellow, 0.5f);
                    }
                }
            }
        }
        
        public  void PivotTowardsTarget(CharacterManager targetCharacter)
        {
            if (_characterManager.IsPerformingAction) return;

            // TODO when i have turn animations
            
            // if (ViewableAngle >= 20 && ViewableAngle <= 60)
            // {
            //     _characterManager.CharacterAnimatorManager.PlayTargetAnimation("there will be", true, true);
            //     
            // } else if ()
        }
        
        public void HandleActionRecovery(AICharacterManager aiCharacterManager)
        {
            if (actionRecoveryTime > 0)
            {
                if (!aiCharacterManager.IsPerformingAction)
                {
                    actionRecoveryTime -= Time.deltaTime;
                }
              
            }
        }

        public void RotateTowardsAgent(AICharacterManager aiCharacterManager)
        {
            if (aiCharacterManager.isMoving)
            {
                aiCharacterManager.transform.rotation = aiCharacterManager.NavMeshAgent.transform.rotation;
            }
        }

        public void RotateTowardsTargetWhileAttacking(AICharacterManager aiCharacterManager)
        {
            if (CurrentTarget == null) return;

            if (!aiCharacterManager.CanRotate) return;
            
            // if (!aiCharacterManager.IsPerformingAction) return;
            
            Vector3 targetDirection = CurrentTarget.transform.position - aiCharacterManager.transform.position;
            targetDirection.y = 0;
            targetDirection.Normalize();

            if (targetDirection == Vector3.zero)
            {
                targetDirection = aiCharacterManager.transform.forward;
            }
            
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            aiCharacterManager.transform.rotation = Quaternion.Slerp(aiCharacterManager.transform.rotation, targetRotation, attackRotationSpeed * Time.deltaTime);
        }
    }
}