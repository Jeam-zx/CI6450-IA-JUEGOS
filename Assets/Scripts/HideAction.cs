using UnityEngine;

public class HideAction : ActionNode
{

    public GameObject target;

    public float maxAcceleration;

    public float threshold;

    public float minTime;

    public float maxTime;

    private float firstTime;

    void Start()
    {
        firstTime = Time.time;
    }

    public override void Execute()
    {
        gameObject.GetComponent<PathFinding>().target = GameObject.FindGameObjectsWithTag("hideSpot")[0];
        gameObject.GetComponent<PathFinding>().maxAcceleration = maxAcceleration;
    }

    public override DecisionNode MakeDecision()
    {
        if  (Time.time - firstTime < minTime || Time.time - firstTime > maxTime || Vector3.Distance(gameObject.transform.position, target.transform.position) > threshold)
        {
            return null;
        }
        Execute();
        return this;
    }
}