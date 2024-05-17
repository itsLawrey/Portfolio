using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a power-up that increases the number of bombs the player can drop.
/// </summary>
[CreateAssetMenu(menuName = "PowerUps/PlusBomb")]
public class PlusBomb : PowerUp
{   
    /// <summary>
    /// Increases the number of bombs the player can drop.
    /// </summary>
    /// <param name="target">The GameObject representing the player to which the power-up effect will be applied.</param>
    public override void Apply(GameObject target)
    {
        target.GetComponent<BombController>().bombAmount++;
        target.GetComponent<BombController>().bombsRemaining++;
    }

    /// <summary>
    /// No action needed to remove the power-up effect.
    /// </summary>
    /// <param name="target">The GameObject representing the player from which the power-up effect will be removed.</param>
    public override void Remove(GameObject target) { }
}
