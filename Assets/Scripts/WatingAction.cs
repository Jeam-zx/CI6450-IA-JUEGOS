using UnityEngine;

public class WaitAction : ActionNode
{

    public float maxTime;
    public float minTime;

    private float firstTime;

    void Start()
    {
        firstTime = Time.time;
        gameObject.GetComponent<PathFinding>().target = gameObject;
    }

    public override void Execute()
    {
        gameObject.GetComponent<PathFinding>().target = gameObject;
        gameObject.GetComponent<Kinematic>().velocity = Vector3.zero;
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