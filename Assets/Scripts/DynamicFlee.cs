using UnityEngine;

public class DynamicFlee : MonoBehaviour
{
    public Kinematic character;
    public Kinematic target;
    public float maxAcceleration;
    public float maxSpeed;

    /// <summary>
    /// Calculates the steering output for fleeing from the target.
    /// </summary>
    /// <returns>The dynamic steering output.</returns>
    protected virtual SteeringOutput GetSteering()
    {
        SteeringOutput result = new();

        // Get the direction to the target or away from it.
        Vector3 direction = character.transform.position - target.transform.position;

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
        SteeringOutput result = new();

        // Get the direction to the target or away from it.
        Vector3 direction = character.transform.position - targetPosition;

        // The velocity is along this direction, at maximum acceleration.
        result.linear = direction.normalized * maxAcceleration;
        result.angular = 0;
        return result;
    }

    /// <summary>
    /// Adjusts the character's velocity and steering output to handle world boundaries.
    /// </summary>
    /// <param name="steering">The current steering output.</param>
    /// <returns>The adjusted steering output.</returns>
    private SteeringOutput HandleWorldBoundaries(SteeringOutput steering)
    {
        // Calculate the future position of the character
        Vector3 futurePosition = character.transform.position + character.velocity * Time.deltaTime;
    
        // Check and handle the x boundaries
        if (futurePosition.x > 10 || futurePosition.x < -10)
        {
            character.velocity.x = 0;
            steering.linear.x = 0;
        }
    
        // Check and handle the y boundaries
        if (futurePosition.y > 5 || futurePosition.y < -5)
        {
            character.velocity.y = 0;
            steering.linear.y = 0;
        }
    
        return steering;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the character's steering based on the steering output
        character.UpdateSteering(HandleWorldBoundaries(GetSteering()), maxSpeed);
    }
}