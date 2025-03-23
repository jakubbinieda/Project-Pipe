using UnityEngine;

namespace ProjectPipe
{
    public class CharacterStatsManager : MonoBehaviour
    {
        [field: Header("Health & Stamina")]
        [SerializeField] private Observable<int> currentHealth = new(0);
        [SerializeField] private Observable<int> maxHealth = new(0);
        [SerializeField] private Observable<float> currentStamina = new(0);
        [SerializeField] private Observable<float> maxStamina = new(0);
        [SerializeField] private float staminaRegenerationAmount = 2;
        [SerializeField] private float staminaRegenerationCooldown = 2;

        private CharacterManager _characterManager;
        private float _staminaRegenerationTickTimer;
        private float _staminaRegenerationTimer;

        protected virtual void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();
            currentStamina.OnValueChanged += ResetStaminaRegenerationTimer;
        }

        protected void Update()
        {
            RegenerateStamina();
        }

        private void RegenerateStamina()
        {
            if (_characterManager.IsPerformingAction) return;

            if (_characterManager.IsSprinting) return;

            _staminaRegenerationTimer =
                Mathf.Clamp(_staminaRegenerationTimer + Time.deltaTime, 0, staminaRegenerationCooldown);

            if (_staminaRegenerationTimer < staminaRegenerationCooldown)
                return;

            _staminaRegenerationTickTimer += Time.deltaTime;

            if (_staminaRegenerationTickTimer < 0.1f)
                return;

            _staminaRegenerationTickTimer = 0;
            currentStamina.Value = Mathf.Clamp(currentStamina.Value + staminaRegenerationAmount, 0, maxStamina);
        }

        private void ResetStaminaRegenerationTimer(float oldValue, float newValue)
        {
            if (oldValue <= newValue) return;

            _staminaRegenerationTimer = 0;
        }

        public bool CanAffordStaminaCost(float cost)
        {
            return currentStamina.Value >= cost;
        }

        public void SpendStamina(float cost)
        {
            currentStamina.Value -= cost;
        }

        public void SetMaxStamina(float value)
        {
            maxStamina.Value = value;
            currentStamina.Value = value;
        }

        public void SetMaxHealth(int value)
        {
            maxHealth.Value = value;
            currentHealth.Value = value;
        }
    }
}