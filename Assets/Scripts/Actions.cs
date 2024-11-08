using UnityEngine;

/// <summary>
/// Static class that contains various actions that the AI can perform.
/// </summary>
public static class Actions
{
    /// <summary>
    /// Makes the AI character attack the closest target.
    /// </summary>
    /// <param name="gameObject">The AI character's game object.</param>
    /// <param name="maxAcceleration">The maximum acceleration for the attack.</param>
    public static void Attack(GameObject gameObject, float maxAcceleration)
    {
        GameObject closestTarget = gameObject;
        string tgt = gameObject.name == "Player1" ? "Enemy" : "Player";
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tgt);
        float minDistance = float.MaxValue;

        // Find the closest target
        foreach (GameObject enemy in targets)
        {
            float distance = Vector3.Distance(gameObject.transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestTarget = enemy;
            }
        }

        // Set the target and max acceleration for pathfinding
        gameObject.GetComponent<PathFinding>().target = closestTarget;
        gameObject.GetComponent<PathFinding>().maxAcceleration = maxAcceleration;

        // Play the attack animation
        SPUM_Prefabs spum = gameObject.GetComponent<SPUM_Prefabs>();
        spum.PlayAnimation(PlayerState.ATTACK, 0);
    }

    /// <summary>
    /// Makes the AI character hide at the nearest hiding spot.
    /// </summary>
    /// <param name="gameObject">The AI character's game object.</param>
    /// <param name="maxAcceleration">The maximum acceleration for hiding.</param>
    public static void Hide(GameObject gameObject, float maxAcceleration)
    {
        SPUM_Prefabs spum = gameObject.GetComponent<SPUM_Prefabs>();
        spum.PlayAnimation(PlayerState.MOVE, 0);

        // Set the target to the nearest hiding spot
        gameObject.GetComponent<PathFinding>().target = GameObject.FindGameObjectsWithTag("hideSpot")[0];
        gameObject.GetComponent<PathFinding>().maxAcceleration = maxAcceleration;
    }

    /// <summary>
    /// Makes the AI character collect resources.
    /// </summary>
    /// <param name="gameObject">The AI character's game object.</param>
    /// <param name="resource">The tag of the resource to collect.</param>
    /// <param name="targets">Array of target game objects.</param>
    /// <param name="isTargetReached">Array indicating if each target has been reached.</param>
    /// <param name="isFindingPath">Indicates if the AI is currently finding a path.</param>
    /// <param name="isWaiting">Indicates if the AI is currently waiting.</param>
    /// <param name="timeToWait">The time to wait before performing an action.</param>
    /// <param name="maxAcceleration">The maximum acceleration for collecting resources.</param>
    public static void Collect(GameObject gameObject, string resource, ref GameObject[] targets, ref bool[] isTargetReached, ref bool isFindingPath, ref bool isWaiting, ref float timeToWait, float maxAcceleration)
    {
        SPUM_Prefabs spum = gameObject.GetComponent<SPUM_Prefabs>();
        spum.PlayAnimation(PlayerState.MOVE, 0);

        // Initialize targets if not already done
        if (targets == null || targets.Length == 0)
        {
            targets = GameObject.FindGameObjectsWithTag(resource);
            isTargetReached = new bool[targets.Length];
        }

        // Check if the AI has arrived at the target
        if (isFindingPath)
        {
            if (gameObject.GetComponent<PathFinding>().hasArrived())
            {
                if (!isWaiting)
                {
                    timeToWait = Time.time;
                    isWaiting = true;
                }

                // Wait for a short duration before proceeding
                if (Time.time - timeToWait < 2)
                {
                    gameObject.GetComponent<SPUM_Prefabs>().PlayAnimation(PlayerState.DEBUFF, 0);
                    return;
                }

                isFindingPath = false;
                for (int i = 0; i < targets.Length; i++)
                {
                    if (targets[i] == gameObject.GetComponent<PathFinding>().target)
                    {
                        isTargetReached[i] = true;
                    }
                }
                isWaiting = false;
            }
            return;
        }

        isFindingPath = true;
        if (targets.Length == 0)
        {
            return;
        }

        // Find the closest target that has not been reached
        float minDistance = float.MaxValue;
        int closestTargetIndex = -1;

        for (int i = 0; i < targets.Length; i++)
        {
            if (!isTargetReached[i])
            {
                float distance = Vector3.Distance(gameObject.transform.position, targets[i].transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTargetIndex = i;
                }
            }
        }

        if (closestTargetIndex == -1)
        {
            return;
        }

        // Set the target and max acceleration for pathfinding
        gameObject.GetComponent<PathFinding>().target = targets[closestTargetIndex];
        gameObject.GetComponent<PathFinding>().maxAcceleration = maxAcceleration;
    }

    /// <summary>
    /// Makes the AI character take damage.
    /// </summary>
    /// <param name="gameObject">The AI character's game object.</param>
    public static void Damage(GameObject gameObject)
    {
        gameObject.GetComponent<PathFinding>().target = gameObject;
        gameObject.GetComponent<PathFinding>().maxAcceleration = 0;

        // Play the damaged animation
        SPUM_Prefabs spum = gameObject.GetComponent<SPUM_Prefabs>();
        spum.PlayAnimation(PlayerState.DAMAGED, 0);
    }

    /// <summary>
    /// Makes the AI character go to the base.
    /// </summary>
    /// <param name="gameObject">The AI character's game object.</param>
    /// <param name="maxAcceleration">The maximum acceleration for going to the base.</param>
    public static void GoToBase(GameObject gameObject, float maxAcceleration)
    {
        SPUM_Prefabs spum = gameObject.GetComponent<SPUM_Prefabs>();
        spum.PlayAnimation(PlayerState.MOVE, 0);

        // Set the target to the base
        GameObject bases = GameObject.FindGameObjectsWithTag("Base")[0];
        gameObject.GetComponent<PathFinding>().target = bases;
        gameObject.GetComponent<PathFinding>().maxAcceleration = maxAcceleration;
    }

    /// <summary>
    /// Makes the AI character call for help.
    /// </summary>
    /// <param name="gameObject">The AI character's game object.</param>
    /// <param name="maxAcceleration">The maximum acceleration for calling for help.</param>
    /// <param name="threshold">The threshold distance to check for players.</param>
    public static void CallForHelp(GameObject gameObject, float maxAcceleration, float threshold)
    {
        SPUM_Prefabs spum = gameObject.GetComponent<SPUM_Prefabs>();
        spum.PlayAnimation(PlayerState.DAMAGED, 0);

        // Find the ally and set the target to the nearest player
        GameObject ally = GameObject.Find("Enemy2");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (Vector3.Distance(gameObject.transform.position, player.transform.position) < threshold)
            {
                ally.GetComponent<PathFinding>().target = player;
                ally.GetComponent<PathFinding>().maxAcceleration = maxAcceleration;
            }
        }
    }

    /// <summary>
    /// Makes the AI character wait.
    /// </summary>
    /// <param name="gameObject">The AI character's game object.</param>
    public static void Wait(GameObject gameObject)
    {
        SPUM_Prefabs spum = gameObject.GetComponent<SPUM_Prefabs>();
        spum.PlayAnimation(PlayerState.IDLE, 0);

        // Set the target to itself and stop moving
        gameObject.GetComponent<PathFinding>().target = gameObject;
        gameObject.GetComponent<PathFinding>().maxAcceleration = 0;
    }

    /// <summary>
    /// Makes the AI character pretend to be dead.
    /// </summary>
    /// <param name="gameObject">The AI character's game object.</param>
    public static void Death(GameObject gameObject)
    {
        gameObject.GetComponent<PathFinding>().target = gameObject;

        // Play the death animation
        SPUM_Prefabs spum = gameObject.GetComponent<SPUM_Prefabs>();
        spum.PlayAnimation(PlayerState.DEATH, 0);
    }
}