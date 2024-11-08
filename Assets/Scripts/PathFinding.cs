using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that implements the A* pathfinding algorithm to move a character through a graph.
/// </summary>
public class PathFinding : MonoBehaviour
{
    /// <summary>
    /// The target that the character should move towards.
    /// </summary>
    public GameObject target;

    /// <summary>
    /// The list of nodes representing the calculated path.
    /// </summary>
    public List<Node> path = new();

    /// <summary>
    /// The maximum acceleration of the character.
    /// </summary>
    public float maxAcceleration;

    /// <summary>
    /// The Kinematic component of the character.
    /// </summary>
    public Kinematic character;

    /// <summary>
    /// The graph containing the nodes and edges.
    /// </summary>
    public Graph graph;

    /// <summary>
    /// The name of the character.
    /// </summary>
    public string nameCharacter;

    /// <summary>
    /// This structure is used to keep track of the information we need for each node.
    /// </summary>
    class NodeRecord
    {
        public Node Node { get; set; }
        public NodeRecord Prev { get; set; }
        public float CostSoFar { get; set; }
        public float EstimatedTotalCost { get; set; }
    }

    /// <summary>
    /// Implements the A* pathfinding algorithm to find the path from the start node to the goal node.
    /// </summary>
    /// <param name="start">The start node.</param>
    /// <param name="goal">The goal node.</param>
    /// <returns>A list of nodes representing the calculated path.</returns>
    public List<Node> PathFindAStar(Node start, Node goal)
    {
        if (goal == null)
            return new List<Node>();

        // Initialize the record for the start node.
        NodeRecord startRecord = new()
        {
            Node = start,
            Prev = null,
            CostSoFar = 0
        };

        // Initialize the open and closed lists.
        List<NodeRecord> open = new() { startRecord };
        List<NodeRecord> closed = new();

        NodeRecord current = null;

        // Iterate through processing each node.
        while (open.Count > 0)
        {
            // Find the smallest element in the open list (using the estimatedTotalCost)
            current = open[0];
            foreach (NodeRecord n in open)
            {
                if (n.EstimatedTotalCost < current.EstimatedTotalCost)
                    current = n;
            }

            // If it is the goal node, then terminate.
            if (current.Node.Id == goal.Id)
                break;

            // Otherwise get its outgoing connections.
            List<Edge> connections = current.Node.Neighbors;

            // Loop through each connection in turn.
            foreach (Edge connection in connections)
            {
                Node endNode = connection.Node1 == current.Node ? connection.Node2 : connection.Node1;
                float endNodeCost = current.CostSoFar + connection.Weight;

                NodeRecord endNodeRecord = closed.Find(n => n.Node == endNode) ?? open.Find(n => n.Node == endNode);
                float endNodeHeuristic = endNodeRecord != null ? endNodeRecord.EstimatedTotalCost - endNodeRecord.CostSoFar : Vector3.Distance(endNode.Center, goal.Center);

                if (endNodeRecord != null)
                {
                    if (endNodeRecord.CostSoFar <= endNodeCost)
                        continue;

                    if (closed.Contains(endNodeRecord))
                        closed.Remove(endNodeRecord);
                }
                else
                {
                    endNodeRecord = new NodeRecord { Node = endNode };
                }

                endNodeRecord.CostSoFar = endNodeCost;
                endNodeRecord.Prev = current;
                endNodeRecord.EstimatedTotalCost = endNodeCost + endNodeHeuristic;

                if (!open.Contains(endNodeRecord))
                    open.Add(endNodeRecord);
            }

            open.Remove(current);
            closed.Add(current);
        }

        if (current.Node != goal)
            return null;

        List<Node> path = new List<Node>();
        while (current.Node.Id != start.Id)
        {
            path.Insert(0, current.Node);
            current = current.Prev;
        }

        return path;
    }

    /// <summary>
    /// Calculates the steering output to move towards the goal node.
    /// </summary>
    /// <param name="goal">The goal node.</param>
    /// <returns>The calculated steering output.</returns>
    protected SteeringOutput GetSteering(Node goal)
    {
        SteeringOutput result = new()
        {
            linear = goal.Center - character.transform.position,
            angular = 0
        };

        result.linear.Normalize();
        result.linear *= maxAcceleration;
        result.linear = new Vector3(result.linear.x, result.linear.y, 0);

        return result;
    }

    /// <summary>
    /// Checks if the character has arrived at the target.
    /// </summary>
    /// <returns>True if the character has arrived at the target, otherwise False.</returns>
    public bool hasArrived()
    {
        return Vector3.Distance(character.transform.position, target.transform.position) < 1.5f;
    }

    /// <summary>
    /// Update method called once per frame.
    /// </summary>
    void Update()
    {
        Node startNode = graph.GetNodeContainingPoint(character.transform.position);
        Node targetNode = graph.GetNodeContainingPoint(target.transform.position);

        startNode ??= graph.GetClosestNode(character.transform.position);

        if (startNode != null && targetNode != null)
        {
            path = PathFindAStar(startNode, targetNode);

            if (path != null && path.Count > 1)
            {
                for (int i = 1; i < path.Count; i++)
                {
                    if (nameCharacter == "Player1") {
                        Debug.DrawLine(path[i - 1].Center, path[i].Center, Color.red);
                    } else if (nameCharacter == "Player2") {
                        Debug.DrawLine(path[i - 1].Center, path[i].Center, Color.blue);
                    } else {
                        Debug.DrawLine(path[i - 1].Center, path[i].Center, Color.green);
                    }
                }

                if (path.Count > 0)
                {
                    character.UpdateSteering(GetSteering(path[0]), maxAcceleration / 2);
                    if (character.velocity.x > 0) {
                        gameObject.transform.localScale = new Vector3(-1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                    } else {
                        gameObject.transform.localScale = new Vector3(1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                    }
                }
            }
        }
    }
}