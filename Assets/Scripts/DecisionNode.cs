/// <summary>
/// Node of the decision tree.
/// </summary>
public class DecisionNode
{
    /// <summary>
    /// The question or decision at this node.
    /// </summary>
    public string Question { get; set; }

    /// <summary>
    /// The branch if the answer is yes.
    /// </summary>
    public DecisionNode Yes { get; set; }

    /// <summary>
    /// The branch if the answer is no.
    /// </summary>
    public DecisionNode No { get; set; }

    /// <summary>
    /// The action to perform if this is a leaf node.
    /// </summary>
    public string Action { get; set; }

    /// <summary>
    /// Indicates if this is a leaf node.
    /// </summary>
    public bool IsLeaf { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DecisionNode"/> class.
    /// </summary>
    /// <param name="question">The question or decision at this node.</param>
    /// <param name="action">The action to perform if this is a leaf node.</param>
    public DecisionNode(string question = "", string action = "")
    {
        Question = question;
        Action = action;
        Yes = null;
        No = null;
        IsLeaf = string.IsNullOrEmpty(question);
    }
}
