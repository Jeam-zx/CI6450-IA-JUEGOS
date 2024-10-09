using System.Collections.Generic;
using UnityEngine;

public class FollowPath : DynamicSeek
{
    public WaypointManager waypointManager;
    // Maximum prediction time
    public float pathOffset;

    public float currentParam;

    public float predictTime = 0.1f;

    private Path path;

    /// <summary>
    /// Overrides the GetSteering method from DynamicSeek to calculate the steering output for following a path.
    /// </summary>
    /// <returns>The dynamic steering output.</returns>
    protected override SteeringOutput GetSteering()
    {
        // Predict the future position of the character
        Vector3 futurePosition = character.transform.position + character.velocity * predictTime;

        // Get the current parameter on the path for the future position
        currentParam = path.getParam(futurePosition, currentParam);

        // Calculate the target parameter on the path
        float targetParam = currentParam + pathOffset;

        // Get the target position on the path
        Vector3 targetPosition = path.getPosition(targetParam);

        // Delegate to GetSteeringWithPosition to get the steering output
        return GetSteeringWithPosition(targetPosition);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the waypoint positions from the waypoint manager
        List<Vector3> waypointPositions = waypointManager.GetWaypointPositions();
        
        // Initialize the path with the waypoint positions
        path = new Path(waypointPositions);
        
        // Get the initial parameter on the path for the character's position
        currentParam = path.getParam(character.transform.position, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // Update the character's steering based on the steering output
        character.UpdateSteering(GetSteering(), maxSpeed);
    }
}