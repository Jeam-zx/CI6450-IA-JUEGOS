using System.Collections.Generic;
using UnityEngine;

public class Separation : MonoBehaviour
{
    public Kinematic character;
    public float maxAcceleration;
    public List<Kinematic> targets;

    // Separation parameters
    public float threshold;
    public float decayCoefficient;

    /// <summary>
    /// Calculates the steering output for separation behavior.
    /// </summary>
    /// <returns>The dynamic steering output.</returns>
    protected SteeringOutput GetSteering()
    {
        SteeringOutput result = new()
        {
            linear = Vector3.zero
        };

        // Iterate through each target to calculate the separation force
        foreach (var target in targets)
        {
            Vector3 direction = character.transform.position - target.transform.position;
            float distance = direction.magnitude;

            // Check if the target is within the threshold distance
            if (distance < threshold)
            {
                // Calculate the strength of the separation force
                float strength = Mathf.Min(decayCoefficient / (distance * distance), maxAcceleration);
                direction.Normalize();
                result.linear += strength * direction;
            }
        }

        return result;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the character's steering based on the steering output
        character.UpdateSteering(GetSteering(), maxAcceleration);
    }
}