using GameKit.Entities;
using UnityEngine;

namespace GameKit.Players
{
    public class PlayerColliderTrigger : MonoBehaviour, IEntityComponent
    {
        public delegate void CollisionEvent(Collider other);

        public event CollisionEvent collided = delegate { };

        public void Initialize(EntityController controller)
        {
        }

        public void ApplyEntity(IEntity entity)
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            collided(other);
        }
    }
}