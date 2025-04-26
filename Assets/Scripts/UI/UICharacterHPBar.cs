using UnityEngine;
using TMPro;

namespace ProjectPipe
{
    public class UICharacterHPBar : UIStatBar
    {
        private CharacterManager _character;

        [Header("HP Bar Settings")]
        [SerializeField] private float _defaultTimeBeforeBarHides = 3f;
        private float _hideTimer = 0f;

        [Header("Damage Text")]
        [SerializeField] private TextMeshProUGUI _characterDamage;
        private int _lastHP = 0;
        private bool _isInitialized = false;

        protected override void Awake()
        {
            base.Awake();
            _character = GetComponentInParent<CharacterManager>();
        }

        protected override void Start()
        {
            base.Start();
            _hideTimer = 0;
            gameObject.SetActive(false);
        }

        public void SetMaxHealthValue(int maxValue)
        {
            _slider.maxValue = maxValue;
            _slider.value = maxValue;
            _lastHP = maxValue;
            _isInitialized = true;
        }

        public override void SetStat(int newHP)
        {
            if (!_isInitialized)
                SetMaxHealthValue(newHP);

            int damageTaken = _lastHP - newHP;

            if (damageTaken != 0)
            {
                ShowBarTemporarily();
                
                if(damageTaken > 0) 
                    _characterDamage.text = "- " + damageTaken;
                else 
                    _characterDamage.text = "+ " + Mathf.Abs(damageTaken);
            }
            else
                _characterDamage.text = "";

            _lastHP = newHP;
            _slider.value = newHP;
        }

        private void ShowBarTemporarily()
        {
            gameObject.SetActive(true);
            _hideTimer = _defaultTimeBeforeBarHides;
        }

        private void Update()
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);

            if (gameObject.activeSelf)
            {
                _hideTimer -= Time.deltaTime;
                if (_hideTimer <= 0f)
                    gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            _characterDamage.text = "";
        }
    }
}