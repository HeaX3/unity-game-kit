using System.Linq;
using Cameras;
using Cameras.Aspects;
using GameKit.Entities;
using UnityEngine;

namespace GameKit.Players
{
    public class PlayerCamera : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private CameraController _camera;
        
        public CameraFocus focus { get; private set; }
        public CameraInput input { get; private set; }
        public CameraZoom zoom { get; private set; }

        public Camera cameraComponent => camera.camera;
        public new CameraController camera => _camera;
        
        public void Initialize(EntityController controller)
        {
            if (!camera) return;
            focus = camera.components.OfType<CameraFocus>().FirstOrDefault();
            input = camera.components.OfType<CameraInput>().FirstOrDefault();
            zoom = camera.components.OfType<CameraZoom>().FirstOrDefault();
        }

        public void ApplyEntity(IEntity entity)
        {
            
        }

        private void OnValidate()
        {
            if (!_camera)
            {
                _camera = transform.root.GetComponentInChildren<CameraController>(true);
            }
        }
    }
}