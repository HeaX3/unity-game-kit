namespace GameKit
{
    public interface IEntityAnimation : IEntityComponent
    {
        public float animationCrossFade { get; set; }
        void Play(string animation) => Play(animation, animationCrossFade);
        void Play(string animation, float transition);
        void Pause();
        void Resume();
    }
}