using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : DynamicSeek
{
    // Minimum distance to a wall (how far to avoid collision)
    // Should be greater than the radius of the character
    public float avoidDistance;
    // Distance to look ahead for a collision
    // (The length of the collision ray)
    public float lookahead;

    // Obstacle list
    private List<GameObject> obstacles = new List<GameObject>();

    SteeringOutput GetSteeringAvoid()
    {
        Vector3 ray;
        // 1. Calculate the target to delegate to seek
        // Calculate the collision ray vector.
        ray = character.velocity;
        ray.Normalize();
        ray *= lookahead;

        // Find the collision
        CollisionObstacle collision = getCollision(character.transform.position, ray);

        // If have no collision, do nothing
        if (collision == null)
        {
            SteeringOutput result = new()
            {
                linear = Vector3.zero,
                angular = 0f
            };
            return result;
        }
        Vector3 pos = base.target.transform.position;
        // 2. Otherwise create a target and delegate to seek
        pos = -(collision.position + collision.normal * avoidDistance);
        return GetSteeringWithPosition(pos);

    }

    CollisionObstacle getCollision(Vector3 position, Vector3 moveAmount)
    {
        CollisionObstacle retorno = new CollisionObstacle();
        Vector3 normal = Vector3.zero;
        Vector3 rayPosition = position + moveAmount;
        foreach(GameObject obstacle in obstacles)
        {
            if (rayPosition.x >= (obstacle.transform.position.x - obstacle.transform.localScale.x/2 - 1) &&
            rayPosition.x <= (obstacle.transform.position.x + obstacle.transform.localScale.x/2 + 1) &&
            rayPosition.y >= (obstacle.transform.position.y - obstacle.transform.localScale.y/2 - 1) &&
            rayPosition.y <= (obstacle.transform.position.y + obstacle.transform.localScale.y/2 + 1))
            {
                Debug.Log(obstacle.transform.position);
                normal = obstacle.transform.position - position;
                retorno = new CollisionObstacle(rayPosition, normal);
                return retorno;
            }
        };
        return null;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        obstacles.Clear();
        foreach(GameObject o in GameObject.FindGameObjectsWithTag("obstacle"))
            obstacles.Add(o);

        character.UpdateSteering(GetSteeringAvoid(), maxSpeed);
    }
}