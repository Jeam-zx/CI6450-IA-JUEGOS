using UnityEngine;

public class Face : Align
{
    /// <summary>
    /// Calculates the steering output for facing the target.
    /// </summary>
    /// <returns>The dynamic steering output.</returns>
    protected override SteeringOutput GetSteering()
    {
        Vector3 direction = target.transform.position - character.transform.position;

        // Check for a zero direction, and make no change if so
        if (direction.magnitude == 0)
        {
            SteeringOutput result = new(){
                linear = Vector3.zero,
                angular = 0
            };
            return result;
        }

        // Calculate the target orientation based on the direction to the target
        float targetOrientation = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;

        // Delegate to GetSteeringWithOrientation to get the steering output
        return GetSteeringWithOrientation(targetOrientation);
    }

    /// <summary>
    /// Calculates the steering output for facing a specific position.
    /// </summary>
    /// <param name="targetPosition">The position to face.</param>
    /// <returns>The dynamic steering output.</returns>
    protected SteeringOutput GetSteeringWithPosition(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - character.transform.position;

        // Check for a zero direction, and make no change if so
        if (direction.magnitude == 0)
        {
            SteeringOutput result = new(){
                linear = Vector3.zero,
                angular = 0
            };
            return result;
        }

        // Calculate the target orientation based on the direction to the target position
        float targetOrientation = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;

        // Delegate to GetSteeringWithOrientation to get the steering output
        return GetSteeringWithOrientation(targetOrientation);
    }

    // Update is called once per frame
    void Update()
    {
        // Update the character's angular steering based on the steering output
        character.UpdateAngular(GetSteering().angular);
    }
}