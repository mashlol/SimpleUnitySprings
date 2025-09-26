using UnityEngine;

namespace SimpleUnitySprings.SpringSolvers
{
    /// <summary>
    /// Interface for a class that computes the next position and velocity of a spring.
    /// </summary>
    public interface ISpringSolver
    {
        /// <summary>
        /// Computes the next position and velocity of a spring given the current state and deltaTime.
        /// </summary>
        /// <param name="from">Initial value to start from.</param>
        /// <param name="to">Value to animate towards.</param>
        /// <param name="deltaTime">Time since the last call to this method.</param>
        /// <param name="position">Current value of the spring. Should be modified by the solver.</param>
        /// <param name="velocity">Current velocity of the spring. Should be modified by the solver.</param>
        /// <param name="springConfig">Common spring configuration.</param>
        void UpdateSpringPosition(float from, float to, float deltaTime, ref float position,
            ref float velocity, SpringConfig springConfig);
    }
}