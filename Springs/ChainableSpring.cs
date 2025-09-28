using System.Collections.Generic;

namespace SimpleUnitySprings
{
    public class ChainableSpring<T> : ISpring<T>
    {
        private readonly ISpring<T> internalSpring;
        private readonly Queue<(T, float)> destinationQueue = new();

        public ChainableSpring(ISpring<T> internalSpring)
        {
            this.internalSpring = internalSpring;
        }

        public bool Tick(float deltaTime)
        {
            var stillProcessing = internalSpring.Tick(deltaTime);

            if (!stillProcessing && destinationQueue.Count > 0)
            {
                var nextDest = destinationQueue.Dequeue();
                internalSpring.To(nextDest.Item1, nextDest.Item2);

                return true;
            }

            return stillProcessing;
        }

        public ISpring<T> To(T value, float delay = 0)
        {
            destinationQueue.Enqueue((value, delay));
            return this;
        }

        public ISpring<T> Instant(T value)
        {
            internalSpring.Instant(value);
            destinationQueue.Clear();
            return this;
        }

        public T Get()
        {
            return internalSpring.Get();
        }

        public T GetVelocity()
        {
            return internalSpring.GetVelocity();
        }

    }
}