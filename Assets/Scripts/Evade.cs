using UnityEngine;

public class Evade : DynamicFlee
{
    // Maximum prediction time
    public float maxPrediction;

    /// <summary>
    /// Calculates the steering output for evading the target.
    /// </summary>
    /// <returns>The dynamic steering output.</returns>
    protected override SteeringOutput GetSteering()
    {
        Vector3 direction;
        float distance;
        float speed;
        float prediction;

        // Calculate the target to delegate to seek
        // Calculate the distance to the target
        direction = target.transform.position - character.transform.position;
        distance = direction.magnitude;

        // Calculate our current speed
        speed = character.velocity.magnitude;

        // Check if the speed gives a reasonable prediction time
        if (speed <= distance / maxPrediction)
            prediction = maxPrediction;
        else
            prediction = distance / speed;

        // Calculate the predicted target position
        Vector3 targetPosition = target.transform.position;
        targetPosition += target.velocity * prediction;

        // Delegate to GetSteeringWithPosition to get the steering output
        return GetSteeringWithPosition(targetPosition);
    }

    /// <summary>
    /// Adjusts the character's velocity and steering output to handle world boundaries.
    /// </summary>
    /// <param name="result">The current steering output.</param>
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