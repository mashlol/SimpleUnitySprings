using SimpleUnitySprings.SpringSolvers;
using UnityEngine;

namespace SimpleUnitySprings
{

    public class Vector3Spring : ISpring<Vector3>
    {
        private readonly Spring xSpring;
        private readonly Spring ySpring;
        private readonly Spring zSpring;
        
        public Vector3Spring(Vector3 from, Vector3 to, SpringConfig config ) : this(from, to, config, new ReactSpringSolver()) {}
        
        public Vector3Spring(Vector3 from, Vector3 to, SpringConfig config, ISpringSolver springSolver )
        {
            xSpring = new(from.x, to.x, config, springSolver);
            ySpring = new(from.y, to.y, config, springSolver);
            zSpring = new(from.z, to.z, config, springSolver);
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