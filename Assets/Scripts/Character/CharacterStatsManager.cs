using UnityEngine;

namespace ProjectPipe
{
    public class CharacterStatsManager : MonoBehaviour
    {
        [field: Header("Health & Stamina")]
        [SerializeField] protected Observable<int> currentHealth = new(0);
        [SerializeField] protected Observable<int> maxHealth = new(0);
        [SerializeField] protected Observable<float> currentStamina = new(0);
        [SerializeField] protected Observable<float> maxStamina = new(0);
        [SerializeField] private float staminaRegenerationAmount = 2;
        [SerializeField] private float staminaRegenerationCooldown = 2;

        private CharacterManager _characterManager;
        private CharacterUIManager _uiManager;
        
        private float _staminaRegenerationTickTimer;
        private float _staminaRegenerationTimer;

        protected virtual void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();
            _uiManager = GetComponentInChildren<CharacterUIManager>();
        }

        protected virtual void Start()
        {
            currentStamina.OnValueChanged += ResetStaminaRegenerationTimer;
            currentHealth.OnValueChanged += CheckHP;
            
            maxHealth.OnValueChanged += _uiManager.SetMaxHealthValue;
            currentHealth.OnValueChanged += _uiManager.OnHPChange;
    
            // This will probably not be needed in the future
            // Now we don't use SetMaxHealth anywhere
            _uiManager.SetMaxHealthValue(maxHealth.Value, maxHealth.Value);
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
            
            if (currentStamina.Value + staminaRegenerationAmount < maxStamina.Value)
                currentStamina.Value += staminaRegenerationAmount;
            else if (!Mathf.Approximately(currentStamina.Value, maxStamina.Value))
                currentStamina.Value = maxStamina.Value;
        }

        private void ResetStaminaRegenerationTimer(float oldValue, float newValue)
        {
            if (oldValue <= newValue) return;

            _staminaRegenerationTimer = 0;
        }

        protected void CheckHP(int oldValue, int newValue)
        {
            if (newValue > 0) return;
            
            StartCoroutine(_characterManager.ProcessDeathEvent());
        }

        public bool CanAffordStaminaCost(float cost)
        {
            return currentStamina.Value >= cost;
        }

        public void SpendStamina(float cost)
        {
            currentStamina.Value -= cost;
        }

        public void ReduceHealth(int damage)
        {
            currentHealth.Value -= damage;
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
