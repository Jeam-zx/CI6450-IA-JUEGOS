using UnityEngine;

public class LookWhereYouAreGoing : Align
{
    /// <summary>
    /// Overrides the GetSteering method from Align to calculate the steering output for looking where you are going.
    /// </summary>
    /// <returns>The dynamic steering output.</returns>
    protected override SteeringOutput GetSteering()
    {
        // Calculate the target to delegate to align
        // Check for a zero direction, and make no change if so
        Vector3 velocity = character.velocity;
        if (velocity.magnitude == 0)
        {
            SteeringOutput result = new()
            {
                linear = Vector3.zero,
                angular = 0
            };
            character.rotation = 0;
            return result;
        }

        // Calculate the target orientation based on the direction of the velocity
        float targetOrientation = Mathf.Atan2(-velocity.x, velocity.y) * Mathf.Rad2Deg;

        // Delegate to align
        return GetSteeringWithOrientation(targetOrientation);
    }

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        // Update the character's angular steering based on the steering output
        character.UpdateAngular(GetSteering().angular);
    }
}