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

        private new Transform transform;
        private Quaternion rotation;
        private Quaternion currentRotation;

        private CharacterController characterController => _characterController;

        public float yaw
        {
            get => _yaw;
            set
            {
                _yaw = value;
                rotation = Quaternion.Euler(0, value, 0);
            }
        }

        private void Awake()
        {
            transform = base.transform;
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
            transform.localRotation = currentRotation;
        }

        private void LateUpdate()
        {
            currentRotation = Quaternion.RotateTowards(currentRotation, rotation, Time.deltaTime * 1080);
            transform.localRotation = currentRotation;
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
            var flags = characterController.Move(motion);
            this.yaw = yaw;
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
            moved(transform.position, yaw);
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