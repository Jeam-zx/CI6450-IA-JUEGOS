using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Class that represents a graph structure with nodes and edges.
/// </summary>
public class Graph : MonoBehaviour
{
    /// <summary>
    /// Dictionary of nodes in the graph, keyed by node ID.
    /// </summary>
    public Dictionary<int, Node> Nodes { get; private set; } = new Dictionary<int, Node>();

    public List<TacticalPoint> tacticalPoints = new();

    [SerializeField] private LayerMask hideSpotLayerMask;

    /// <summary>
    /// Initializes the graph by calculating nodes and edges, and drawing triangles.
    /// </summary>
    void Start()
    {
        CalculateNodes();
        CalculateEdges();
        DrawTriangles();
        InitializeTacticalPoints();
    }

    /// <summary>
    /// Updates the graph each frame by drawing triangles.
    /// </summary>
    void Update()
    {
        DrawTriangles();
    }

    /// <summary>
    /// Calculates the nodes in the graph based on game objects with the "map" tag.
    /// </summary>
    public void CalculateNodes()
    {
        int id = 0;
        foreach (GameObject square in GameObject.FindGameObjectsWithTag("map"))
        {
            Vector3 topLeft = square.transform.position + new Vector3(-square.transform.localScale.x / 2, square.transform.localScale.y / 2, 0);
            Vector3 topRight = square.transform.position + new Vector3(square.transform.localScale.x / 2, square.transform.localScale.y / 2, 0);
            Vector3 bottomRight = square.transform.position + new Vector3(square.transform.localScale.x / 2, -square.transform.localScale.y / 2, 0);
            Vector3 bottomLeft = square.transform.position + new Vector3(-square.transform.localScale.x / 2, -square.transform.localScale.y / 2, 0);

            Node node1 = new(id++, topRight, bottomLeft, topLeft);
            Node node2 = new(id++, topRight, bottomLeft, bottomRight);

            Nodes.Add(node1.Id, node1);
            Nodes.Add(node2.Id, node2);
        }
    }
        
    private void InitializeTacticalPoints()
    {
        // Handle all map squares and their triangles
        foreach (GameObject square in GameObject.FindGameObjectsWithTag("map"))
        {
            Vector3 topLeft = square.transform.position + new Vector3(-square.transform.localScale.x / 2, square.transform.localScale.y / 2, 0);
            Vector3 topRight = square.transform.position + new Vector3(square.transform.localScale.x / 2, square.transform.localScale.y / 2, 0);
            Vector3 bottomRight = square.transform.position + new Vector3(square.transform.localScale.x / 2, -square.transform.localScale.y / 2, 0);
            Vector3 bottomLeft = square.transform.position + new Vector3(-square.transform.localScale.x / 2, -square.transform.localScale.y / 2, 0);

            // Calculate triangle centers
            Vector3 triangle1Center = (topLeft + topRight + bottomLeft) / 3;
            Vector3 triangle2Center = (bottomRight + topRight + bottomLeft) / 3;

            CheckTriangleForTacticalPoints(square, triangle1Center);
            CheckTriangleForTacticalPoints(square, triangle2Center);
        }
    }
    
    private void CheckTriangleForTacticalPoints(GameObject square, Vector3 triangleCenter)
    {
        // Check for HidingGrove points
        foreach (GameObject grove in GameObject.FindGameObjectsWithTag("grove"))
        {
            if (Vector3.Distance(grove.transform.position, triangleCenter) < 0.5f)
            {
                AddTacticalPoint(square, 1, TacticalPointType.HidingGrove, triangleCenter);
            }
        }
    
        // Check for PoisonArea points
        foreach (GameObject mushroom in GameObject.FindGameObjectsWithTag("mushroom"))
        {
            if (Vector3.Distance(mushroom.transform.position, triangleCenter) < 0.5f)
            {
                AddTacticalPoint(square, 1, TacticalPointType.PoisonArea, triangleCenter);
            }
        }
    
        // Check for ResourceSpot points
        foreach (GameObject jar in GameObject.FindGameObjectsWithTag("jar"))
        {
            if (Vector3.Distance(jar.transform.position, triangleCenter) < 0.5f)
            {
                AddTacticalPoint(square, 1, TacticalPointType.ResourceSpot, triangleCenter);
            }
        }
    }
    
    private void AddTacticalPoint(GameObject square, int t, TacticalPointType type, Vector3 triangleCenter)
    {
        var tacticalPoint = square.AddComponent<TacticalPoint>();
        tacticalPoint.t = t;
        tacticalPoint.type = type;
        tacticalPoint.position = triangleCenter;
        tacticalPoints.Add(tacticalPoint);
    }

    /// <summary>
    /// Gets the closest node to a given point.
    /// </summary>
    /// <param name="point">The point to find the closest node to.</param>
    /// <returns>The closest node to the given point.</returns>
    public Node GetClosestNode(Vector3 point)
    {
        Node closestNode = null;
        float minDistance = float.MaxValue;

        foreach (var nodePair in Nodes)
        {
            float distance = Vector3.Distance(nodePair.Value.Center, point);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestNode = nodePair.Value;
            }
        }

