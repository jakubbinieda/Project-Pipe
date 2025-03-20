using UnityEngine;

namespace ProjectPipe
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private Transform cameraPivotTransform;
        [SerializeField] private float horizontalLookAngle;
        [SerializeField] private float verticalLookAngle;

        [SerializeField] private LayerMask collisionLayer;
        [SerializeField] private float cameraCollisionRadius = 0.2f;
        [SerializeField] private float cameraSmoothSpeed = 0.025f;

        [SerializeField] private float horizontalRotationSpeed = 220f;
        [SerializeField] private float maximumPivotAngle = 60f;
        [SerializeField] private float minimumPivotAngle = -35f;
        [SerializeField] private float verticalRotationSpeed = 220f;

        [field: SerializeField] public PlayerManager PlayerManager { get; set; }
        [field: SerializeField] public Camera CameraObject { get; set; }

        private Vector3 _cameraObjectPosition;
        private Vector3 _cameraVelocity;
        private float _cameraZPosition;
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
            Cursor.visible = false; // TODO: Move somewhere else
            _cameraZPosition = CameraObject.transform.localPosition.z;
        }

        private void LateUpdate()
        {
            if (!PlayerManager || !PlayerInputManager.Instance) return;

            HandleFollowTarget();
            HandleRotations();
            HandleCollisions();
        }

        private void HandleFollowTarget()
        {
            var targetPosition = Vector3.SmoothDamp(transform.position, PlayerManager.transform.position,
                ref _cameraVelocity, cameraSmoothSpeed);
            transform.position = targetPosition;
        }

        private void HandleRotations()
        {
            horizontalLookAngle += PlayerInputManager.Instance.CameraInput.x * horizontalRotationSpeed * Time.deltaTime;
            verticalLookAngle -= PlayerInputManager.Instance.CameraInput.y * verticalRotationSpeed * Time.deltaTime;

            verticalLookAngle = Mathf.Clamp(verticalLookAngle, minimumPivotAngle, maximumPivotAngle);

            var rotation = Vector3.zero;
            rotation.y = horizontalLookAngle;

            var targetRotation = Quaternion.Euler(rotation);
            transform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = verticalLookAngle;

            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
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
    }
}