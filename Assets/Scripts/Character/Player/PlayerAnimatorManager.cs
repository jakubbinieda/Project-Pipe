namespace ProjectPipe
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        public override void UpdateAnimatorLocomotionValues(float horizontalValue, float verticalValue)
        {
            base.UpdateAnimatorLocomotionValues(horizontalValue, verticalValue);
            _characterManager.Animator.SetBool(_isMovingHash, verticalValue >= 0.5);
        }
    }
}