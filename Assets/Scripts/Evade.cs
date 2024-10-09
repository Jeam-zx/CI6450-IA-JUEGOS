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

    // Update is called once per frame
    void Update()
    {
        // Update the character's steering based on the steering output
        character.UpdateSteering(GetSteering(), maxSpeed);
    }
}