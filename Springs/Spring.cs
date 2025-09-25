using SimpleUnitySprings.SpringSolvers;
using UnityEngine;

namespace SimpleUnitySprings
{

    public class Spring : ISpring<float>
    {
        private readonly SpringConfig config;
        private float delay;

        private float velocity = 0;

        private float to;
        private float from;
        private float currentPosition;

        private bool isAnimating = false;
        private float animateStartTime = 0;
        
        private SpringSolvers.AbstractSpringSolver springSolver = null;
        
        public Spring(SpringConfig config, float from, float to)
        {
            this.config = config;
            this.from = from;
            currentPosition = from;
            To(to);
            
            if (config.solver == SpringConfig.SpringSolver.ReactSpring)
            {
                springSolver = new ReactSpringSolver(config);
            }
            else if (config.solver == SpringConfig.SpringSolver.ClosedForm)
            {
                springSolver = new ClosedFormSpringSolver(config);
            }
            else
            {
                throw new System.NotImplementedException($"{nameof(Spring)}: Solver type {config.solver} not yet implemented :(!");
            }
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

            if (IsChangeSmall(to, currentPosition, velocity))
            {
                isAnimating = false;
                return false;
            }
            
            springSolver.UpdateSpringPosition(deltaTime, to, from, ref currentPosition, ref velocity);

            return true;
        }

        private static readonly float REST_VELOCITY_FACTOR = 0.1f;

        private bool IsChangeSmall(float to, float position, float velocity)
        {
            return Mathf.Abs(velocity) <= config.precision * REST_VELOCITY_FACTOR && Mathf.Abs(to - position) <= config.precision;
        }
        
        public ISpring<float> To(float value, float delay = 0f)
        {
            to = value;
            isAnimating = value != currentPosition;
            animateStartTime = Time.time;
            this.delay = delay;
            return this;
        }

        public ISpring<float> Instant(float value)
        {
            currentPosition = value;
            isAnimating = false;
            return this;
        }

        public float Get()
        {
            return currentPosition;
        }

        public float GetVelocity()
        {
            return velocity;
        }

    }
}