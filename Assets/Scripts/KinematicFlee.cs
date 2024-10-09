using UnityEngine;

public class KinematicFlee : MonoBehaviour
{
    public Kinematic character;
    public Kinematic target;
    public float maxSpeed;

    /// <summary>
    /// Calculates the steering output for fleeing from the target.
    /// </summary>
    /// <returns>The kinematic steering output.</returns>
    KinematicSteeringOutput GetSteering()
    {
        KinematicSteeringOutput result = new();

        // Get the direction to the target.
        Vector3 direction = character.transform.position - target.transform.position;

        // The velocity is along this direction, at full speed.
        result.velocity = direction.normalized * maxSpeed;

        // Face in the direction we want to move.
        character.NewOrientation(character.transform.rotation.eulerAngles.z, result.velocity);

        result.rotation = 0;
        return result;
    }

        /// <summary>
    /// Adjusts the character's velocity and kinematic steering output to handle world boundaries.
    /// </summary>
    /// <param name="steering">The current steering output.</param>
    /// <returns>The adjusted kinematic steering output.</returns>
    private KinematicSteeringOutput HandleWorldBoundaries(KinematicSteeringOutput steering)
    {
        // Calculate the future position of the character
        Vector3 futurePosition = character.transform.position + steering.velocity*Time.deltaTime;
    
        // Check and handle the x boundaries
        if (futurePosition.x > 10 || futurePosition.x < -10)
        {
            character.velocity.x = 0;
            steering.velocity.x = 0;
        }
    
        // Check and handle the y boundaries
        if (futurePosition.y > 5 || futurePosition.y < -5)
        {
            character.velocity.y = 0;
            steering.velocity.y = 0;
        }
    
        return steering;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the character's steering based on the steering output
        character.UpdateKinematic(HandleWorldBoundaries(GetSteering()));
    }
}