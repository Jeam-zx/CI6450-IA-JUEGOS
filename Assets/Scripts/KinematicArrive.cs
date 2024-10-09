using UnityEngine;

public class KinematicArrive : MonoBehaviour
{
    public Kinematic character;
    public Kinematic target;
    public float maxSpeed;

    // Satisfaction radius
    public float radius;

    // Time to target
    public float timeToTarget = 0.25f;

    /// <summary>
    /// Calculates the steering output for arriving at the target.
    /// </summary>
    /// <returns>The kinematic steering output.</returns>
    KinematicSteeringOutput GetSteering()
    {
        KinematicSteeringOutput result = new KinematicSteeringOutput();

        // Get direction to target
        result.velocity = target.transform.position - character.transform.position;

        // Check if within radius
        if (result.velocity.magnitude < radius)
        {
            // If within the satisfaction radius, stop moving
            result.velocity = Vector3.zero;
            return result;
        }

        // Move to target
        result.velocity /= timeToTarget;

        // If too fast, clamp to maxSpeed
        if (result.velocity.magnitude > maxSpeed)
        {
            result.velocity = result.velocity.normalized * maxSpeed;
        }

        // Face in the direction we want to move
        character.NewOrientation(character.transform.rotation.eulerAngles.z, result.velocity);

        result.rotation = 0;
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the character's kinematic state based on the steering output
        character.UpdateKinematic(GetSteering());
    }
}