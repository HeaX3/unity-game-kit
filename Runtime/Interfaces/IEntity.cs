using UnityEngine;

namespace GameKit
{
    public interface IEntity
    {
        string type { get; }
        Vector3 position { get; }
    }
}