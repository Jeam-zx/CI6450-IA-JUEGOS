using UnityEngine;

public class DecisionTree : MonoBehaviour
{
    public ActionNode[] decisionNodes;

    void Start()
    {
        decisionNodes = new ActionNode[3];
        int i = 0;
        if (gameObject.GetComponent<WaitAction>() != null)
        {
            decisionNodes[i] = gameObject.GetComponent<WaitAction>();
            i++;
        }
        if (gameObject.GetComponent<PathFindingAction>() != null)
        {
            decisionNodes[i] = gameObject.GetComponent<PathFindingAction>();
            i++;
        }
        if (gameObject.GetComponent<HideAction>() != null)
        {
            decisionNodes[i] = gameObject.GetComponent<HideAction>();
            i++;
        }
        if (gameObject.GetComponent<AlarmAction>() != null)
        {
            decisionNodes[i] = gameObject.GetComponent<AlarmAction>();
            i++;
        }
        if (gameObject.GetComponent<GoBackAction>() != null)
        {
            decisionNodes[i] = gameObject.GetComponent<GoBackAction>();
            i++;
        }

    }

    public void Update()
    {
        if (decisionNodes != null)
        {
            foreach (ActionNode node in decisionNodes)
            {
                if (node != null)
                {
                    node.MakeDecision();
                }
            }
        }
    }
}

