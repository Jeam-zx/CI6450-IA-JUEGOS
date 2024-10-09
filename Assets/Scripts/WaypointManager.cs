using UnityEngine;
using System.Collections.Generic;

public class WaypointManager : MonoBehaviour
{
    public List<Transform> waypoints = new();

    /// <summary>
    /// Collects all child waypoints and adds them to the waypoints list.
    /// </summary>
    void Awake()
    {
        // Collect all child waypoints
        foreach (Transform child in transform)
        {
            waypoints.Add(child);
        }
    }

    /// <summary>
    /// Gets the positions of all waypoints.
    /// </summary>
    /// <returns>A list of Vector3 positions of all waypoints.</returns>
    public List<Vector3> GetWaypointPositions()
    {
        List<Vector3> positions = new();
        foreach (Transform waypoint in waypoints)
        {
            positions.Add(waypoint.position);
        }
        return positions;
    }
}