using UnityEngine;

public class DynamicArrive : MonoBehaviour
{
    public Kinematic character;
    public Kinematic target;
    public float maxAcceleration;
    public float maxSpeed;

    // Radius for arriving at the target
    public float targetRadius;
    // Radius for slowing down
    public float slowRadius;
    // Time to target
    private float timeToTarget = 0.1f;

    /// <summary>
    /// Calculates the steering output for arriving at the target.
    /// </summary>
    /// <returns>The dynamic steering output.</returns>
    SteeringOutput GetSteering()
    {
        SteeringOutput result = new();
        Vector3 direction = target.transform.position - character.transform.position;
        float distance = direction.magnitude;

        // Check if within target radius, request no steering
        if (distance < targetRadius)
        {
            character.velocity = Vector3.zero;
            result.linear = Vector3.zero;
            result.angular = 0;
            return result;
        }

        // Determine target speed
        float targetSpeed = (distance > slowRadius) ? maxSpeed : maxSpeed * distance / slowRadius;

        // Target velocity combines speed and direction
        Vector3 targetVelocity = direction.normalized * targetSpeed;

        // Acceleration tries to get to the target velocity
        result.linear = (targetVelocity - character.velocity) / timeToTarget;

        // Clamp acceleration to maxAcceleration
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
        character.UpdateSteering(GetSteering(), maxSpeed);
    }
}