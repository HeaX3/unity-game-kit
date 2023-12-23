using System.Collections;
using UnityEngine;

namespace GameKit.Entities
{
    [RequireComponent(typeof(CharacterController))]
    public class NavigationEntity : MonoBehaviour, IEntityComponent
    {
        public delegate void MoveEvent(Vector3 position, Quaternion rotation);

        public event MoveEvent moved = delegate { };

        [SerializeField] [HideInInspector] private CharacterController _characterController;

        private Transform _transform;
        private bool _movementAppliedThisTick;

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

        public Vector3 velocity => currentVelocity;

        public bool hasGravity { get; set; }
        public bool controlYaw { get; set; } = true;
        public bool controlRotation { get; set; } = false;

        public float yaw { get; private set; }

        private void OnEnable()
        {
            previousPosition = transform.position;
        }

        public void Initialize(EntityController controller)
        {
        }

        public void ApplyEntity(IEntity entity)
        {
        }

        public void SetPositionWithoutNotify(Vector3 position)
        {
            previousPosition = transform.position;
            transform.position = position;
            UpdateVelocity();
        }

        public void SetYawWithoutNotify(float yaw)
        {
            this.yaw = yaw;
            rotation = Quaternion.Euler(0, yaw, 0);
            currentRotation = rotation;
            if (controlYaw) transform.localRotation = currentRotation;
        }

        public void SetRotationWithoutNotify(Quaternion rotation)
        {
            yaw = rotation.eulerAngles.y;
            currentRotation = rotation;
            if (controlRotation) transform.localRotation = currentRotation;
        }

        private void LateUpdate()
        {
            currentRotation = Quaternion.RotateTowards(currentRotation, rotation, Time.deltaTime * 1080);
            if (controlYaw || controlRotation) transform.localRotation = currentRotation;
            if (!_movementAppliedThisTick)
            {
                if (hasGravity) Move(new Vector3(0, -1, 0) * Time.deltaTime);
                else UpdateVelocity();
            }

            _movementAppliedThisTick = false;
        }

        /// <summary>
        ///   Supplies the movement of a GameObject with an attached CharacterController component.
        /// </summary>
        /// <param name="motion">Attempted motion from the current position</param>
        public CollisionFlags Move(Vector3 motion) => Move(motion,
            motion.sqrMagnitude > 0.0001f ? Quaternion.LookRotation(motion) : rotation);

        /// <summary>
        ///   Supplies the movement of a GameObject with an attached CharacterController component.
        /// </summary>
        /// <param name="motion">Attempted motion from the current position</param>
        /// <param name="yaw">Yaw in degrees where 0 points north, 90 points east, 180 points south and 270 points west</param>
        public CollisionFlags Move(Vector3 motion, float yaw) => Move(motion, Quaternion.Euler(0, yaw, 0));

        /// <summary>
        ///   Supplies the movement of a GameObject with an attached CharacterController component.
        /// </summary>
        /// <param name="motion">Attempted motion from the current position</param>
        /// <param name="rotation">Rotation</param>
        public CollisionFlags Move(Vector3 motion, Quaternion rotation)
        {
            if (hasGravity) motion.y -= 1;
            var flags = characterController.Move(motion);
            yaw = rotation.eulerAngles.y;
            this.rotation = rotation;
            _movementAppliedThisTick = true;
            UpdateVelocity();
            moved(transform.position, rotation);
            return flags;
        }

        /// <summary>
        ///   Moves the character with speed.
        /// </summary>
        /// <param name="speed">Speed per second</param>
        public void SimpleMove(Vector3 speed) => SimpleMove(speed, controlRotation && speed.sqrMagnitude > 0.0001f
            ? Quaternion.LookRotation(speed)
            : rotation);

        /// <summary>
        ///   Moves the character with speed.
        /// </summary>
        /// <param name="speed">Speed per second</param>
        /// <param name="yaw">Yaw in degrees where 0 points north, 90 points east, 180 points south and 270 points west</param>
        public void SimpleMove(Vector3 speed, float yaw) => SimpleMove(speed, controlYaw
            ? Quaternion.Euler(0, yaw, 0)
            : rotation);

        /// <summary>
        ///   Moves the character with speed.
        /// </summary>
        /// <param name="speed">Speed per second</param>
        /// <param name="rotation">Rotation</param>
        public void SimpleMove(Vector3 speed, Quaternion rotation)
        {
            if (!characterController.SimpleMove(speed)) return;
            yaw = rotation.eulerAngles.y;
            this.rotation = rotation;
            _movementAppliedThisTick = true;
            UpdateVelocity();
            moved(transform.position, rotation);
        }

        private void UpdateVelocity()
        {
            var position = transform.position;
            currentVelocity = (position - previousPosition) / Time.deltaTime;
            previousPosition = position;
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