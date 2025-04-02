using UnityEngine;

namespace ProjectPipe
{
    public class WeaponItemAction : ScriptableObject
    {
        public int ActionID { get; set; }

        public virtual void AttemptToPerformAction(CharacterManager attacker, WeaponItem weapon)
        {
        }
    }
}