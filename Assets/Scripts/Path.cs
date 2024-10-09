using UnityEngine;
using System.Collections.Generic;

public class Path : IPath
{
    public List<Vector3> waypoints;

    /// <summary>
    /// Initializes a new instance of the Path class with the specified waypoints.
    /// </summary>
    /// <param name="waypoints">The list of waypoints that define the path.</param>
    public Path(List<Vector3> waypoints)
    {
        this.waypoints = waypoints;
    }

    /// <summary>
    /// Gets the parameter on the path closest to the given position.
    /// </summary>
    /// <param name="position">The position to find the closest parameter for.</param>
    /// <param name="lastParam">The last parameter value used, for continuity.</param>
    /// <returns>The parameter on the path closest to the given position.</returns>
    public float getParam(Vector3 position, float lastParam)
    {
        // Get the last segment index and the last segment
        int segmentCount = waypoints.Count - 1;
        int lastSegment = Mathf.FloorToInt(lastParam) % segmentCount;

        // Initialize the closest parameter and distance
        float closestParam = lastParam;
        float closestDistance = float.MaxValue;

        // Search in the segments near the last segment
        for (int i = -1; i <= 1; i++)
        {
            // Get the segment index and the segment start and end points
            int segmentIndex = (lastSegment + i + segmentCount) % segmentCount;
            // Get the segment start and end points
            Vector3 segmentStart = waypoints[segmentIndex];
            Vector3 segmentEnd = waypoints[(segmentIndex + 1) % waypoints.Count];

            // Calculate the distance to the segment
            float distance = DistanceToSegment(position, segmentStart, segmentEnd);

            // Update the closest parameter and distance if necessary
            if (distance < closestDistance)
            {
                // Calculate the parameter on the segment
                closestDistance = distance;
                closestParam = segmentIndex + (distance / Vector3.Distance(segmentStart, segmentEnd));
            }
        }

        return closestParam;
    }

    /// <summary>
    /// Gets the position on the path corresponding to the given parameter.
    /// </summary>
    /// <param name="param">The parameter value on the path.</param>
    /// <returns>The position on the path corresponding to the given parameter.</returns>
    public Vector3 getPosition(float param)
    {
        // Get the segment index and the interpolation parameter
        int segmentIndex = Mathf.FloorToInt(param) % (waypoints.Count - 1);
        float t = param - segmentIndex;

        // Get the segment start and end points
        Vector3 segmentStart = waypoints[segmentIndex];
        Vector3 segmentEnd = waypoints[(segmentIndex + 1) % waypoints.Count];

        // Interpolate between the segment start and end points
        return Vector3.Lerp(segmentStart, segmentEnd, t);
    }

    /// <summary>
    /// Calculates the distance from a point to a segment defined by two points.
    /// </summary>
    /// <param name="point">The point to calculate the distance from.</param>
    /// <param name="segmentStart">The start point of the segment.</param>
    /// <param name="segmentEnd">The end point of the segment.</param>
    /// <returns>The distance from the point to the segment.</returns>
    private float DistanceToSegment(Vector3 point, Vector3 segmentStart, Vector3 segmentEnd)
    {
        // Calculate the segment and the vector to the point
        Vector3 segment = segmentEnd - segmentStart;
        Vector3 toPoint = point - segmentStart;

        // Calculate the projection parameter
        float t = Mathf.Clamp01(Vector3.Dot(toPoint, segment) / segment.sqrMagnitude);
        Vector3 projection = segmentStart + t * segment;

        // Return the distance between the point and its projection
        return Vector3.Distance(point, projection);
    }
}