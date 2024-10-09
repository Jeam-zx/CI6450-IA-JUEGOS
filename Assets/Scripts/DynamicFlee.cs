using UnityEngine;

public class DynamicFlee : MonoBehaviour
{
    public Kinematic character;
    public Kinematic target;
    public float maxAcceleration;
    public float maxSpeed;
    public float escapeRadius;

    /// <summary>
    /// Calculates the steering output for fleeing from the target.
    /// </summary>
    /// <returns>The dynamic steering output.</returns>
    protected virtual SteeringOutput GetSteering()
    {
        SteeringOutput result = new();

        // Get the direction to the target or away from it.
        Vector3 direction = character.transform.position - target.transform.position;

        // Check if the character is within the escape radius.
        if (direction.magnitude > escapeRadius)
        {
            // If outside the escape radius, no fleeing movement is needed.
            result.linear = Vector3.zero;
            result.angular = 0;
            character.velocity = Vector3.zero;
            return result;
        }

        // The velocity is along this direction, at maximum acceleration.
        result.linear = direction.normalized * maxAcceleration;
        result.angular = 0;
        return result;
    }

    /// <summary>
    /// Calculates the steering output for fleeing from a specific position.
    /// </summary>
    /// <param name="targetPosition">The position to flee from.</param>
    /// <returns>The dynamic steering output.</returns>
    protected SteeringOutput GetSteeringWithPosition(Vector3 targetPosition)
    {
        SteeringOutput result = new SteeringOutput();

        // Get the direction to the target or away from it.
        Vector3 direction = character.transform.position - targetPosition;

        // Check if the character is within the escape radius.
        if (direction.magnitude > escapeRadius)
        {
            // If outside the escape radius, no fleeing movement is needed.
            result.linear = Vector3.zero;
            result.angular = 0;
            character.velocity = Vector3.zero;
            return result;
        }

        // The velocity is along this direction, at maximum acceleration.
        result.linear = direction.normalized * maxAcceleration;
        result.angular = 0;
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the character's steering based on the steering output.
        character.UpdateSteering(GetSteering(), maxSpeed);
    }
}