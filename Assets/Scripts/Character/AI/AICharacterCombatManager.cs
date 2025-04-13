using UnityEngine;

namespace ProjectPipe
{
    public class AICharacterCombatManager : CharacterCombatManager
    {
        [field: Header("Debug")]
        [field: SerializeField] private bool drawVisionCone = true;
        [field: SerializeField] private bool drawSightRays = true;

        [field: Header("Current Target")]
        [field: SerializeField] public CharacterManager CurrentTarget { get; private set; }

        [field: Header("Sight")]
        [field: SerializeField] private float sightRange = 17f;
        [field: SerializeField] private float minimumSightAngle = -20f;
        [field: SerializeField] private float maximumSightAngle = 20f;
        
        protected override void Update()
        {
            base.Update();
            if (drawVisionCone) DrawVisionCone(transform.position, transform.forward, 2 * maximumSightAngle, sightRange);
        }

        // -------------------------------------------------------------------------
        private void DrawVisionCone(Vector3 origin, Vector3 forward, float visionAngle, float sightRange,
            int horizontalRayCount = 5, int verticalRayCount = 5)
        {
            Vector3 offset = new Vector3(0, 0, 0);
            offset.y += _characterManager.CharacterController.height * transform.localScale.y;
            
            for (var h = 1; h < horizontalRayCount; h++)
            {
                var horizontalLerpFactor = (float) h / horizontalRayCount;
                var horizontalAngle = Mathf.Lerp(-visionAngle / 2, visionAngle / 2, horizontalLerpFactor);

                for (var v = 1; v < verticalRayCount; v++)
                {
                    var verticalLerpFactor = (float)v / verticalRayCount;
                    var verticalAngle = Mathf.Lerp(-visionAngle / 2, visionAngle / 2, verticalLerpFactor);

                    var rayRotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
                    var rayDirection = rayRotation * forward * sightRange;

                    Debug.DrawLine(origin + offset, origin + rayDirection, Color.cyan);
                }
            }
        }
        // -------------------------------------------------------------------------

        public void FindTargetViaLineOfSight(AICharacterManager aiCharacterManager)
        {
            if (CurrentTarget != null) return;
            
            Vector3 offset = new Vector3(0, 0, 0);
            offset.y += _characterManager.CharacterController.height * transform.localScale.y;

            var colliders = Physics.OverlapSphere(
                aiCharacterManager.transform.position + offset,
                sightRange,
                WorldUtilityManager.Instance.CharacterLayers);

            for (var i = 0; i < colliders.Length; i++)
            {
                var targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();
                if (targetCharacter == null) continue;
                if (targetCharacter == aiCharacterManager) continue;
                if (targetCharacter.IsDead) continue;

                var targetDirection = targetCharacter.transform.position - aiCharacterManager.transform.position + offset;
                var viewableAngle = Vector3.Angle(targetDirection, aiCharacterManager.transform.forward);
                
                if (viewableAngle < maximumSightAngle && viewableAngle > minimumSightAngle)
                {
                    if (Physics.Linecast(aiCharacterManager.transform.position + offset,
                            targetCharacter.transform.position,
                            WorldUtilityManager.Instance.EnvironmentLayers))
                    {
                        if (drawSightRays)
                        {
                            Debug.DrawLine(aiCharacterManager.transform.position + offset,
                                targetCharacter.transform.position,
                                Color.red, 0.5f);
                            Debug.Log("BLOCKED");
                        }
                    }
                    else
                    {
                        if (drawSightRays)
                        {
                            Debug.DrawLine(aiCharacterManager.transform.position + offset,
                                targetCharacter.transform.position,
                                Color.green, 2f);
                            Debug.Log("FOUND TARGET");
                        }
                        aiCharacterManager.AICharacterCombatManager.CurrentTarget = targetCharacter;
                    }
                }
                else
                {
                    if (drawSightRays)
                    {
                        Debug.Log("OUT OF SIGHT");
                        Debug.DrawLine(aiCharacterManager.transform.position + offset, targetCharacter.transform.position,
                            Color.yellow, 0.5f);
                    }
                }
            }
        }
    }
}