using UnityEngine;

namespace SimpleUnitySprings.SpringSolvers
{
    /* Code obtained from: https://github.com/llamacademy/juicy-springs
     * Which was derived from: https://github.com/thammin/unity-spring
     * Written by Thammin
     * Modified by Chris from LlamAcademy
     * Modified by Jade
        MIT License
        Copyright (c) 2019 Paul Young
        Copyright (c) 2022 LlamAcademy

        Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

        The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

        THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    */
    public class ClosedFormSpringSolver: ISpringSolver
    {
        private float totalTime = 0.0f;

        public ClosedFormSpringSolver(){}

        public void UpdateSpringPosition(float to, float from, float deltaTime, ref float position,
            ref float velocity, SpringConfig springConfig)
        {
            totalTime += deltaTime;

            float c = springConfig.friction;
            float m = springConfig.mass;
            float k = springConfig.tension;
            float v0 = -0.0f; // Initial velocity
            float t = totalTime;

            float zeta = c / (2 * Mathf.Sqrt(k * m)); // damping ratio
            float omega0 = Mathf.Sqrt(k / m); // undamped angular frequency of the oscillator (rad/s)
            float x0 = to - from;

            float omegaZeta = omega0 * zeta;
            float x;
            float v;

            if (zeta < 1) // Under damped
            {
                float omega1 = omega0 * Mathf.Sqrt(1.0f - zeta * zeta); // exponential decay
                float e = Mathf.Exp(-omegaZeta * t);
                float c1 = x0;
                float c2 = (v0 + omegaZeta * x0) / omega1;
                float cos = Mathf.Cos(omega1 * t);
                float sin = Mathf.Sin(omega1 * t);
                x = e * (c1 * cos + c2 * sin);
                v = -e * ((x0 * omegaZeta - c2 * omega1) * cos + (x0 * omega1 + c2 * omegaZeta) * sin);
            }
            else if (zeta > 1) // Over damped
            {
                float omega2 = omega0 * Mathf.Sqrt(zeta * zeta - 1.0f); // frequency of damped oscillation
                float z1 = -omegaZeta - omega2;
                float z2 = -omegaZeta + omega2;
                float e1 = Mathf.Exp(z1 * t);
                float e2 = Mathf.Exp(z2 * t);
                float c1 = (v0 - x0 * z2) / (-2 * omega2);
                float c2 = x0 - c1;
                x = c1 * e1 + c2 * e2;
                v = c1 * z1 * e1 + c2 * z2 * e2;
            }
            else // Critically damped
            {
                float e = Mathf.Exp(-omega0 * t);
                x = e * (x0 + (v0 + omega0 * x0) * t);
                v = e * (v0 * (1 - t * omega0) + t * x0 * (omega0 * omega0));
            }

            position = to - x;
            velocity = v;
        }
    }
}