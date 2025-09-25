using UnityEngine;

namespace SimpleUnitySprings
{

    public class Vector3Spring : ISpring<Vector3>
    {
        private readonly Spring xSpring;
        private readonly Spring ySpring;
        private readonly Spring zSpring;

        public Vector3Spring(SpringConfig config, Vector3 from, Vector3 to)
        {
            xSpring = new(config, from.x, to.x);
            ySpring = new(config, from.y, to.y);
            zSpring = new(config, from.z, to.z);
        }

        public bool Tick(float deltaTime)
        {
            var xTick = xSpring.Tick(deltaTime);
            var yTick = ySpring.Tick(deltaTime);
            var zTick = zSpring.Tick(deltaTime);

            return xTick || yTick || zTick;
        }

        public ISpring<Vector3> To(Vector3 value, float delay = 0f)
        {
            xSpring.To(value.x, delay);
            ySpring.To(value.y, delay);
            zSpring.To(value.z, delay);
            return this;
        }

        public ISpring<Vector3> Instant(Vector3 value)
        {
            xSpring.Instant(value.x);
            ySpring.Instant(value.y);
            zSpring.Instant(value.z);
            return this;
        }

        public Vector3 Get()
        {
            return new Vector3(xSpring.Get(), ySpring.Get(), zSpring.Get());
        }

        public Vector3 GetVelocity()
        {
            return new Vector3(xSpring.GetVelocity(), ySpring.GetVelocity(), zSpring.GetVelocity());
        }

    }
}