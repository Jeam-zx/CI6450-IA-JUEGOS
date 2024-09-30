using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kinematic : MonoBehaviour
{
    public Vector3 velocity;
    public float rotation;

    public float orientation;

    /// <summary>
    /// Updates the position, orientation, velocity, and rotation of the kinematic object
    /// based on the provided steering output and the time.
    /// </summary>
    /// <param name="steering">The steering output containing linear and angular accelerations.</param>
    public void UpdateKinematic(SteeringOutput steering)
    {
        // Update the position based on velocity and time.
        transform.position += velocity * Time.deltaTime;
        
        // Update the orientation based on rotation and time.
        orientation += rotation * Time.deltaTime;
    
        // Update the velocity and rotation based on the steering output.
        velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;
    
        // Apply the new orientation to the transform.
        transform.rotation = Quaternion.Euler(0, 0, orientation);
    }

    /// <summary>
    /// Updates the position, orientation, velocity, and rotation of the kinematic object
    /// based on the provided steering output and the maximum allowed speed, and time.
    /// </summary>
    /// <param name="steering">The steering output containing linear and angular vectors.</param>
    /// <param name="maxSpeed">The maximum allowed speed for the object.</param>
    public void UpdateSteeringOutput(SteeringOutput steering, float maxSpeed) {
        // Update the position based on velocity and time.
        transform.position += velocity * Time.deltaTime;
        
        // Update the orientation based on rotation and time.
        orientation += rotation * Time.deltaTime;
    
        // Update the velocity and rotation based on the steering output.
        velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;
    
        // Check if the velocity exceeds the maximum allowed speed and adjust if necessary.
        if (velocity.magnitude > maxSpeed) {
            velocity.Normalize();
            velocity *= maxSpeed;
        }
    }

    public float newOrientation(float current, Vector3 velocity) {
        if (velocity.magnitude > 0) {
            return Mathf.Atan2(-velocity.x, velocity.y) * Mathf.Rad2Deg;
        } else {
            return current;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
