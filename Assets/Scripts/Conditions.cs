using UnityEngine;

/// <summary>
/// Static class that contains various conditions for the AI decision-making process.
/// </summary>
public static class Conditions
{
    /// <summary>
    /// Checks if there are any enemies nearby within a given threshold distance.
    /// </summary>
    /// <param name="transform">The transform of the AI character.</param>
    /// <param name="threshold">The threshold distance to check for enemies.</param>
    /// <returns>True if there are enemies nearby, otherwise false.</returns>
    public static bool CheckEnemyNearby(Transform transform, float threshold)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < threshold)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if the AI character has a numerical advantage over nearby enemies.
    /// </summary>
    /// <param name="transform">The transform of the AI character.</param>
    /// <param name="threshold">The threshold distance to check for players and enemies.</param>
    /// <returns>True if the AI character has a numerical advantage, otherwise false.</returns>
    public static bool CheckAdvantage(Transform transform, float threshold)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int enemiesNearby = 0;
        int playersNearby = 0;

        // Count nearby enemies
        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < threshold)
            {
                enemiesNearby++;
            }
        }

        // Count nearby players
        foreach (GameObject player in players)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < threshold)
            {
                playersNearby++;
            }
        }

        // Check if the number of nearby enemies is greater than or equal to the number of nearby players
        return enemiesNearby >= playersNearby;
    }

    /// <summary>
    /// Checks if there are any resources available or if there are any targets that have not been reached.
    /// </summary>
    /// <param name="isTargetReached">Array indicating if each target has been reached.</param>
    /// <returns>True if there are resources available or targets that have not been reached, otherwise false.</returns>
    public static bool CheckResources(bool[] isTargetReached)
    {
        if (isTargetReached == null || isTargetReached.Length == 0)
        {
            GameObject[] resources = GameObject.FindGameObjectsWithTag("jar");
            foreach (GameObject res in resources)
            {
                return true;
            }
        }
        else
        {
            foreach (bool rs in isTargetReached)
            {
                if (!rs)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if there are multiple enemies nearby within a given threshold distance.
    /// </summary>
    /// <param name="transform">The transform of the AI character.</param>
    /// <param name="threshold">The threshold distance to check for enemies.</param>
    /// <returns>True if there are multiple enemies nearby, otherwise false.</returns>
    public static bool CheckMultipleEnemies(Transform transform, float threshold)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int enemiesNearby = 0;
        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < threshold)
            {
                enemiesNearby++;
            }
        }
        return enemiesNearby > 1;
    }

    /// <summary>
    /// Checks if any nearby enemies have a higher maximum acceleration than the AI character.
    /// </summary>
    /// <param name="maxAcceleration">The AI character's maxAcceleration.</param>
    /// <returns>True if any nearby enemies have a higher maximum acceleration, otherwise false.</returns>
    public static bool CheckEnemySpeed(float maxAcceleration)
    {
        GameObject enemy = GameObject.Find("Enemy1");

        if (enemy.GetComponent<IAControllerDecisionTree>().maxAcceleration > maxAcceleration)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if there are any players nearby within a given threshold distance.
    /// </summary>
    /// <param name="transform">The transform of the AI character.</param>
    /// <param name="threshold">The threshold distance to check for players.</param>
    /// <returns>True if there are players nearby, otherwise false.</returns>
    public static bool CheckPlayersNearby(Transform transform, float threshold)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < threshold)
            {
                return true;
            }
        }
        return false;
    }
}