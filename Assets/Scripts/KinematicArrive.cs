using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicArrive : MonoBehaviour
{
    public Kinematic character;
    public Kinematic target;
    public float maxSpeed;

    public float radius;

    public float timeToTarget = 0.25f;

    /// <summary>
    /// Calculates the steering output for the kinematic arrive behavior.
    /// </summary>
    /// <returns>
    /// A KinematicSteeringOutput containing the velocity and rotation needed to arrive at the target.
    /// Returns null if the character is within the arrival radius.
    /// </returns>
    public KinematicSteeringOutput getSteering()
    {
        KinematicSteeringOutput result = new KinematicSteeringOutput();
    
        // Calculate the distance to the target.
        result.velocity = target.transform.position - character.transform.position;
    
        // If the character is within the arrival radius, no steering is needed.
        if (result.velocity.magnitude < radius)
        {
            return null;
        }
    
        // Adjust the velocity to take into account the time to target.
        result.velocity /= timeToTarget;
    
        // Ensure the velocity does not exceed the maximum speed.
        if (result.velocity.magnitude > maxSpeed)
        {
            result.velocity.Normalize();
            result.velocity *= maxSpeed;
        }
    
        // Update the character's orientation based on the new velocity.
        character.orientation = character.newOrientation(character.orientation, result.velocity);
    
        // Set the rotation to zero as this is a kinematic behavior.
        result.rotation = 0;
    
        return result;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
