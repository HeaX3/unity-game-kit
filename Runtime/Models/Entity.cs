using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameKit
{
    public abstract class Entity : IEntity
    {
        public abstract string type { get; }
        public abstract Vector3 position { get; }

        private readonly HashSet<IEntityData> _data = new();

        public T GetData<T>() where T : IEntityData, new()
        {
            return _data.OfType<T>().DefaultIfEmpty(CreateMetadata<T>()).First();
        }

        public IEnumerable<T> GetAllData<T>() where T : IEntityData, new() => _data.OfType<T>();
        
        public IEnumerable<IEntityData> GetAllData() => _data;

        private T CreateMetadata<T>() where T : IEntityData, new()
        {
            var result = new T();
            _data.Add(result);
            return result;
        }
    }
}