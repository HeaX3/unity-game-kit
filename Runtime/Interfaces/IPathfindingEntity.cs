using JetBrains.Annotations;
using UnityEngine;

namespace GameKit
{
    public interface IPathfindingEntity : IEntityComponent
    {
        float speed { get; set; }
        [CanBeNull] Vector3[] path { get; }
        bool SetDestination(Vector3 position);
        bool SetDestination(Vector3 position, float speed);
        void Cancel();
    }
}