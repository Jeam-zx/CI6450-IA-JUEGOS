using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public Dictionary<int, Node> Nodes { get; private set; } = new Dictionary<int, Node>();

    void Start()
    {
        CalculateNodes();
        CalculateEdges();
        DrawTriangles();

    }

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

    public void DrawTriangles()
    {
        foreach (var nodePair in Nodes)
        {
            Vector3[] vertices = nodePair.Value.Vertices;
            Debug.DrawLine(vertices[0], vertices[1], Color.white);
            Debug.DrawLine(vertices[1], vertices[2], Color.white);
            Debug.DrawLine(vertices[2], vertices[0], Color.white);
        }
    }

    public void DrawEdge(Edge edge)
    {
        Debug.DrawLine(edge.Node1.Center, edge.Node2.Center, Color.red);
    }

    private bool ArePointsOnSameSide(Vector3 point1, Vector3 point2, Vector3 linePoint1, Vector3 linePoint2)
    {
        Vector3 crossProduct1 = Vector3.Cross(linePoint2 - linePoint1, point1 - linePoint1);
        Vector3 crossProduct2 = Vector3.Cross(linePoint2 - linePoint1, point2 - linePoint1);
        return Vector3.Dot(crossProduct1, crossProduct2) >= 0;
    }

    private bool IsPointInTriangle(Vector3 point, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
    {
        return ArePointsOnSameSide(point, vertex1, vertex2, vertex3) &&
               ArePointsOnSameSide(point, vertex2, vertex1, vertex3) &&
               ArePointsOnSameSide(point, vertex3, vertex1, vertex2);
    }

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
}