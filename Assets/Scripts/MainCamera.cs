using UnityEngine;

/// <summary>
/// Class that controls the main camera movement.
/// </summary>
public class MainCamera : MonoBehaviour
{
    /// <summary>
    /// The speed at which the camera moves.
    /// </summary>
    public float moveSpeed = 10f;

    /// <summary>
    /// Updates the camera movement each frame.
    /// </summary>
    void Update()
    {
        // Get input from the horizontal and vertical axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate the movement vector
        Vector3 movement = new Vector3(horizontal, vertical, 0) * moveSpeed * Time.deltaTime;

        // Move the camera
        transform.Translate(movement, Space.World);
    }
}