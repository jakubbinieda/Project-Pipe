using Unity.VisualScripting;

namespace ProjectPipe
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        protected override void Start()
        {
            var hud = PlayerUIManager.Instance.PlayerUIHudManager;

            maxStamina.OnValueChanged += hud.SetMaxStaminaValue;
            currentStamina.OnValueChanged += hud.SetNewStaminaValue;

            maxHealth.OnValueChanged += hud.SetMaxHealthValue;
            currentHealth.OnValueChanged += hud.SetNewHealthValue;
        }
    }
}