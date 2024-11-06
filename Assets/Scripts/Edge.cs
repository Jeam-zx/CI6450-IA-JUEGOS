using UnityEngine;

public class Edge
{
    public Node Node1;
    public Node Node2; 
    public float Weight; 

    public Edge(Node node1, Node node2, float weight)
    {
        Node1 = node1;
        Node2 = node2;
        Weight = weight;
    }
}