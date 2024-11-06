using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int Id { get; private set; }
    public Vector3[] Vertices { get; private set; }
    public List<Edge> Neighbors { get; private set; } = new List<Edge>();
    public Vector3 Center { get; private set; }

    public Node(int id, Vector3 vertice1, Vector3 vertice2, Vector3 vertice3)
    {
        Id = id;
        Vertices = new Vector3[] { vertice1, vertice2, vertice3 };
        Center = (vertice1 + vertice2 + vertice3) / 3;
    }

    public void AddEdge(Edge edge)
    {
        Neighbors.Add(edge);
    }

    public void RemoveEdge(Edge edge)
    {
        Neighbors.Remove(edge);
    }
}