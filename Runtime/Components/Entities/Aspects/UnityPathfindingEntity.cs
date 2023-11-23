using UnityEngine;
using UnityEngine.AI;

namespace GameKit.Entities
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class UnityPathfindingEntity : MonoBehaviour, IPathfindingEntity
    {
        [SerializeField] [HideInInspector] private NavMeshAgent _agent;

        public NavMeshAgent agent => _agent;
        public Vector3[] path => agent.path?.corners;
        
        public float speed
        {
            get => agent.speed;
            set => agent.speed = value;
        }

        public bool moving => agent.hasPath;

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