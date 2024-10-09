using UnityEngine;

public class Align : MonoBehaviour
{
    public Kinematic character;
    public Kinematic target;
    public float maxAngularAcceleration;
    public float maxRotation;
    // The radius for arriving at the target
    public float targetRadius;
    // The radius for beginning to slow down
    public float slowRadius;
    // The time over which to achieve target speed
    private readonly float timeToTarget = 0.1f;

    /// <summary>
    /// Calculates the steering output for aligning with the target.
    /// </summary>
    /// <returns>The dynamic steering output.</returns>
    protected virtual SteeringOutput GetSteering()
    {
        SteeringOutput result = new();
        float rotation;
        float rotationSize;
        float targetRotation;

        // Get the naive direction to the target
        rotation = target.transform.rotation.eulerAngles.z - character.transform.rotation.eulerAngles.z;

        // Map the result to the (-180, 180) interval
        rotation = MapToRange(rotation);
        rotationSize = Mathf.Abs(rotation);

        // Check if we are within the target radius, return no steering
        if (rotationSize < targetRadius)
        {
            result.angular = 0f;
            result.linear = Vector3.zero;
            character.rotation = 0f;
            return result;
        }

        // If we are outside the slowRadius, use max rotation
        targetRotation = (rotationSize > slowRadius) ? maxRotation : maxRotation * rotationSize / slowRadius;

        // Final targetRotation combines speed and direction
        targetRotation *= rotation / rotationSize;

        // Acceleration tries to get to the targetRotation
        result.angular = targetRotation - character.rotation;
        result.angular /= timeToTarget;

        // Check if acceleration is too big
        float angularAcceleration = Mathf.Abs(result.angular);
        if (angularAcceleration > maxAngularAcceleration)
        {
            result.angular /= angularAcceleration;
            result.angular *= maxAngularAcceleration;
        }

        result.linear = Vector3.zero;
        return result;
    }

    /// <summary>
    /// Calculates the steering output for aligning with a specific orientation.
    /// </summary>
    /// <param name="targetOrientation">The target orientation in degrees.</param>
    /// <returns>The dynamic steering output.</returns>
    protected SteeringOutput GetSteeringWithOrientation(float targetOrientation)
    {
        SteeringOutput result = new();
        float rotation;
        float rotationSize;
        float targetRotation;

        // Get the naive direction to the target
        rotation = targetOrientation - character.transform.rotation.eulerAngles.z;

        // Map the result to the (-180, 180) interval
        rotation = MapToRange(rotation);
        rotationSize = Mathf.Abs(rotation);

        // Check if we are within the target radius, return no steering
        if (rotationSize < targetRadius)
        {
            result.angular = 0f;
            result.linear = Vector3.zero;
            character.rotation = 0f;
            return result;
        }

        // If we are outside the slowRadius, use max rotation
        targetRotation = (rotationSize > slowRadius) ? maxRotation : maxRotation * rotationSize / slowRadius;

        // Final targetRotation combines speed and direction
        targetRotation *= rotation / rotationSize;

        // Acceleration tries to get to the targetRotation
        result.angular = targetRotation - character.rotation;
        result.angular /= timeToTarget;

        // Check if acceleration is too big
        float angularAcceleration = Mathf.Abs(result.angular);
        if (angularAcceleration > maxAngularAcceleration)
        {
            result.angular /= angularAcceleration;
            result.angular *= maxAngularAcceleration;
        }

        result.linear = Vector3.zero;
        return result;
    }

    /// <summary>
    /// Maps an angle to the range (-180, 180).
    /// </summary>
    /// <param name="angle">The angle in degrees.</param>
    /// <returns>The mapped angle in the range (-180, 180).</returns>
    float MapToRange(float angle)
    {
        angle = Mathf.Repeat(angle + 180, 360) - 180;
        return angle;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the character's angular steering based on the steering output.
        character.UpdateAngular(GetSteering().angular);
    }
}