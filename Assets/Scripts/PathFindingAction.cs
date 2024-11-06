using UnityEngine;

public class PathFindingAction : ActionNode
{
    public string targetsName;

    private GameObject target;
    private bool isFindingPath = false;

    private GameObject[] targets;

    private bool[] isTargetReached;

    public float maxAcceleration;

    public float minTime;

    public float maxTime;

    private float timeToWait;

    private bool isWaiting = false;

    private float firstTime;
    
    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag(targetsName);
        isTargetReached = new bool[targets.Length];
        firstTime = Time.time;
    }

    public override void Execute()
    {
        if (isFindingPath)
        {
            if (gameObject.GetComponent<PathFinding>().hasArrived())
            {
                if (!isWaiting)
                {
                    timeToWait = Time.time;
                    isWaiting = true;
                }
                if (Time.time - timeToWait < 2)
                {
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
        // If there are no targets left, return
        if (targets.Length == 0)
        {
            return;
        }
        
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

        gameObject.GetComponent<PathFinding>().target = targets[closestTargetIndex];
        target = targets[closestTargetIndex];
        gameObject.GetComponent<PathFinding>().maxAcceleration = maxAcceleration;
    }

    public override DecisionNode MakeDecision()
    {
        if (gameObject == GameObject.Find("Enemy1")){
            Debug.Log("Time: " + (Time.time - firstTime));
            Debug.Log("Imprimiendo targets");
            for (int i = 0; i < targets.Length; i++)
            {
                Debug.Log(targets[i]);
            }
        }
        if (Time.time - firstTime < minTime || Time.time - firstTime > maxTime)
        {
            gameObject.GetComponent<PathFinding>().target = gameObject;
            return null;
        }
        Execute();
        return this;
    }
}