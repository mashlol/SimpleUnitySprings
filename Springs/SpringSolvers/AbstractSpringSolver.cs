using UnityEngine;

namespace SimpleUnitySprings.SpringSolvers
{
    public abstract class AbstractSpringSolver
    {
        protected SpringConfig springConfig;

        public AbstractSpringSolver(SpringConfig springConfig)
        {
            this.springConfig = springConfig;
        }
        
        public abstract void UpdateSpringPosition(float deltaTime, float to, float from, ref float position, ref float velocity);
    }
}