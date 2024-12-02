using UnityEngine;

/// <summary>
/// Enum representing the type of tactical point.
/// </summary>
public enum TacticalPointType
{
    PoisonArea,    // Áreas venenosas que dañan al jugador
    ResourceSpot,  // Puntos donde recolectar recursos
    HidingGrove    // Arboledas donde esconderse
}

/// <summary>
/// Class that represents a tactical point in the game.
/// </summary>
public class TacticalPoint : MonoBehaviour
{
    public Vector3 position;
    public int t;
    public TacticalPointType type;
    private const float visualOffset = -0.1f; // Offset to avoid overlap with nodes
    private const float circleRadius = 0.05f;  // Size of the gizmo

    void OnDrawGizmos()
    {
        // Set color based on type
        Gizmos.color = type switch
        {
            TacticalPointType.PoisonArea => Color.magenta,     // Rojo para áreas venenosas
            TacticalPointType.ResourceSpot => Color.yellow, // Amarillo para recursos
            TacticalPointType.HidingGrove => Color.green,  // Verde para escondites
            _ => Color.white
        };

        // Make inactive points more transparent
        if (t == 0)
        {
            Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.3f);
        }

        // Draw circle at offset position
        Vector3 offsetPosition = new Vector3(position.x + visualOffset, position.y + visualOffset, position.z);
        Gizmos.DrawWireSphere(offsetPosition, circleRadius);
    }
}