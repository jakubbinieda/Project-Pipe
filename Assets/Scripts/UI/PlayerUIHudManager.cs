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

        public void SetMaxHealthValue(int oldValue, int newValue)
        {
            healthBar.SetMaxStat(newValue);
        }

        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxStaminaValue(float oldValue, float newValue)
        {
            staminaBar.SetMaxStat(Mathf.RoundToInt(newValue));
        }
    } 
}
