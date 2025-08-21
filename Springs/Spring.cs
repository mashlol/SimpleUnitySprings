using UnityEngine;

public class Spring : ISpring<float> {

    private static readonly float SMALL_FLOAT = 0.00001f;
    private static readonly float REST_VELOCITY_FACTOR = 0.1f;

    private readonly SpringConfig config;
    private readonly float precision;
    private float delay;

    private float velocity = 0;

    private float to;
    private float position;

    private bool isAnimating = false;
    private float animateStartTime = 0;


    public Spring(SpringConfig config, float from, float to, float? precision = null) {
        this.precision = precision ?? SMALL_FLOAT;

        this.config = config;
        position = from;
        this.to = to;
    }

    public bool Tick(float deltaTime) {
        if (!isAnimating) {
            return false;
        }

        if (Time.time < animateStartTime + delay) {
            return false;
        }

        var numSteps = Mathf.CeilToInt(deltaTime * 1000);
        for (int i = 0; i < numSteps; i++) {
            if (Mathf.Abs(velocity) <= precision * REST_VELOCITY_FACTOR && Mathf.Abs(to - position) <= precision) {
                // Finished.
                isAnimating = false;
                return false;
            }

            float springForce = -config.tension * 0.000001f * (position - to);
            float dampingForce = -config.friction * 0.001f * velocity;
            float acceleration = (springForce + dampingForce) / config.mass;

            velocity += acceleration;
            position += velocity;
        }

        return true;
    }

    public ISpring<float> To(float value, float delay = 0f) {
        to = value;
        isAnimating = true;
        animateStartTime = Time.time;
        this.delay = delay;
        return this;
    }

    public ISpring<float> Instant(float value) {
        position = value;
        isAnimating = false;
        return this;
    }

    public float Get() {
        return position;
    }

    public float GetVelocity() {
        return velocity;
    }

}