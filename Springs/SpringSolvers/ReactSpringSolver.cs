using UnityEngine;

namespace SimpleUnitySprings.SpringSolvers
{
    /// <summary>
    /// Spring solver present in react-spring.
    /// </summary>
    /// <remarks>
    /// It's a Semi-Implicit Euler Spring Solver, default of 1000 iterations/second.
    /// </remarks>
    public class ReactSpringSolver: ISpringSolver
    {
        /// <summary>
        /// The number of iterations/seconds to use. react-spring uses 1000 iterations by default.
        /// Lower is faster, but more jittery. Values less than the current framerate will be very visible.
        /// </summary>
        private readonly float stepsPerSecond = 1000.0f;
        private readonly float oneOverStepsPerSecond = 0.001f;
        
        float numStepsRemainder = 0.0f;
        
        public ReactSpringSolver(float stepsPerSecond = 1000.0f)
        {
            this.stepsPerSecond = stepsPerSecond;
            this.oneOverStepsPerSecond = 1.0f / stepsPerSecond;
        }
        
        public void UpdateSpringPosition(float from, float to, float deltaTime, ref float position,
            ref float velocity, SpringConfig springConfig)
        {
            float numStepsFloat = deltaTime * stepsPerSecond + numStepsRemainder;
            var numStepsInt = Mathf.FloorToInt(numStepsFloat);
            
            // Save a float of partial steps we couldn't fit into a full integer step this frame.
            // This keeps the steps more accurate next frame.
            numStepsRemainder = numStepsFloat - numStepsInt;
            
            for (int i = 0; i < numStepsInt; i++)
            {
                float springForce = -springConfig.tension * oneOverStepsPerSecond * oneOverStepsPerSecond * (position - to);
                float dampingForce = -springConfig.friction * oneOverStepsPerSecond * velocity;
                float acceleration = (springForce + dampingForce) / springConfig.mass;

                velocity += acceleration;
                position += velocity;
            }
        }
    }
}