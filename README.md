# Simple Unity Springs

Simple and free spring library for Unity, perfect for animating UI or game elements in your project.

## Installation

Open your Unity package Manager and press "Add package from git url".

```
https://github.com/mashlol/SimpleUnitySprings
```

## Usage

### Basic usage

```C#
public class MyBehaviour : MonoBehaviour {

    // Initialize a new Vector3 spring starting at position 0,0,0 and moving to 1,1,1
    private readonly Vector3Spring spring = new(
        config: SpringConfig.Create(tension: 600, friction: 30),
        from: Vector3.zero,
        to: Vector3.one);

    private void Update() {
        spring.Tick(Time.deltaTime);

        transform.position = spring.Get();
    }
}
```

### Chaining

```C#
public class MySpringChainingBehaviour : MonoBehaviour {

    // Initialize a new Vector3 spring starting at position 0,0,0 and moving to 1,1,1
    private readonly ChainableSpring<Vector3> spring = new(
        new Vector3Spring(
            config: SpringConfig.Create(tension: 600, friction: 30),
            from: Vector3.zero,
            to: Vector3.one));

    private void Start() {
        // One at a time the spring will transition to each destination
        spring.To(Vector3.one * 2f);
        spring.To(Vector3.one * -2f);
        spring.To(Vector3.one * 4f);
        spring.To(Vector3.one * -4f);
    }

    private void Update() {
        spring.Tick(Time.deltaTime);

        transform.position = spring.Get();
    }
}
```

### Spring Configs

A spring config tells the spring how to behave, and can either be created at
runtime with SpringConfig.Create() or as a ScriptableObject in your project via
Create -> Data -> SpringConfig.

It's recommended to use a ScriptableObject in your scene to share spring configs
with multiple springs, as well as to be able to easily edit the configs at
runtime to tweak values.