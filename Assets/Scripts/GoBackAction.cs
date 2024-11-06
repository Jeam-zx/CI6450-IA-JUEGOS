using UnityEngine;

public class GoBackAction : ActionNode
{
    public float maxAcceleration;

    public float minTime;

    public float maxTime;

    private float firstTime;

    void Start()
    {
        firstTime = Time.time;
    }

    public override void Execute()
    {
        gameObject.GetComponent<PathFinding>().target = GameObject.FindGameObjectsWithTag("three")[0];
        gameObject.GetComponent<PathFinding>().maxAcceleration = maxAcceleration;
    }

    public override DecisionNode MakeDecision()
    {
        if  (Time.time - firstTime < minTime || Time.time - firstTime > maxTime)
        {
            return null;
        }
        Execute();
        return this;
    }
}