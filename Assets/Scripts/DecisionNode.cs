using System;
using UnityEngine;

public class DecisionNode : MonoBehaviour
{
    private DecisionNode trueNode;
    private DecisionNode falseNode;

    public DecisionNode(DecisionNode trueNode, DecisionNode falseNode)
    {
        this.trueNode = trueNode;
        this.falseNode = falseNode;
    }

    public virtual DecisionNode MakeDecision()
    {
        return null;
    }
}