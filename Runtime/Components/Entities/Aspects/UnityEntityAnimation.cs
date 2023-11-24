using UnityEngine;

namespace GameKit.Entities
{
    [RequireComponent(typeof(Animator))]
    public class UnityEntityAnimation : MonoBehaviour, IEntityAnimation
    {
        [SerializeField] [HideInInspector] private Animator _animator;

        public float animationCrossFade { get; set; } = 0.1f;

        public void Initialize(EntityController controller)
        {
        }

        public void ApplyEntity(IEntity entity)
        {
        }

        public void Play(string animation, float transition)
        {
            _animator.CrossFadeInFixedTime(animation, transition);
        }

        public void Pause()
        {
            _animator.StopPlayback();
        }

        public void Resume()
        {
            _animator.StartPlayback();
        }
    }
}