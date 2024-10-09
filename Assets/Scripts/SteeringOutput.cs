using UnityEngine;

public class SteeringOutput
{
    public Vector3 linear;
    public float angular;

    /// <summary>
    /// Initializes a new instance of the <see cref="SteeringOutput"/> class with specified linear and angular components.
    /// </summary>
    /// <param name="linear">The linear component of the steering output.</param>
    /// <param name="angular">The angular component of the steering output.</param>
    public SteeringOutput(Vector3 linear, float angular)
    {
        this.linear = linear;
        this.angular = angular;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SteeringOutput"/> class with zero linear and angular components.
    /// </summary>
    public SteeringOutput()
    {
        linear = Vector3.zero;
        angular = 0f;
    }
}