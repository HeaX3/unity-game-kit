using JetBrains.Annotations;
using UnityEngine;

namespace GameKit
{
    public interface IEntity
    {
        string type { get; }
        Vector3 position { get; }

        /**
         * Get a metadata block of the specified type.
         * If no block of the requested type is present, a new one is created.
         * The returned data is mutable.
         */
        [NotNull]
        T GetData<T>() where T : IEntityData, new() => new();
    }
}