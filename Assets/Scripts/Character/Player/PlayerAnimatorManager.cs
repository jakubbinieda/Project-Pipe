using System;
using UnityEngine;

namespace ProjectPipe
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        public override void UpdateAnimatorLocomotionValues(float horizontalValue, float verticalValue)
        {
            base.UpdateAnimatorLocomotionValues(horizontalValue, verticalValue);
            _characterManager.Animator.SetBool(_isMovingHash, Math.Abs(verticalValue) >= 0.5 || Math.Abs(horizontalValue) >= 0.5);
        }
    }
}