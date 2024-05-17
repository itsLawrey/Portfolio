using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a power-up that enables instant bomb dropping for the player.
/// </summary>
[CreateAssetMenu(menuName = "PowerUps/InstantBomb")]
public class InstantBomb : PowerUp
{
    /// <summary>
    /// Enables instant bomb drop power up for the player.
    /// </summary>
    /// <param name="target">The GameObject representing the player to which the power-up effect will be applied.</param>
    public override void Apply(GameObject target)
    {
        // Enable instant bomb dropping for the player
        target.GetComponent<BombController>().instantBombDrop = true;

    }

    /// <summary>
    /// Disables instant bomb drop power up for the player.
    /// </summary>
    /// <param name="target">The GameObject representing the player from which the power-up effect will be removed.</param>
    public override void Remove(GameObject target)
    {
        // Disable instant bomb dropping for the player
        target.GetComponent<BombController>().instantBombDrop = false;
    }

}
