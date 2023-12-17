using UnityEngine;

namespace GameKit.Entities
{
    [RequireComponent(typeof(CharacterController))]
    public class NavigationEntity : MonoBehaviour, IEntityComponent
    {
        public delegate void MoveEvent(Vector3 position, float yaw);

        public event MoveEvent moved = delegate { };

        [SerializeField] [HideInInspector] private CharacterController _characterController;

        private float _yaw;
        private Transform _transform;
        private bool _gravityAppliedThisTick = false;
        private bool _velocityUpdatedThisTick = false;

        private new Transform transform
        {
            get
            {
                if (!_transform) _transform = base.transform;
                return _transform;
            }
        }

        private Vector3 previousPosition;
        private Vector3 currentVelocity;
        private Quaternion rotation;
        private Quaternion currentRotation;

        private CharacterController characterController => _characterController;

        public Vector3 velocity => _velocityUpdatedThisTick ? currentVelocity : UpdateVelocity();

        public bool hasGravity { get; set; }
        public bool controlYaw { get; set; } = true;

        public float yaw
        {
            get => _yaw;
            set
            {
                _yaw = value;
                rotation = Quaternion.Euler(0, value, 0);
            }
        }

        private void OnEnable()
        {
            previousPosition = transform.position;
            _velocityUpdatedThisTick = false;
        }

        public void Initialize(EntityController controller)
        {
        }

        public void ApplyEntity(IEntity entity)
        {
        }

        public void SetPositionWithoutNotify(Vector3 position)
        {
            characterController.enabled = false;
            transform.position = position;
            // ReSharper disable once Unity.InefficientPropertyAccess
            characterController.enabled = true;
        }

        public void SetYawWithoutNotify(float yaw)
        {
            this.yaw = yaw;
            currentRotation = rotation;
            if (controlYaw) transform.localRotation = currentRotation;
        }

        private void LateUpdate()
        {
            currentRotation = Quaternion.RotateTowards(currentRotation, rotation, Time.deltaTime * 1080);
            if (controlYaw) transform.localRotation = currentRotation;
            if (!_gravityAppliedThisTick && hasGravity)
            {
                Move(new Vector3(0, -1, 0) * Time.deltaTime);
                _gravityAppliedThisTick = false;
            }

            if (!_velocityUpdatedThisTick) UpdateVelocity();
            previousPosition = transform.position;
            _velocityUpdatedThisTick = false;
        }

        /// <summary>
        ///   Supplies the movement of a GameObject with an attached CharacterController component.
        /// </summary>
        /// <param name="motion">Attempted motion from the current position</param>
        public CollisionFlags Move(Vector3 motion) => Move(motion,
            motion.sqrMagnitude > 0.000001f
                ? motion.x > 0
                    ? Vector3.Angle(Vector3.forward, motion.normalized)
                    : 360 - Vector3.Angle(Vector3.forward, motion.normalized)
                : yaw);

        /// <summary>
        ///   Supplies the movement of a GameObject with an attached CharacterController component.
        /// </summary>
        /// <param name="motion">Attempted motion from the current position</param>
        /// <param name="yaw">Yaw in degrees where 0 points north, 90 points east, 180 points south and 270 points west</param>
        public CollisionFlags Move(Vector3 motion, float yaw)
        {
            if (hasGravity) motion.y -= 1;
            var flags = characterController.Move(motion);
            this.yaw = yaw;
            _gravityAppliedThisTick = true;
            UpdateVelocity();
            moved(transform.position, yaw);
            return flags;
        }

        /// <summary>
        ///   Moves the character with speed.
        /// </summary>
        /// <param name="speed">Speed per second</param>
        public void SimpleMove(Vector3 speed) => SimpleMove(speed,
            speed.sqrMagnitude > 0.0000001f
                ? speed.x > 0
                    ? Vector3.Angle(Vector3.forward, speed.normalized)
                    : 360 - Vector3.Angle(Vector3.forward, speed.normalized)
                : yaw);

        /// <summary>
        ///   Moves the character with speed.
        /// </summary>
        /// <param name="speed">Speed per second</param>
        /// <param name="yaw">Yaw in degrees where 0 points north, 90 points east, 180 points south and 270 points west</param>
        public void SimpleMove(Vector3 speed, float yaw)
        {
            if (!characterController.SimpleMove(speed)) return;
            this.yaw = yaw;
            UpdateVelocity();
            moved(transform.position, yaw);
        }

        private Vector3 UpdateVelocity()
        {
            currentVelocity = (transform.position - previousPosition) / Time.deltaTime;
            _velocityUpdatedThisTick = true;
            return currentVelocity;
        }

        private void OnValidate()
        {
            if (!_characterController || _characterController.gameObject != gameObject)
            {
                _characterController = GetComponent<CharacterController>();
            }
        }
    }
}