using GameKit.Entities;

namespace GameKit
{
    public interface IEntityComponent
    {
        void Initialize(EntityController controller);
        void ApplyEntity(IEntity entity);
    }
}