        return closestNode;
    }

    /// <summary>
    /// Calculates the edges between nodes in the graph.
    /// </summary>
    public void CalculateEdges()
    {
        foreach (var nodePair1 in Nodes)
        {
            foreach (var nodePair2 in Nodes)
            {
                if (nodePair1.Key == nodePair2.Key)
                    continue;

                int sharedVerticesCount = 0;
                foreach (Vector3 vertex1 in nodePair1.Value.Vertices)
                {
                    foreach (Vector3 vertex2 in nodePair2.Value.Vertices)
                    {
                        if (vertex1 == vertex2)
                            sharedVerticesCount++;
                    }
                }

                if (sharedVerticesCount == 2)
                {
                    Edge edge = new(nodePair1.Value, nodePair2.Value, Vector3.Distance(nodePair1.Value.Center, nodePair2.Value.Center));
                    nodePair1.Value.AddEdge(edge);
                    nodePair2.Value.AddEdge(edge);
                }
            }
        }
    }

    /// <summary>
    /// Draws the triangles representing the nodes in the graph.
    /// </summary>
    public void DrawTriangles()
    {
        foreach (var nodePair in Nodes)
        {
            Vector3[] vertices = nodePair.Value.Vertices;
            Debug.DrawLine(vertices[0], vertices[1], Color.white);
            Debug.DrawLine(vertices[1], vertices[2], Color.white);
            Debug.DrawLine(vertices[2], vertices[0], Color.white);

            Vector3 centroid = nodePair.Value.Center;

            // Draw a circle at the centroid
            DrawCircle(centroid, 0.05f, Color.red);
        }
    }

    /// <summary>
    /// Draws an edge between two nodes.
    /// </summary>
    /// <param name="edge">The edge to draw.</param>
    public void DrawEdge(Edge edge)
    {
        Debug.DrawLine(edge.Node1.Center, edge.Node2.Center, Color.red);
    }

    /// <summary>
    /// Checks if two points are on the same side of a line.
    /// </summary>
    /// <param name="point1">The first point.</param>
    /// <param name="point2">The second point.</param>
    /// <param name="linePoint1">The first point on the line.</param>
    /// <param name="linePoint2">The second point on the line.</param>
    /// <returns>True if the points are on the same side of the line, otherwise false.</returns>
    private bool ArePointsOnSameSide(Vector3 point1, Vector3 point2, Vector3 linePoint1, Vector3 linePoint2)
    {
        Vector3 crossProduct1 = Vector3.Cross(linePoint2 - linePoint1, point1 - linePoint1);
        Vector3 crossProduct2 = Vector3.Cross(linePoint2 - linePoint1, point2 - linePoint1);
        return Vector3.Dot(crossProduct1, crossProduct2) >= 0;
    }

    /// <summary>
    /// Checks if a point is inside a triangle defined by three vertices.
    /// </summary>
    /// <param name="point">The point to check.</param>
    /// <param name="vertex1">The first vertex of the triangle.</param>
    /// <param name="vertex2">The second vertex of the triangle.</param>
    /// <param name="vertex3">The third vertex of the triangle.</param>
    /// <returns>True if the point is inside the triangle, otherwise false.</returns>
    private bool IsPointInTriangle(Vector3 point, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
    {
        return ArePointsOnSameSide(point, vertex1, vertex2, vertex3) &&
               ArePointsOnSameSide(point, vertex2, vertex1, vertex3) &&
               ArePointsOnSameSide(point, vertex3, vertex1, vertex2);
    }

    /// <summary>
    /// Gets the node that contains a given point.
    /// </summary>
    /// <param name="point">The point to check.</param>
    /// <returns>The node that contains the point, or null if no node contains the point.</returns>
    public Node GetNodeContainingPoint(Vector3 point)
    {
        foreach (var nodePair in Nodes)
        {
            Node node = nodePair.Value;
            if (IsPointInTriangle(point, node.Vertices[0], node.Vertices[1], node.Vertices[2]))
                return node;
        }

        return null;
    }

    /// <summary>
    /// Draws a circle at the specified position.
    /// </summary>
    /// <param name="position">The position of the circle.</param>
    /// <param name="radius">The radius of the circle.</param>
    /// <param name="color">The color of the circle.</param>
    private void DrawCircle(Vector3 position, float radius, Color color)
    {
        int segments = 20;
        float angle = 0f;
        float angleStep = 360f / segments;
    
        Vector3 lastPoint = position + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * radius, Mathf.Sin(Mathf.Deg2Rad * angle) * radius, 0f);
    
        for (int i = 1; i <= segments; i++)
        {
            angle += angleStep;
            Vector3 nextPoint = position + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * radius, Mathf.Sin(Mathf.Deg2Rad * angle) * radius, 0f);
            
            // Draw the outer circle
            Debug.DrawLine(lastPoint, nextPoint, color);
            
            // Draw lines from the center to the edge to create a filled effect
            Debug.DrawLine(position, nextPoint, color);
            
            lastPoint = nextPoint;
        }
    }   
}