using UnityEngine;

namespace ProjectPipe
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        [Header("VFX")]
        [field: SerializeField] private GameObject bloodSplatterVFX;

        private CharacterManager CharacterManager { get; set; }

        protected virtual void Awake()
        {
            CharacterManager = GetComponent<CharacterManager>();
        }

        public void ProcessInstantEffect(InstantCharacterEffect instantCharacterEffect)
        {
            instantCharacterEffect.ProcessEffect(CharacterManager);
        }

        public void PlayBloodSplatterVFX(Vector3 contactPoint)
        {
            Instantiate(
                bloodSplatterVFX ? bloodSplatterVFX : WorldCharacterEffectsManager.Instance.DefaultBloodSplatterVFX,
                contactPoint, Quaternion.identity);
        }
    }
}