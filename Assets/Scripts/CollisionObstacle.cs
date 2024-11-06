using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionObstacle
{
    public Vector3 position;
    public Vector3 normal;

    public CollisionObstacle(Vector3 position, Vector3 normal)
    {
        this.position = position;
        this.normal = normal;
    }
    public CollisionObstacle()
    {
        this.position = Vector3.zero;
        this.normal = Vector3.zero;
    }
}