using UnityEngine;

namespace SimpleUnitySprings
{

    [CreateAssetMenu(fileName = "SpringConfig", menuName = "Data/SpringConfig", order = 0)]
    public class SpringConfig : ScriptableObject
    {

        public float tension = 600;
        public float friction = 30;
        public float mass = 1;

        public float trailDelay = 0.1f;


        public static SpringConfig Create(float tension = 600, float friction = 30, float mass = 1, float trailDelay = 0.1f)
        {
            var config = ScriptableObject.CreateInstance<SpringConfig>();
            config.tension = tension;
            config.friction = friction;
            config.mass = mass;
            config.trailDelay = trailDelay;
            return config;
        }

    }
}