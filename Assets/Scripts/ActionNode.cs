using System;
using UnityEngine;

public abstract class ActionNode : DecisionNode
{

    public ActionNode() : base(null, null)
    {   }

    public abstract void Execute();

    public override DecisionNode MakeDecision()
    {
        return null;
    }
}