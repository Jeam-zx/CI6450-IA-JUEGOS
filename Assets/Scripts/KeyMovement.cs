using UnityEngine;

public class KeyMovement : MonoBehaviour
{
    public float maxSpeed;
    public Kinematic character;
    Vector3 velocity;
    KinematicSteeringOutput result = new KinematicSteeringOutput();

    // Update is called once per frame
    void Update()
    {
        // Get input from the horizontal and vertical axes (e.g., arrow keys or WASD)
        velocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        // Normalize the velocity to ensure consistent speed in all directions
        velocity.Normalize();
        
        // Scale the velocity by the maximum speed
        velocity *= maxSpeed;

        // Check if the Left Control key is pressed to increase speed
        if (Input.GetKey(KeyCode.LeftControl))
            result.velocity = velocity * 1.5f; // Increase speed by 1.5 times
        else
            result.velocity = velocity; // Use normal speed

        result.rotation = 0; // No rotation needed for this movement

        // Update the character's kinematic state based on the steering output
        character.UpdateKinematic(result);

        // Update the character's orientation based on the current velocity
        character.NewOrientation(transform.rotation.eulerAngles.z, velocity);
    }
}