using UnityEngine;

/// <summary>
/// Class that controls the AI using a decision tree.
/// </summary>
public class IAControllerDecisionTree : MonoBehaviour
{
    private DecisionTree decisionTree;
    private bool isFindingPath = false;
    private GameObject[] targets;
    private bool[] isTargetReached;
    public float maxAcceleration;
    private float timeToWait;
    private bool isWaiting = false;
    public float threshold;

    /// <summary>
    /// Initializes the decision tree component.
    /// </summary>
    void Start()
    {
        decisionTree = GetComponent<DecisionTree>();
    }

    /// <summary>
    /// Updates the AI controller each frame.
    /// </summary>
    void Update()
    {
        string action = decisionTree.MakeDecision(EvaluateCondition);
        PerformAction(action);
        if (gameObject.name == "Enemy1")
        {
            AttackAlly();
        }
    }

    /// <summary>
    /// Evaluates the condition based on the given question.
    /// </summary>
    /// <param name="question">The question to evaluate.</param>
    /// <returns>True if the condition is met, otherwise false.</returns>
    private bool EvaluateCondition(string question)
    {
        switch (question)
        {
            case "¿El enemigo está cerca?":
                return Conditions.CheckEnemyNearby(transform, threshold);
            case "¿Tengo ventaja numérica?":
                return Conditions.CheckAdvantage(transform, threshold);
            case "¿Hay recursos?":
                return Conditions.CheckResources(isTargetReached);
            case "¿Son más de uno?":
                return Conditions.CheckMultipleEnemies(transform, threshold);
            case "¿Es rápido?":
                return Conditions.CheckEnemySpeed(maxAcceleration);
            case "¿Los jugadores están cerca?":
                return Conditions.CheckPlayersNearby(transform, threshold);
            default:
                return false;
        }
    }

    /// <summary>
    /// Performs the action based on the given action string.
    /// </summary>
    /// <param name="action">The action to perform.</param>
    private void PerformAction(string action)
    {
        switch (action)
        {
            case "Atacar":
                Actions.Attack(gameObject, maxAcceleration);
                break;
            case "Esconderse":
                Actions.Hide(gameObject, maxAcceleration);
                break;
            case "Recolectar cajas":
                Actions.Collect(gameObject, "box", ref targets, ref isTargetReached, ref isFindingPath, ref isWaiting, ref timeToWait, maxAcceleration);
                break;
            case "Daño":
                Actions.Damage(gameObject);
                break;
            case "Recolectar jarras":
                Actions.Collect(gameObject, "jar", ref targets, ref isTargetReached, ref isFindingPath, ref isWaiting, ref timeToWait, maxAcceleration);
                break;
            case "Ir a la base":
                Actions.GoToBase(gameObject, maxAcceleration);
                break;
            case "Pedir ayuda":
                Actions.CallForHelp(gameObject, maxAcceleration, threshold);
                break;
            case "Esperar":
                Actions.Wait(gameObject);
                break;
            case "Fingir muerte":
                Actions.Death(gameObject);
                break;
        }
    }

    /// <summary>
    /// Makes the ally attack nearby players.
    /// </summary>
    private void AttackAlly()
    {
        GameObject ally = GameObject.Find("Enemy2");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (Vector3.Distance(gameObject.transform.position, player.transform.position) < threshold)
            {
                SPUM_Prefabs spum = ally.GetComponent<SPUM_Prefabs>();
                spum.PlayAnimation(PlayerState.ATTACK, 0);
            }
        }
    }
}