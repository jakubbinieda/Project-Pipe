using UnityEngine;

namespace ProjectPipe
{
    public class Enums : MonoBehaviour
    {
    }

    public enum WeaponModelSlot
    {
        Hand,
        Hips
    }

    public enum AttackType
    {
        LightAttack01,
        LightAttack02,
        HeavyAttack01,
        HeavyAttack02,
        ChargedAttack01,
        ChargedAttack02,
        RunAttack01,
        RollAttack01,
        BackstepAttack01
    }

    public enum GameSlot
    {
        GameSlot_01,
        GameSlot_02,
        GameSlot_03
    }
}