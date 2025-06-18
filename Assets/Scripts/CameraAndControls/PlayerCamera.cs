using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectPipe
{
    public class PlayerCamera : MonoBehaviour
    {
        [field: SerializeField] public PlayerManager PlayerManager { get; set; }
        [field: SerializeField] public Camera CameraObject { get; set; }

        [Header("Camera Values")]
        [SerializeField] private Transform cameraPivotTransform;
        [SerializeField] private float horizontalLookAngle;
        [SerializeField] private float verticalLookAngle;
        [field: SerializeField] public bool IsLockedOn { get; set; }

        [Header("Camera Settings")]
        [SerializeField] private LayerMask collisionLayer;
        [SerializeField] private float cameraCollisionRadius = 0.2f;
        [SerializeField] private float cameraSmoothSpeed = 0.1f;
        [SerializeField] private float horizontalRotationSpeed = 220f;
        [SerializeField] private float maximumPivotAngle = 60f;
        [SerializeField] private float minimumPivotAngle = -35f;
        [SerializeField] private float verticalRotationSpeed = 220f;
        [SerializeField] private float unlockedCameraHeight = 1.5f;

        [Header("Lock-On Settings")]
        [SerializeField] private float lockOnRadius = 20f;
        [SerializeField] private float lockOnMinimumAngle = -75f;
        [SerializeField] private float lockOnMaximumAngle = 75f;
        [SerializeField] private float lockedCameraHeight = 2f;
        [SerializeField] private float lockOnSmoothSpeed = 1f;
        [SerializeField] private float setCameraHeightSpeed = 0.2f;
        [SerializeField] private List<CharacterManager> targetsInRange = new();
        [SerializeField] private CharacterManager closestTarget;
        [SerializeField] private CharacterManager targetToLeft;
        [SerializeField] private CharacterManager targetToRight;

        private Vector3 _cameraObjectPosition;
        private Vector3 _cameraVelocity;
        private float _cameraZPosition;
        private Coroutine _lockOnCoroutine;
        private Coroutine _lockOnHeightCoroutine;
        private float _targetCameraZPosition;

        public static PlayerCamera Instance { get; private set; }

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _cameraZPosition = CameraObject.transform.localPosition.z;
        }

        private void LateUpdate()
        {
            if (!PlayerManager || !PlayerInputManager.Instance) return;

            HandleFollowTarget();
            if (IsLockedOn) HandleLockedRotation();
            else HandleUnlockedRotation();

            HandleCollisions();
            HandleLockOn();
            HandleLockOnTargetChange();
        }

        private void HandleFollowTarget()
        {
            var targetPosition = Vector3.SmoothDamp(transform.position, PlayerManager.transform.position,
                ref _cameraVelocity, cameraSmoothSpeed);
            transform.position = targetPosition;
        }

        private void HandleLockedRotation()
        {
            var cameraRotationDirection =
                PlayerManager.PlayerCombatManager.CurrentTarget.CharacterCombatManager.LockOnTransform.position -
                transform.position;
            cameraRotationDirection.y = 0;
            cameraRotationDirection.Normalize();

            var cameraTargetRotation = Quaternion.LookRotation(cameraRotationDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, cameraTargetRotation, lockOnSmoothSpeed);

            var pivotRotationDirection =
                PlayerManager.PlayerCombatManager.CurrentTarget.CharacterCombatManager.LockOnTransform.position -
                cameraPivotTransform.position;
            pivotRotationDirection.Normalize();

            var pivotTargetRotation = Quaternion.LookRotation(pivotRotationDirection);
            cameraPivotTransform.transform.rotation = Quaternion.Slerp(cameraPivotTransform.transform.rotation,
                pivotTargetRotation, lockOnSmoothSpeed);

            horizontalLookAngle = transform.eulerAngles.y;
            verticalLookAngle = transform.eulerAngles.x;
        }

        private void HandleUnlockedRotation()
        {
            float sensitivity = WorldSettingsManager.Instance.MouseSensitivity;

            horizontalLookAngle += PlayerInputManager.Instance.CameraInput.x * horizontalRotationSpeed * sensitivity * Time.deltaTime;
            verticalLookAngle -= PlayerInputManager.Instance.CameraInput.y * verticalRotationSpeed * sensitivity * Time.deltaTime;
            verticalLookAngle = Mathf.Clamp(verticalLookAngle, minimumPivotAngle, maximumPivotAngle);

            var cameraRotation = Vector3.zero;
            cameraRotation.y = horizontalLookAngle;

            var cameraTargetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = cameraTargetRotation;

            var pivotCameraRotation = Vector3.zero;
            pivotCameraRotation.x = verticalLookAngle;

            var pivotTargetRotation = Quaternion.Euler(pivotCameraRotation);
            cameraPivotTransform.localRotation = pivotTargetRotation;
        }

        private void HandleCollisions()
        {
            _targetCameraZPosition = _cameraZPosition;
            var direction = CameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out var hit,
                    Mathf.Abs(_targetCameraZPosition), collisionLayer))
            {
                var distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
                _targetCameraZPosition = -(distance - cameraCollisionRadius);
            }

            if (Mathf.Abs(_targetCameraZPosition) < cameraCollisionRadius)
                _targetCameraZPosition = -cameraCollisionRadius;

            _cameraObjectPosition.z = Mathf.Lerp(CameraObject.transform.localPosition.z, _targetCameraZPosition, 0.2f);
            CameraObject.transform.localPosition = _cameraObjectPosition;
        }

        private void HandleLockOn()
        {
            if (!PlayerInputManager.Instance.LockOnInput) return;

            if (IsLockedOn)
            {
                PlayerInputManager.Instance.LockOnInput = false;
                IsLockedOn = false;
                ClearLockOnTargets();
            }
            else
            {
                PlayerInputManager.Instance.LockOnInput = false;
                HandleLocatingTargets();

                if (!closestTarget) return;

                IsLockedOn = true;
                PlayerManager.PlayerCombatManager.SetTarget(closestTarget);
            }
        }

        private void HandleLocatingTargets()
        {
            var closestDistance = Mathf.Infinity;
            var closestDistanceToRightTarget = Mathf.Infinity;
            var closestDistanceToLeftTarget = -Mathf.Infinity;

            var colliders = Physics.OverlapSphere(PlayerManager.transform.position, lockOnRadius,
                WorldUtilityManager.Instance.AILayers);

            foreach (var targetsCollider in colliders)
            {
                var target = targetsCollider.GetComponent<CharacterManager>();

                if (!target) continue;

                if (target.IsDead) continue;

                if (target.transform.root == PlayerManager.transform.root) continue;

                var directionToTarget = target.transform.position - PlayerManager.transform.position;
                var angleToTarget = Vector3.Angle(directionToTarget, CameraObject.transform.forward);

                if (angleToTarget > lockOnMaximumAngle || angleToTarget < lockOnMinimumAngle) continue;

                if (Physics.Linecast(PlayerManager.PlayerCombatManager.LockOnTransform.position,
                        target.CharacterCombatManager.LockOnTransform.position, out var hit,
                        WorldUtilityManager.Instance.EnvironmentLayers)) continue;

                if (!targetsInRange.Contains(target)) targetsInRange.Add(target);
            }

            foreach (var target in targetsInRange)
                if (target)
                {
                    var distanceToTarget =
                        Vector3.Distance(PlayerManager.transform.position, target.transform.position);

                    if (distanceToTarget < closestDistance)
                    {
                        closestDistance = distanceToTarget;
                        closestTarget = target;
                    }

                    if (!IsLockedOn) continue;

                    var relativeEnemyPosition = transform.InverseTransformPoint(target.transform.position);
                    var distanceToRightTarget = relativeEnemyPosition.x;
                    var distanceToLeftTarget = relativeEnemyPosition.x;

                    if (target == PlayerManager.PlayerCombatManager.CurrentTarget) continue;

                    if (relativeEnemyPosition.x <= 0.0 && distanceToLeftTarget > closestDistanceToLeftTarget)
                    {
                        closestDistanceToLeftTarget = distanceToLeftTarget;
                        targetToLeft = target;
                    }
                    else if (relativeEnemyPosition.x >= 0.0 && distanceToRightTarget < closestDistanceToRightTarget)
                    {
                        closestDistanceToRightTarget = distanceToRightTarget;
                        targetToRight = target;
                    }
                }
                else
                {
                    ClearLockOnTargets();
                    IsLockedOn = false;
                }
        }

        private void HandleLockOnTargetChange()
        {
            if (IsLockedOn && PlayerManager.PlayerCombatManager.CurrentTarget &&
                PlayerManager.PlayerCombatManager.CurrentTarget.IsDead)
            {
                if (_lockOnCoroutine != null) StopCoroutine(_lockOnCoroutine);
                _lockOnCoroutine = StartCoroutine(WaitThenFindNewTarget());
            }

            if (PlayerInputManager.Instance.ChangeLockOnToLeft)
            {
                PlayerInputManager.Instance.ChangeLockOnToLeft = false;

                if (IsLockedOn)
                {
                    HandleLocatingTargets();

                    if (targetToLeft) PlayerManager.PlayerCombatManager.SetTarget(targetToLeft);
                }
            }

            if (PlayerInputManager.Instance.ChangeLockOnToRight)
            {
                PlayerInputManager.Instance.ChangeLockOnToRight = false;

                if (IsLockedOn)
                {
                    HandleLocatingTargets();

                    if (targetToRight) PlayerManager.PlayerCombatManager.SetTarget(targetToRight);
                }
            }
        }

        private void ClearLockOnTargets()
        {
            closestTarget = null;
            targetToLeft = null;
            targetToRight = null;
            PlayerManager.CharacterCombatManager.SetTarget(null);
            targetsInRange.Clear();
        }

        private IEnumerator SetCameraHeight()
        {
            if (!PlayerManager) yield break;

            var timer = 0f;

            var velocity = Vector3.zero;
            var newLockedCameraHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, lockedCameraHeight);
            var newUnlockedCameraHeight =
                new Vector3(cameraPivotTransform.transform.localPosition.x, unlockedCameraHeight);

            while (timer < 1f)
            {
                timer += Time.deltaTime;


                if (PlayerManager.PlayerCombatManager.CurrentTarget)
                {
                    var newPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition,
                        newLockedCameraHeight, ref velocity, setCameraHeightSpeed);
                    var newRotation = Quaternion.Slerp(cameraPivotTransform.transform.localRotation,
                        Quaternion.Euler(0, 0, 0), setCameraHeightSpeed);
                    cameraPivotTransform.transform.SetLocalPositionAndRotation(newPosition, newRotation);
                }
                else
                {
                    var newPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition,
                        newUnlockedCameraHeight, ref velocity, setCameraHeightSpeed);
                    var newRotation = Quaternion.RotateTowards(cameraPivotTransform.transform.localRotation,
                        Quaternion.identity, setCameraHeightSpeed);
                    cameraPivotTransform.transform.SetLocalPositionAndRotation(newPosition, newRotation);
                }


                yield return null;
            }

            if (PlayerManager.CharacterCombatManager.CurrentTarget)
                cameraPivotTransform.transform.SetLocalPositionAndRotation(newLockedCameraHeight,
                    Quaternion.Euler(0, 0, 0));
            else
                cameraPivotTransform.transform.localPosition = newUnlockedCameraHeight;

            yield return null;
        }

        public void SetLockOnCameraHeight()
        {
            if (_lockOnHeightCoroutine != null) StopCoroutine(_lockOnHeightCoroutine);
            _lockOnHeightCoroutine = StartCoroutine(SetCameraHeight());
        }

        private IEnumerator WaitThenFindNewTarget()
        {
            while (PlayerManager.IsPerformingAction)
                yield return null;

            ClearLockOnTargets();
            HandleLocatingTargets();

            PlayerManager.PlayerCombatManager.SetTarget(closestTarget);
            IsLockedOn = closestTarget;

            yield return null;
        }
    }
}