using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// Defines a power-up that grants a temporary speed bonus to the player.
/// </summary>
[CreateAssetMenu(menuName = "PowerUps/SpeedBonus")]
public class SpeedBonus : PowerUp
{
    /// <summary>
    /// The amount by which the player's speed is increased.
    /// </summary>
    public float plusSpeed;

    /// <summary>
    /// The base speed of the player before applying the power-up.
    /// </summary>
    public float speedBase;

    /// <summary>
    /// Grants a temporary speed bonus to the player.
    /// </summary>
    /// <param name="target">The GameObject representing the player to which the power-up effect will be applied.</param>
    public override void Apply(GameObject target)
    {
        // Retrieve current speed
        float speed = target.GetComponent<PlayerController>().speed;

        // Apply speed bonus if not already affected by the same amount
        if (speed != speedBase+plusSpeed && speed != speedBase - plusSpeed)//van e rajta erosites vagy lelassitas
        {
            target.GetComponent<PlayerController>().speed+=plusSpeed;
        }
    }

    /// <summary>
    /// No action needed to remove the power-up effect.
    /// </summary>
    /// <param name="target">The GameObject representing the player from which the power-up effect will be removed.</param>
    public override void Remove(GameObject target) { }

    
}
