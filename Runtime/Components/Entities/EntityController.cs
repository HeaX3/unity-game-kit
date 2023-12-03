using ObjectPooling;
using UnityEngine;

namespace GameKit.Entities
{
    public sealed class EntityController : MonoBehaviour, IPoolable
    {
        public delegate void UpdateEvent();

        public event UpdateEvent invisibleChanged = delegate { };

        [SerializeField] private Transform _meshTransform;

        public IEntityComponent[] components { get; private set; }

        private Transform _transform;
        private IEntity _entity;

        private new Transform transform
        {
            get
            {
                if (!_transform) _transform = base.transform;
                return _transform;
            }
        }

        public string type => entity?.type;

        private Transform meshTransform => _meshTransform;

        public IEntity entity
        {
            get => _entity;
            set => ApplyEntity(value);
        }

        private void Awake()
        {
            components = GetComponentsInChildren<IEntityComponent>();
            foreach (var component in components)
            {
                component.Initialize(this);
            }
        }

        public void Activate()
        {
            OnActivate();
        }

        private void OnEnable()
        {
        }

        public void ResetForPool()
        {
            _entity = null;
        }

        public void UpdateEntity()
        {
            foreach (var component in components)
            {
                component.ApplyEntity(_entity);
            }
        }

        private void ApplyEntity(IEntity entity)
        {
            _entity = entity;
            if (entity != null)
            {
                transform.position = entity.position;
            }

            UpdateEntity();
        }

        private void OnDisable()
        {
        }

        private void OnActivate()
        {
        }

        private void OnInitialize()
        {
        }

        private void OnVisibilityChanged(bool value)
        {
        }
    }
}