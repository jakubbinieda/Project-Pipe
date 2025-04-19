using UnityEngine;

namespace ProjectPipe
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]
    public class HeavyAttackWeaponItemAction : WeaponItemAction
    {
        public override void AttemptToPerformAction(CharacterManager attacker, WeaponItem weapon)
        {
            base.AttemptToPerformAction(attacker, weapon);

            if (!attacker.IsGrounded) return;

            if (!attacker.CharacterStatsManager.CanAffordStaminaCost(weapon.BaseStaminaCost *
                                                                     weapon.HeavyAttackStaminaMultiplier))
                return;

            if (attacker.CharacterCombatManager.CanDoCombo && attacker.IsPerformingAction)
            {
                attacker.CharacterStatsManager.SpendStamina(
                    weapon.BaseStaminaCost * weapon.HeavyAttackStaminaMultiplier);

                attacker.CharacterCombatManager.DisableCanDoCombo();

                switch (attacker.CharacterCombatManager.LastAttackAnimation)
                {
                    case "Main_Heavy_Attack_01":
                        attacker.CharacterAnimatorManager.PlayTargetAttackActionAnimation(AttackType.HeavyAttack02,
                            "Main_Heavy_Attack_02", true);
                        break;
                    case "Main_Heavy_Attack_02":
                        attacker.CharacterAnimatorManager.PlayTargetAttackActionAnimation(AttackType.HeavyAttack01,
                            "Main_Heavy_Attack_01", true);
                        break;
                }
            }
            else if (!attacker.IsPerformingAction)
            {
                attacker.CharacterStatsManager.SpendStamina(
                    weapon.BaseStaminaCost * weapon.HeavyAttackStaminaMultiplier);

                attacker.CharacterAnimatorManager.PlayTargetAttackActionAnimation(AttackType.HeavyAttack01,
                    "Main_Heavy_Attack_01", true);
            }
        }
    }
}