using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Defines a power-up that prevents the player from dropping bombs.
/// </summary>
[CreateAssetMenu(menuName = "PowerUps/NoBomb")]
public class NoBomb : PowerUp
{
    /// <summary>
    /// Prevents the player from dropping bombs.
    /// </summary>
    /// <param name="target">The GameObject representing the player to which the power-up effect will be applied.</param>
    public override void Apply(GameObject target)
    {
        target.GetComponent<BombController>().noBomb = true;

    }

    /// <summary>
    /// Restores the player's ability to drop bombs.
    /// </summary>
    /// <param name="target">The GameObject representing the player from which the power-up effect will be removed.</param>
    public override void Remove(GameObject target)
    {
        // Restore the player's ability to drop bombs
        target.GetComponent<BombController>().noBomb = false;
    }
}
