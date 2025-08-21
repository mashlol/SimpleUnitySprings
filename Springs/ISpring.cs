namespace SimpleUnitySprings
{

    public interface ISpring<T>
    {

        // True means still animating, false means dest reached
        public bool Tick(float deltaTime);

        public ISpring<T> To(T value, float delay = 0f);

        public ISpring<T> Instant(T value);

        public T Get();

        public T GetVelocity();

    }
}