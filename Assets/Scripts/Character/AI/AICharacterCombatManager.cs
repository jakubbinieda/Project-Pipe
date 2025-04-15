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
       private void DrawVisionCone(Vector3 origin, Vector3 forward, int rayCount = 20)
       {
           for (int i = 0; i < rayCount; i++)
           {
               float lerpFactor = (float)i / (rayCount - 1);
               float angle = Mathf.Lerp(-sightAngle, sightAngle, lerpFactor);
       
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
                var viewableAngle = Vector3.Angle(targetDirection, aiCharacterManager.transform.forward);

                if (viewableAngle < sightAngle && viewableAngle > -sightAngle)
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
                            Debug.Log("BLOCKED");
                        }
                    }
                    else
                    {
                        if (drawSightRays)
                        {
                            Debug.DrawLine(aiCharacterManager.transform.position,
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
                        Debug.DrawLine(aiCharacterManager.transform.position,
                            targetCharacter.transform.position,
                            Color.yellow, 0.5f);
                        Debug.Log("OUT OF SIGHT");
                    }
                }
            }
        }
    }
}