using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameKit
{
    public abstract class Entity : IEntity
    {
        public abstract string type { get; }
        public abstract Vector3 position { get; }

        private readonly HashSet<IEntityData> _dataObjects = new();

        public bool TryGetData<T>(out T data) where T : IEntityData, new()
        {
            data = _dataObjects.OfType<T>().FirstOrDefault();
            return data != null;
        }

        public T GetData<T>() where T : IEntityData, new()
        {
            return _dataObjects.OfType<T>().DefaultIfEmpty(CreateMetadata<T>()).First();
        }

        public IEnumerable<T> GetAllData<T>() where T : IEntityData => _dataObjects.OfType<T>();

        public IEnumerable<IEntityData> GetAllData() => _dataObjects;

        private T CreateMetadata<T>() where T : IEntityData, new()
        {
            var result = new T();
            _dataObjects.Add(result);
            return result;
        }
    }
}