using UnityEngine;

public class DynamicWander : Face
{
    // Radius and forward offset of the wander circle
    public float wanderOffset;
    public float wanderRadius;

    // Maximum rate of which the wander orientation can change
    public float wanderRate;

    // Current orientation of the wander target
    private float wanderOrientation;

    // Maximum acceleration of the character
    public float maxAcceleration;

    /// <summary>
    /// Calculates the steering output for wandering behavior.
    /// </summary>
    /// <returns>The dynamic steering output.</returns>
    protected override SteeringOutput GetSteering()
    {
        float targetOrientation;
        SteeringOutput result;

        // Calculate the target to delegate to face
        // Update the wander orientation
        wanderOrientation += RandomBinomial() * wanderRate;

        // Calculate the combined target orientation
        targetOrientation = wanderOrientation + character.transform.rotation.eulerAngles.z;
        
        // Calculate the center of the wander circle.
        Vector3 targetPosition = character.transform.position + wanderOffset * AsVector(character.transform.rotation.eulerAngles.z);

        // Calculate the target location
        targetPosition += wanderRadius * AsVector(targetOrientation);

        // 2. Delegate to face
        result = GetSteeringWithPosition(targetPosition);

        // 3. Now set the linear acceleration to be at full acceleration
        // in the direction of the orientation
        result.linear = maxAcceleration * AsVector(character.transform.rotation.eulerAngles.z);
        
        // Return the result
        return result;
    }

    /// <summary>
    /// Converts an orientation angle to a direction vector.
    /// </summary>
    /// <param name="orientation">The orientation angle in degrees.</param>
    /// <returns>The direction vector.</returns>
    Vector3 AsVector(float orientation)
    {
        float radians = orientation * Mathf.Deg2Rad;
        return new Vector3(-Mathf.Sin(radians), Mathf.Cos(radians), 0f);
    }

    /// <summary>
    /// Generates a random number between -1 and 1.
    /// </summary>
    /// <returns>A random float between -1 and 1.</returns>
    float RandomBinomial()
    {
        return Random.Range(-1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Update the character's steering based on the steering output.
        character.UpdateSteering(GetSteering(), maxAcceleration);
    }
}