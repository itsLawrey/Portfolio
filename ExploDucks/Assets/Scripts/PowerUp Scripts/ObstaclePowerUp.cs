using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a power-up that increases the number of obstacles the player can place.
/// </summary>
[CreateAssetMenu(menuName = "PowerUps/ObstaclePowerUp")]
public class ObstaclePowerUp : PowerUp
{
    /// <summary>
    /// The number of additional obstacles to be added.
    /// </summary>
    public int plusObstacles;

    /// <summary>
    /// Increases the number of obstacles the player can place.
    /// </summary>
    /// <param name="target">The GameObject representing the player to which the power-up effect will be applied.</param>
    public override void Apply(GameObject target)
    {
        target.GetComponent<ObstacleController>().obstacleAmount += plusObstacles;
        
    }

    /// <summary>
    /// No action needed to remove the power-up effect.
    /// </summary>
    /// <param name="target">The GameObject representing the player from which the power-up effect will be removed.</param>
    public override void Remove(GameObject target)
    {
        
    }
}
