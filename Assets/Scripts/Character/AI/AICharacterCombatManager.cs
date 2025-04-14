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
        [field: SerializeField] private float sightAngle = 20f;

        protected override void Update()
        {
            base.Update();
            if (drawVisionCone) DrawVisionCone(transform.position, transform.forward);
        }

        // This is debug only function
       private void DrawVisionCone(Vector3 origin, Vector3 forward, int horizontalRayCount = 5, int verticalRayCount = 5)
       {
           var offset = new Vector3(0, _characterManager.CharacterController.height * transform.localScale.y, 0);
       
           for (var h = 0; h < horizontalRayCount; h++)
           {
               var horizontalLerpFactor = (float)h / (horizontalRayCount - 1);
               var horizontalAngle = Mathf.Lerp(-sightAngle, sightAngle, horizontalLerpFactor);
       
               for (var v = 0; v < verticalRayCount; v++)
               {
                   var verticalLerpFactor = (float)v / (verticalRayCount - 1);
                   var verticalAngle = Mathf.Lerp(-sightAngle, sightAngle, verticalLerpFactor);
       
                   var rayRotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
                   var rayDirection = rayRotation * forward * sightRange;
       
                   Debug.DrawLine(origin + offset, origin + offset + rayDirection, Color.cyan);
               }
           }
       }

        public void FindTargetViaLineOfSight(AICharacterManager aiCharacterManager)
        {
            if (CurrentTarget != null) return;

            var offset = new Vector3(0, 0, 0);
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

                var targetDirection = 
                    targetCharacter.transform.position - aiCharacterManager.transform.position + offset;
                var viewableAngle = Vector3.Angle(targetDirection, aiCharacterManager.transform.forward);

                if (viewableAngle < sightAngle && viewableAngle > -sightAngle)
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
                                Color.green, 20f);
                            Debug.Log("FOUND TARGET");
                        }

                        aiCharacterManager.AICharacterCombatManager.CurrentTarget = targetCharacter;
                    }
                }
                else
                {
                    if (drawSightRays)
                    {
                        Debug.DrawLine(aiCharacterManager.transform.position + offset,
                            targetCharacter.transform.position,
                            Color.yellow, 0.5f);
                        Debug.Log("OUT OF SIGHT");
                    }
                }
            }
        }
    }
}