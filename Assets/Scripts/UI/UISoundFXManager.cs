using UnityEngine;

namespace ProjectPipe
{
    public class UISoundFXManager : MonoBehaviour
    {
        public static UISoundFXManager Instance { get; private set; }

        [Header("UI Sounds")]
        [SerializeField] private AudioClip buttonHoverSFX;
        [SerializeField] private AudioClip buttonClickSFX;

        private AudioSource audioSource;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
        }

        public void PlayHover()
        {
            audioSource.PlayOneShot(buttonHoverSFX);
        }

        public void PlayClick()
        {
            audioSource.PlayOneShot(buttonClickSFX);
        }
    }
}
