using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectPipe
{
    public class CharacterManager : MonoBehaviour
    {
        [field: Header("Flags")]
        [field: SerializeField] public bool ApplyRootMotion { get; set; }
        [field: SerializeField] public bool CanRotate { get; set; } = true;
        [field: SerializeField] public bool CanMove { get; set; } = true;
        [field: SerializeField] public bool IsGrounded { get; set; } = true;
        [field: SerializeField] public bool IsJumping { get; set; }
        [field: SerializeField] public bool IsPerformingAction { get; set; }
        [field: SerializeField] public bool IsSprinting { get; set; }

        [field: Header("Status")]
        [field: SerializeField] public bool IsDead { get; private set; }

        public Animator Animator { get; private set; }
        public CharacterAnimatorManager CharacterAnimatorManager { get; private set; }
        public CharacterCombatManager CharacterCombatManager { get; private set; }
        public CharacterController CharacterController { get; private set; }
        public CharacterEffectsManager CharacterEffectsManager { get; private set; }
        public CharacterStatsManager CharacterStatsManager { get; private set; }

        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);

            Animator = GetComponent<Animator>();
            CharacterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            CharacterCombatManager = GetComponent<CharacterCombatManager>();
            CharacterController = GetComponent<CharacterController>();
            CharacterEffectsManager = GetComponent<CharacterEffectsManager>();
            CharacterStatsManager = GetComponent<CharacterStatsManager>();
        }

        protected virtual void Start()
        {
            IgnoreOwnColliders();
        }

        private void IgnoreOwnColliders()
        {
            var characterControllerCollider = GetComponent<Collider>();
            var damageableCharacterColliders = GetComponentsInChildren<Collider>();

            List<Collider> allColliders = new();
            allColliders.AddRange(damageableCharacterColliders);
            allColliders.Add(characterControllerCollider);

            for (var i = 0; i < allColliders.Count; i++)
            for (var j = i + 1; j < allColliders.Count; j++)
                Physics.IgnoreCollision(allColliders[i], allColliders[j], true);
        }

        public IEnumerator ProcessDeathEvent()
        {
            IsDead = true;

            CharacterAnimatorManager.PlayTargetAnimation("Death_01", true, true);

            yield return new WaitForSeconds(5f);
        }

        protected virtual void FixedUpdate() {}

        protected virtual void Update() {}
    }
}