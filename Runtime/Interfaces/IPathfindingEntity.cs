using JetBrains.Annotations;
using UnityEngine;

namespace GameKit
{
    public interface IPathfindingEntity : IEntityComponent
    {
        bool moving { get; }
        float speed { get; set; }
        [CanBeNull] Vector3[] path { get; }
        bool Teleport(Vector3 position);
        bool SetDestination(Vector3 position);
        bool SetDestination(Vector3 position, float speed);
        bool CalculatePath(Vector3 position, out Vector3[] path);
        void Cancel();
    }
}