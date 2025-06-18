namespace ProjectPipe
{
    public class AIUndeadEquipmentManager : CharacterEquipmentManager
    {
        private AIUndeadCombatManager _aiUndeadCombatManager;
        
        protected override void Awake()
        {
            base.Awake();
            _aiUndeadCombatManager = GetComponent<AIUndeadCombatManager>();
        }
        
        public override void OpenDamageCollider()
        {
            base.OpenDamageCollider();
            _aiUndeadCombatManager.SetAttack01Damage();
            _aiUndeadCombatManager.SetAttack02Damage();
            _aiUndeadCombatManager.OpenRightHandDamageCollider();
            _aiUndeadCombatManager.OpenLeftHandDamageCollider();
        }

        public override void CloseDamageCollider()
        {
            base.CloseDamageCollider();
            _aiUndeadCombatManager.CloseRightHandDamageCollider();
            _aiUndeadCombatManager.CloseLeftHandDamageCollider();
        }
    }
}