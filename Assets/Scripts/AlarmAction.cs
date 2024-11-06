using UnityEngine;

public class AlarmAction : ActionNode
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
        gameObject.GetComponent<PathFinding>().target = target;
        gameObject.GetComponent<PathFinding>().maxAcceleration = maxAcceleration;
    }

    public override DecisionNode MakeDecision()
    {
        if  (Time.time - firstTime < minTime || Time.time - firstTime > maxTime || Vector3.Distance(gameObject.transform.position, target.transform.position) > threshold)
        {
            gameObject.GetComponent<PathFinding>().target = gameObject;
            return null;
        }
        Execute();
        return this;
    }
}