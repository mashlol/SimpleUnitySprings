using UnityEngine;

namespace SimpleUnitySprings.SpringSolvers
{
    /// <summary>
    /// Spring solver present from React Spring. A.k.a. a Semi-Implicit Euler Spring Solver.
    /// </summary>
    public class ReactSpringSolver: AbstractSpringSolver
    {
        float numStepsRemainder = 0.0f;

        public ReactSpringSolver(SpringConfig springConfig) : base(springConfig){}
        
        public override void UpdateSpringPosition(float deltaTime, float to, float from, ref float position, ref float velocity)
        {
            float samplesPerSecond = springConfig.reactSpringSolverIterations;
            float oneOverSamplesPerSecond = 1.0f / samplesPerSecond;
            
            float numStepsFloat = deltaTime * samplesPerSecond + numStepsRemainder;
            
            var numStepsInt = Mathf.FloorToInt(numStepsFloat);
            
            // Save a float of partial steps we couldn't fit into a full integer step this frame.
            // This keeps it more accurate.
            numStepsRemainder = numStepsFloat - numStepsInt;
            
            for (int i = 0; i < numStepsInt; i++)
            {
                float springForce = -springConfig.tension * oneOverSamplesPerSecond * oneOverSamplesPerSecond * (position - to);
                float dampingForce = -springConfig.friction * oneOverSamplesPerSecond * velocity;
                float acceleration = (springForce + dampingForce) / springConfig.mass;

                velocity += acceleration;
                position += velocity;
            }
        }
    }
}