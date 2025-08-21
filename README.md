# Simple Unity Springs

Simple and free spring library for Unity, perfect for animating UI or game
elements in your project, heavily inspired by https://www.react-spring.dev/.

## Installation

Open your Unity package Manager, press "Add package from git url" and enter the following URL:

```
https://github.com/mashlol/SimpleUnitySprings.git
```

## Usage

### Basic usage

The `Spring` class lets you animate a single float value.

[Movie_007.webm](https://github.com/user-attachments/assets/69e450f1-d59a-442d-b9b0-f5e811e56947)

```C#
using SimpleUnitySprings;
using UnityEngine;

public class MyBehaviour : MonoBehaviour {

    private Spring spring;

    private void Start() {
        spring = new(
            config: SpringConfig.Create(tension: 600, friction: 3),
            from: 1,
            to: 2f);
    }

    private void Update() {
        spring.Tick(Time.deltaTime);

        transform.localScale = Vector3.one * spring.Get();
    }
}
```

The `Vector3Spring` class lets you animate a Vector3 value.

[Movie_006.webm](https://github.com/user-attachments/assets/96886bf9-8b8c-42b8-8c6a-60a35f738a1e)

```C#
using SimpleUnitySprings;
using UnityEngine;

public class MyBehaviour : MonoBehaviour {

    private Vector3Spring spring;

    private void Start() {
        spring = new(
            config: SpringConfig.Create(tension: 600, friction: 30),
            from: new Vector3(-6.5f, -2.2f, -2f),
            to: new Vector3(6.5f, 4f, -2f));
    }

    private void Update() {
        spring.Tick(Time.deltaTime);

        transform.position = spring.Get();
    }
}
```

### Chaining

You can pass any Spring into `ChainableSpring` to support chaining, so each
invocation of `.To` will queue until the previous has completed.

[Movie_004.webm](https://github.com/user-attachments/assets/3862d1d3-dffa-4fd0-ba82-878619f4850f)

```C#
using SimpleUnitySprings;
using UnityEngine;

public class MySpringChainingBehaviour : MonoBehaviour {

    // Initialize a new Vector3 spring starting at position 0,0,0 and moving to 1,1,1
    private ChainableSpring<Vector3> spring;

    private void Start() {
        spring = new(new Vector3Spring(
            config: SpringConfig.Create(tension: 600, friction: 30),
            from: new Vector3(-6.5f, -2.2f, -2f),
            to: new Vector3(6.5f, 4f, -2f)));

        // One at a time the spring will transition to each destination
        spring
            .To(new Vector3(-9, 5, 0))
            .To(new Vector3(4, -1, -4.6f))
            .To(new Vector3(0, 2f, -6.7f))
            .To(Vector3.zero);
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


