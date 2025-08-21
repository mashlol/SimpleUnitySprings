using UnityEngine;

[CreateAssetMenu(fileName = "SpringConfig", menuName = "Data/SpringConfig", order = 0)]
public class SpringConfig : ScriptableObject {

    public float tension = 600;
    public float friction = 30;
    public float mass = 1;

    public float trailDelay = 0.1f;

}