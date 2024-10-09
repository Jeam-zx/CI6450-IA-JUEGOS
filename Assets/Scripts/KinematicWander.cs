using UnityEngine;

public class KinematicWander : MonoBehaviour
{
    public Kinematic character;
    public float maxSpeed;
    
    // Maximum rotation speed
    public float maxRotation;

    /// <summary>
    /// Calculates the steering output for wandering behavior.
    /// </summary>
    /// <returns>The kinematic steering output.</returns>
    KinematicSteeringOutput GetSteering()
    {
        KinematicSteeringOutput result = new()
        {
            // Get velocity from vector form of orientation
            velocity = maxSpeed * AsVector(character.transform.rotation.eulerAngles.z),

            // Change orientation randomly
            rotation = RandomBinomial() * maxRotation
        };

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
        // Update the character's kinematic state based on the steering output.
        character.UpdateKinematic(GetSteering());
    }
}