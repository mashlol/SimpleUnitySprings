using UnityEngine;
using UnityEngine.Serialization;

namespace SimpleUnitySprings
{

    [CreateAssetMenu(fileName = "SpringConfig", menuName = "Data/SpringConfig", order = 0)]
    public class SpringConfig : ScriptableObject
    {
        public enum SpringSolver
        {
            /// <summary>
            /// Iterative approach used in React-Spring.
            /// </summary>
            ReactSpring,
            /// <summary>
            /// Single step closed form solver, (Less iterations, but slower than React-Spring)
            /// </summary>
            ClosedForm,
        }
        public SpringSolver solver = SpringSolver.ReactSpring;
        
        /// <summary>
        /// If the solver is React-Spring, this is the number of iterations/seconds to use.
        /// At least 60 (for 60fps) is recommended. Higher is more accurate, but slower.
        /// React Spring uses 1000 iterations by default.
        /// </summary>
        public float reactSpringSolverIterations = 120;
        
        public float tension = 600;
        public float friction = 30;
        public float mass = 1;

        public float precision = 0.00001f;
        public float trailDelay = 0.1f;
        
        public static SpringConfig Create(float tension = 600, float friction = 30, float mass = 1, float trailDelay = 0.1f, float precision = 0.00001f, SpringSolver solver = SpringSolver.ClosedForm)
        {
            var config = ScriptableObject.CreateInstance<SpringConfig>();
            config.tension = tension;
            config.friction = friction;
            config.mass = mass;
            config.trailDelay = trailDelay;
            config.precision = precision;
            config.solver = solver;
            return config;
        }

    }
}