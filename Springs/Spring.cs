using SimpleUnitySprings.SpringSolvers;
using UnityEngine;

namespace SimpleUnitySprings
{

    public class Spring : ISpring<float>
    {
        
        private static readonly float REST_VELOCITY_FACTOR = 0.1f;

        private readonly SpringConfig config;
        private float delay;

        private float velocity = 0;

        private float to;
        private float from;
        private float position;

        private bool isAnimating = false;
        private float animateStartTime = 0;
        
        private ISpringSolver springSolver = null;
        
        public Spring(float from, float to, SpringConfig config) : this(from, to, config, new ReactSpringSolver()) {}
        
        public Spring(float from, float to, SpringConfig config, ISpringSolver springSolver)
        {
            this.from = from;
            this.position = from;

            this.config = config;
            this.springSolver = springSolver;
            
            To(to);
        }

        public bool Tick(float deltaTime)
        {
            if (!isAnimating)
            {
                return false;
            }

            if (Time.time < animateStartTime + delay)
            {
                return false;
            }

            if (IsChangeSmall(to, position, velocity))
            {
                isAnimating = false;
                return false;
            }
            
            springSolver.UpdateSpringPosition(from, to, deltaTime, ref position, ref velocity, config);

            return true;
        }
        
        private bool IsChangeSmall(float to, float position, float velocity)
        {
            return Mathf.Abs(velocity) <= config.precision * REST_VELOCITY_FACTOR && Mathf.Abs(to - position) <= config.precision;
        }
        
        public ISpring<float> To(float value, float delay = 0f)
        {
            to = value;
            isAnimating = value != position;
            animateStartTime = Time.time;
            this.delay = delay;
            return this;
        }

        public ISpring<float> Instant(float value)
        {
            position = value;
            isAnimating = false;
            return this;
        }

        public float Get()
        {
            return position;
        }

        public float GetVelocity()
        {
            return velocity;
        }

    }
}