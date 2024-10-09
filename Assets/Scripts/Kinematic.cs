using UnityEngine;

public class Kinematic : MonoBehaviour
{
    public Vector3 velocity;
    public float rotation;
    private Vector3 rotationVector = new(0f, 0f, 0f);

    public string text;
    private TextHandler textHandler;

    /// <summary>
    /// Updates the kinematic position and orientation based on the given steering output.
    /// </summary>
    /// <param name="steering">The kinematic steering output.</param>
    public void UpdateKinematic(KinematicSteeringOutput steering)
    {
        // Update the position based on velocity.
        transform.position += steering.velocity * Time.deltaTime;
        
        // Update the rotation based on the steering rotation.
        rotationVector.z = transform.rotation.eulerAngles.z + steering.rotation;
        transform.rotation = Quaternion.Euler(rotationVector);

        // Update the velocity.
        velocity = steering.velocity;
    }

    /// <summary>
    /// Updates the position and orientation based on the given steering output and max speed.
    /// </summary>
    /// <param name="steering">The dynamic steering output.</param>
    /// <param name="maxSpeed">The maximum speed.</param>
    public void UpdateSteering(SteeringOutput steering, float maxSpeed)
    {
        // Update the position based on velocity.
        transform.position += velocity * Time.deltaTime;
        
        // Update the rotation based on the current rotation and time.
        rotationVector.z += rotation * Time.deltaTime * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(rotationVector);

        // Update the velocity and rotation based on the steering output.
        velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;

        // Clamp the velocity to the maximum speed.
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }
    }

    /// <summary>
    /// Updates the angular rotation.
    /// </summary>
    /// <param name="angular">The angular velocity.</param>
    public void UpdateAngular(float angular)
    {
        // Update the rotation based on the current rotation and time.
        rotationVector.z += rotation * Time.deltaTime * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(rotationVector);
        
        // Update the rotation.
        rotation += angular * Time.deltaTime;
    }

    /// <summary>
    /// Sets a new orientation based on the current orientation and velocity.
    /// </summary>
    /// <param name="current">The current orientation.</param>
    /// <param name="velocity">The current velocity.</param>
    public void NewOrientation(float current, Vector3 velocity)
    {
        Vector3 orientation = transform.rotation.eulerAngles;
        
        // Check if there is any velocity.
        if (velocity.magnitude > 0)
        {
            // Calculate orientation from the velocity.
            orientation.z = Mathf.Atan2(-velocity.x, velocity.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(orientation);
        }
        // Otherwise, use the current orientation.
    }

    /// <summary>
    /// Sets the orientation to a specific value.
    /// </summary>
    /// <param name="orientation">The new orientation.</param>
    public void SetOrientation(float orientation)
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.z = orientation;
        transform.rotation = Quaternion.Euler(rotation);
    }

    void Awake()
    {
        // Initialize the TextHandler with the transform and text.
        textHandler = new TextHandler(transform, text);
    }

    void Update()
    {
        // Update the text position and wrap the position around screen edges.
        textHandler.UpdateTextPosition();
        WrapPositionAroundScreenEdges();
    }

    /// <summary>
    /// Wraps the position around the screen edges.
    /// </summary>
    private void WrapPositionAroundScreenEdges()
    {
        // Check if the position is outside the horizontal bounds.
        if (transform.position.x > 12 || transform.position.x < -12)
        {
            transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
        }
        // Check if the position is outside the vertical bounds.
        else if (transform.position.y > 6 || transform.position.y < -6)
        {
            transform.position = new Vector3(transform.position.x, -transform.position.y, transform.position.z);
        }
    }
}