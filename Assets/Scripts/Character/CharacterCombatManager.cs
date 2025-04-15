using UnityEngine;

namespace ProjectPipe
{
    public class CharacterCombatManager : MonoBehaviour
    {
        [field: SerializeField] public Transform LockOnTransform { get; set; }
        [field: SerializeField] public CharacterManager CurrentTarget { get; set; }

        private readonly int _isChargingAttackHash = Animator.StringToHash("IsChargingAttack");
        private CharacterManager _characterManager;

        public bool CanPerformCombo { get; private set; }
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

        private void OnIsChargingHeavyAttackValueChanged(bool oldValue, bool newValue)
        {
            _characterManager.Animator.SetBool(_isChargingAttackHash, newValue);
        }

        public void EnableCombo()
        {
            CanPerformCombo = true;
        }

        public void DisableCombo()
        {
            CanPerformCombo = false;
        }

        public virtual void SetTarget(CharacterManager target)
        {
            CurrentTarget = target ? target : null;
        }
    }
}