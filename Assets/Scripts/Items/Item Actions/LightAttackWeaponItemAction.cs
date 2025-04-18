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

            attacker.CharacterStatsManager.SpendStamina(weapon.BaseStaminaCost * weapon.LightAttackStaminaMultiplier);


            if (attacker.IsSprinting)
                PerformRunAttack(attacker, weapon);
            else if (attacker.CharacterCombatManager.CanDoRollAttack)
                PerformRollAttack(attacker, weapon);
            else if (attacker.CharacterCombatManager.CanDoBackstepAttack)
                PerformBackstepAttack(attacker, weapon);
            else
                PerformDefaultAttack(attacker, weapon);
        }

        private void PerformDefaultAttack(CharacterManager attacker, WeaponItem weapon)
        {
            if (attacker.CharacterCombatManager.CanDoCombo && attacker.IsPerformingAction)
            {
                attacker.CharacterCombatManager.DisableCanDoCombo();

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
                attacker.CharacterAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01,
                    "Main_Light_Attack_01", true);
            }
        }

        private void PerformRunAttack(CharacterManager attacker, WeaponItem weapon)
        {
            attacker.CharacterAnimatorManager.PlayTargetAttackActionAnimation(AttackType.RunAttack01,
                "Main_Run_Attack_01", true);
        }

        private void PerformRollAttack(CharacterManager attacker, WeaponItem weapon)
        {
            attacker.CharacterCombatManager.DisableCanDoRollAttack();
            attacker.CharacterAnimatorManager.PlayTargetAttackActionAnimation(AttackType.RollAttack01,
                "Main_Roll_Attack_01", true);
        }

        private void PerformBackstepAttack(CharacterManager attacker, WeaponItem weapon)
        {
            attacker.CharacterCombatManager.DisableCanDoBackstepAttack();
            attacker.CharacterAnimatorManager.PlayTargetAttackActionAnimation(AttackType.BackstepAttack01,
                "Main_Backstep_Attack_01", true);
        }
    }
}