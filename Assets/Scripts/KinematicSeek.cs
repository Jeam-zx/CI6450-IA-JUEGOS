using UnityEngine;

public class KinematicSeek : MonoBehaviour
{
    public Kinematic character;
    public Kinematic target;
    public float maxSpeed;

    /// <summary>
    /// Calculates the steering output for seeking the target.
    /// </summary>
    /// <returns>The kinematic steering output.</returns>
    KinematicSteeringOutput GetSteering()
    {
        KinematicSteeringOutput result = new KinematicSteeringOutput();

        // Get the direction to the target.
        result.velocity = target.transform.position - character.transform.position;

        // The velocity is along this direction, at full speed.
        result.velocity = result.velocity.normalized * maxSpeed;

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