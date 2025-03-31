using UnityEngine;

namespace ProjectPipe
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [SerializeField] private UIStatBar healthBar;
        [SerializeField] private UIStatBar staminaBar;

        public void SetNewHealthValue(int oldValue, int newValue)
        {
            healthBar.SetStat(newValue);
        }

        public void SetMaxHealthValue(int oldValue, int maxHealth)
        {
            healthBar.SetMaxStat(maxHealth);
        }

        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxStaminaValue(float oldValue, float maxStamina)
        {
            staminaBar.SetMaxStat(Mathf.RoundToInt(maxStamina));
        }
    } 
}
