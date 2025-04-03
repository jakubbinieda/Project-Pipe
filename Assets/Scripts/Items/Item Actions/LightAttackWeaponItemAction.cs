using UnityEngine;

namespace ProjectPipe
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
    public class LightAttackWeaponItemAction : WeaponItemAction
    {
        public override void AttemptToPerformAction(CharacterManager attacker, WeaponItem weapon)
        {
            base.AttemptToPerformAction(attacker, weapon);

            if (!attacker.IsGrounded) return;

            if (!attacker.CharacterStatsManager.CanAffordStaminaCost(weapon.BaseStaminaCost *
                                                                     weapon.LightAttackStaminaMultiplier)) return;


            if (attacker.CharacterCombatManager.CanPerformCombo && attacker.IsPerformingAction)
            {
                attacker.CharacterStatsManager.SpendStamina(
                    weapon.BaseStaminaCost * weapon.LightAttackStaminaMultiplier);

                attacker.CharacterCombatManager.DisableCombo();

                switch (attacker.CharacterCombatManager.LastAttackAnimation)
                {
                    case "Main_Light_Attack_01":
                        attacker.CharacterAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack02,
                            "Main_Light_Attack_02", true);
                        break;
                    case "Main_Light_Attack_02":
                        attacker.CharacterAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01,
                            "Main_Light_Attack_01", true);
                        break;
                }
            }
            else if (!attacker.IsPerformingAction)
            {
                attacker.CharacterStatsManager.SpendStamina(
                    weapon.BaseStaminaCost * weapon.LightAttackStaminaMultiplier);

                attacker.CharacterAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01,
                    "Main_Light_Attack_01", true);
            }
        }
    }
}