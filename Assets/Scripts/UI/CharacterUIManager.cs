using UnityEngine;

namespace ProjectPipe
{
    public class CharacterUIManager : MonoBehaviour
    {
        [Header("UI")]
        public UICharacterHPBar characterHPBar;

        private void Awake()
        {
            characterHPBar = GetComponentInChildren<UICharacterHPBar>();
        }

        public void SetMaxHealthValue(int oldValue, int newValue)
        {
            characterHPBar.SetMaxHealthValue(newValue);
        }

        public void OnHPChange(int oldValue, int newValue)
        {
            characterHPBar.SetStat(newValue);
        }
    }
}