using UnityEngine;

public class VelocityMatching : MonoBehaviour
{
    public Kinematic character;
    public Kinematic target;
    public float maxAcceleration;
    // Time over which to achieve target speed
    public float timeToTarget = 0.1f;

    /// <summary>
    /// Calculates the steering output for matching the velocity of the target.
    /// </summary>
    /// <returns>The dynamic steering output.</returns>
    protected SteeringOutput GetSteering()
    {
        SteeringOutput result = new()
        {
            // Acceleration tries to get to the target velocity
            linear = (target.velocity - character.velocity) / timeToTarget
        };

        // Check if the acceleration is too fast
        if (result.linear.magnitude > maxAcceleration)
        {
            result.linear = result.linear.normalized * maxAcceleration;
        }

        result.angular = 0;
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the character's steering based on the steering output
        character.UpdateSteering(GetSteering(), maxAcceleration);
    }
}