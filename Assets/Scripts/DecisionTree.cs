using UnityEngine;
using System;

public class DecisionTree : MonoBehaviour
{
    private DecisionNode root;
    

    void Start()
    {
        // Inicializar el árbol
        CreateDecisionTree();
    }

    void CreateDecisionTree()
    {
        if (gameObject.name == "Player1") {
            // Crear el árbol con algunos nodos de ejemplo
            root = new DecisionNode("¿El enemigo está cerca?")
            {
                // Rama SI
                Yes = new DecisionNode("¿Son más de uno?")
                {
                    Yes = new DecisionNode("", "Fingir muerte"),
                    No = new DecisionNode("", "Atacar")
                },

                // Rama NO
                No = new DecisionNode("", "Recolectar cajas")
            };
        }

        if (gameObject.name == "Player2") {
            root = new DecisionNode("¿El enemigo está cerca?")
            {
                Yes = new DecisionNode("¿Es rápido?")
                {
                    Yes = new DecisionNode("", "Daño"),
                    No = new DecisionNode("", "Esconderse")
                },

                No = new DecisionNode("¿Hay recursos?")
                {
                    Yes = new DecisionNode("", "Recolectar jarras"),
                    No = new DecisionNode("", "Ir a la base")
                }
            
            };
        }

        if (gameObject.name == "Enemy1") {
            root = new DecisionNode("¿Los jugadores están cerca?")
            {
                Yes = new DecisionNode("¿Tengo ventaja numérica?")
                {
                    Yes = new DecisionNode("", "Atacar"),
                    No = new DecisionNode("", "Pedir ayuda")
                },

                No = new DecisionNode("", "Esperar")
            };
        }
    }

    // Método para evaluar el árbol
    public string MakeDecision(Func<string, bool> evaluateCondition)
    {
        DecisionNode currentNode = root;

        while (!currentNode.IsLeaf)
        {
            bool decision = evaluateCondition(currentNode.Question);
            currentNode = decision ? currentNode.Yes : currentNode.No;
        }

        return currentNode.Action;
    }

    // Método para agregar una nueva decisión
    public void AddDecision(string[] path, string question, string actionIfYes, string actionIfNo)
    {
        DecisionNode current = root;
        
        // Navegar hasta el nodo donde queremos agregar la decisión
        foreach (string direction in path)
        {
            if (direction.ToLower() == "yes" || direction.ToLower() == "si")
            {
                if (current.Yes == null)
                {
                    Debug.LogError("Camino inválido: no existe el nodo Yes");
                    return;
                }
                current = current.Yes;
            }
            else if (direction.ToLower() == "no")
            {
                if (current.No == null)
                {
                    Debug.LogError("Camino inválido: no existe el nodo No");
                    return;
                }
                current = current.No;
            }
        }

        // Agregar la nueva decisión
        current.Question = question;
        current.IsLeaf = false;
        current.Yes = new DecisionNode("", actionIfYes);
        current.No = new DecisionNode("", actionIfNo);
    }
}