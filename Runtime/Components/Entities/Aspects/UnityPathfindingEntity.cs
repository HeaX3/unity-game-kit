using UnityEngine;
using UnityEngine.AI;

namespace GameKit.Entities
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class UnityPathfindingEntity : MonoBehaviour, IPathfindingEntity
    {
        [SerializeField] [HideInInspector] private NavMeshAgent _agent;

        private NavMeshPath _path;

        public NavMeshAgent agent => _agent;
        public Vector3[] path => agent.path?.corners;

        public float speed
        {
            get => agent.speed;
            set => agent.speed = value;
        }

        public bool moving => agent.hasPath && agent.velocity.sqrMagnitude < 0.001f &&
                              agent.remainingDistance <= agent.stoppingDistance;

        public void Initialize(EntityController controller)
        {
        }

        public void ApplyEntity(IEntity entity)
        {
        }

        public bool SetDestination(Vector3 position)
        {
            return agent.SetDestination(position);
        }

        public bool SetDestination(Vector3 position, float speed)
        {
            this.speed = speed;
            return agent.SetDestination(position);
        }

        public bool CalculatePath(Vector3 position, out Vector3[] path)
        {
            _path ??= new NavMeshPath();
            var result = agent.CalculatePath(position, _path);
            if (!result)
            {
                path = null;
                return false;
            }

            path = _path.corners;
            return true;
        }

        public void Cancel()
        {
            agent.ResetPath();
        }

        private void OnValidate()
        {
            if (!_agent || _agent.gameObject != gameObject) _agent = GetComponent<NavMeshAgent>();
        }
    }
}