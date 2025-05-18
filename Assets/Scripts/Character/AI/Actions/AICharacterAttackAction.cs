using UnityEngine;

namespace ProjectPipe
{
    [CreateAssetMenu(menuName = "AI/Actions/AICharacterAttackAction")]
    public class AICharacterAttackAction : ScriptableObject
    {
        [field: Header("Attack")]
        [field: SerializeField] private string attackAnimation;
        
        [field: Header("Combo Action")]
        public AICharacterAttackAction comboAction;

        [field: Header("Action Values")] 
        [field: SerializeField] private AttackType attackType;
        public int attackWeight = 50;
        public float actionRecoveryTime = 1.5f;
        public float foa = 35;
        public float minimumAttackDistance = 0;
        public float maximumAttackDistance = 2;

        public void AttemptToPerformAction(AICharacterManager aiCharacterManager)
        {
            aiCharacterManager.CharacterAnimatorManager.PlayTargetAttackActionAnimation(attackType, attackAnimation, true);
        }
    }
}