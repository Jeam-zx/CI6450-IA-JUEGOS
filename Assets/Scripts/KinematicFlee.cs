using UnityEngine;

public class KinematicFlee : MonoBehaviour
{
    public Kinematic character;
    public Kinematic target;
    public float maxSpeed;
    public float escapeRadius;

    /// <summary>
    /// Calculates the steering output for fleeing from the target.
    /// </summary>
    /// <returns>The kinematic steering output.</returns>
    KinematicSteeringOutput GetSteering()
    {
        KinematicSteeringOutput result = new();

        // Get the direction to the target.
        Vector3 direction = character.transform.position - target.transform.position;
        
        // Check if the character is within the escape radius.
        if (direction.magnitude > escapeRadius)
        {
            // If outside the escape radius, no steering is needed.
            result.velocity = Vector3.zero;
            result.rotation = 0;
            return result;
        }

        // The velocity is along this direction, at full speed.
        result.velocity = direction.normalized * maxSpeed;

        // Face in the direction we want to move.
        character.NewOrientation(character.transform.rotation.eulerAngles.z, result.velocity);

        result.rotation = 0;
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the character's kinematic state based on the steering output.
        character.UpdateKinematic(GetSteering());
    }
}