using UnityEngine;

namespace ProjectPipe
{
    public class CharacterCombatManager : MonoBehaviour
    {
        [field: SerializeField] public Transform LockOnTransform { get; set; }
        [field: SerializeField] public CharacterManager CurrentTarget { get; set; }

        [Header("Attack Flags")]
        [field: SerializeField] public bool CanDoRollAttack { get; private set; }
        [field: SerializeField] public bool CanDoBackstepAttack { get; private set; }
        [field: SerializeField] public bool CanDoCombo { get; private set; }

        private readonly int _isChargingAttackHash = Animator.StringToHash("IsChargingAttack");
        protected CharacterManager _characterManager;

        public string LastAttackAnimation { get; set; }

        public AttackType CurrentAttackType { get; set; }
        protected Observable<bool> IsChargingHeavyAttack { get; set; } = new(false);

        protected virtual void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();

            IsChargingHeavyAttack.OnValueChanged += OnIsChargingHeavyAttackValueChanged;
        }

        protected virtual void Update()
        {
        }

        public virtual void SetTarget(CharacterManager target)
        {
            CurrentTarget = target ? target : null;
        }

        private void OnIsChargingHeavyAttackValueChanged(bool oldValue, bool newValue)
        {
            _characterManager.Animator.SetBool(_isChargingAttackHash, newValue);
        }

        public void EnableCanDoCombo()
        {
            CanDoCombo = true;
        }

        public void DisableCanDoCombo()
        {
            CanDoCombo = false;
        }


        public void EnableCanDoRollAttack()
        {
            CanDoRollAttack = true;
        }

        public void DisableCanDoRollAttack()
        {
            CanDoRollAttack = false;
        }

        public void EnableCanDoBackstepAttack()
        {
            CanDoBackstepAttack = true;
        }

        public void DisableCanDoBackstepAttack()
        {
            CanDoBackstepAttack = false;
        }
    }
}