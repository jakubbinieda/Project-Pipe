using UnityEngine;

namespace ProjectPipe
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        private CharacterManager CharacterManager { get; set; }

        protected virtual void Awake()
        {
            CharacterManager = GetComponent<CharacterManager>();
        }

        public void ProcessInstantEffect(InstantCharacterEffect instantCharacterEffect)
        {
            instantCharacterEffect.ProcessEffect(CharacterManager);
        }
    }
}