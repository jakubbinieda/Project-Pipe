using UnityEngine;

namespace ProjectPipe
{
    public class InstantCharacterEffect : ScriptableObject
    {
        public int InstantEffectID { get; set; }

        public virtual void ProcessEffect(CharacterManager characterManager)
        {
        }
    }
}