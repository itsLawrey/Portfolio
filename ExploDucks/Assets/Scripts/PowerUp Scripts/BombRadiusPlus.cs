using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a power-up that increases the explosion radius of bombs.
/// </summary>
[CreateAssetMenu(menuName = "PowerUps/BombRadiusPlus")]
public class BombRadiusPlus : PowerUp
{
    /// <summary>
    /// Increases the explosion radius of the bomb, if it's not already reduced.
    /// </summary>
    /// <param name="target">The GameObject representing the bomb to which the power-up effect will be applied.</param>
    public override void Apply(GameObject target)
    {
        // Check if the bomb's explosion radius is not already reduced
        if (!target.GetComponent<BombController>().isReducedRadius)
        {
            // Increase the explosion radius of the bomb
            target.GetComponent<BombController>().explosionRadius++;
        }
        
    }

    /// <summary>
    /// No action needed to remove the power-up effect.
    /// </summary>
    /// <param name="target">The GameObject representing the bomb from which the power-up effect will be removed.</param>
    public override void Remove(GameObject target) {}
}
