using GameKit.Entities;
using UnityEngine;

namespace GameKit
{
    public interface IEntityComponent
    {
        void Initialize(EntityController controller);
        void ApplyEntity(IEntity entity);
    }

    public interface IEntityComponent<in T> : IEntityComponent where T : IEntity
    {
        void IEntityComponent.ApplyEntity(IEntity entity)
        {
            if (entity == null)
            {
                ApplyEntity(default);
                return;
            }

            if (entity is not T t)
            {
                Debug.LogWarning("Cannot assign entity of type " + entity.GetType().Name + " as " + typeof(T).Name);
                return;
            }

            ApplyEntity(t);
        }

        void ApplyEntity(T entity);
    }
}