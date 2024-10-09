using UnityEngine;

public class DynamicSeek : MonoBehaviour
{
    public Kinematic character;
    public Kinematic target;
    public float maxAcceleration;
    public float maxSpeed;

    /// <summary>
    /// Calculates the steering output for seeking the target.
    /// </summary>
    /// <returns>The dynamic steering output.</returns>
    protected virtual SteeringOutput GetSteering()
    {
        SteeringOutput result = new();

        // Get the direction to the target.
        result.linear = target.transform.position - character.transform.position;

        // Normalize the direction and scale by maximum acceleration.
        result.linear.Normalize();
        result.linear *= maxAcceleration;

        result.angular = 0;
        return result;
    }

    /// <summary>
    /// Calculates the steering output for seeking a specific position.
    /// </summary>
    /// <param name="targetPosition">The position to seek.</param>
    /// <returns>The dynamic steering output.</returns>
    protected SteeringOutput GetSteeringWithPosition(Vector3 targetPosition)
    {
        SteeringOutput result = new();

        // Get the direction to the target position.
        result.linear = targetPosition - character.transform.position;

        // Normalize the direction and scale by maximum acceleration.
        result.linear.Normalize();
        result.linear *= maxAcceleration;

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