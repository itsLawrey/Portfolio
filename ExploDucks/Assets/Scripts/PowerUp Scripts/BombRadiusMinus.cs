using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Defines a power-up that reduces the explosion radius of bombs.
/// </summary>
[CreateAssetMenu(menuName = "PowerUps/BombRadiusMinus")]
public class BombRadiusMinus : PowerUp
{
    /// <summary>
    /// The original explosion radius of the bomb before the power-up is applied.
    /// </summary>
    [HideInInspector]
    public int explosionRadius;

    /// <summary>
    /// Applies the power-up effect to reduce the explosion radius of the bomb.
    /// </summary>
    /// <param name="target">The GameObject representing the bomb to which the power-up effect will be applied.</param>
    public override void Apply(GameObject target)
    {
        // Store the original explosion radius of the bomb
        explosionRadius = target.GetComponent<BombController>().explosionRadius;
        // Reduce the explosion radius of the bomb to 1
        target.GetComponent<BombController>().explosionRadius = 1;
        // Set a flag indicating that the bomb has a reduced explosion radius
        target.GetComponent<BombController>().isReducedRadius = true;
        
    }

    /// <summary>
    /// Removes the power-up effect by restoring the bomb's original explosion radius.
    /// </summary>
    /// <param name="target">The GameObject representing the bomb from which the power-up effect will be removed.</param>
    public override void Remove(GameObject target)
    {
        // Restore the bomb's original explosion radius
        target.GetComponent<BombController>().explosionRadius = explosionRadius;

        // Reset the flag indicating the bomb's reduced explosion radius
        target.GetComponent<BombController>().isReducedRadius = false;
    }
}
