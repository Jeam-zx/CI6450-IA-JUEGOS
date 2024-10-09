using UnityEngine;

public interface IPath
{
    /// <summary>
    /// Gets the parameter on the path closest to the given position.
    /// </summary>
    /// <param name="position">The position to find the closest parameter for.</param>
    /// <param name="lastParam">The last parameter value used, for continuity.</param>
    /// <returns>The parameter on the path closest to the given position.</returns>
    float getParam(Vector3 position, float lastParam);

    /// <summary>
    /// Gets the position on the path corresponding to the given parameter.
    /// </summary>
    /// <param name="param">The parameter value on the path.</param>
    /// <returns>The position on the path corresponding to the given parameter.</returns>
    Vector3 getPosition(float param);
}