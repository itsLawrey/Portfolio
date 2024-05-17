using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a power-up that grants the player the ability to detonate bombs simultaneously.
/// </summary>
[CreateAssetMenu(menuName = "PowerUps/Detonator")]
public class Detonator : PowerUp
{
    /// <summary>
    /// Grants the player the ability to detonate bombs simultaneously.
    /// </summary>
    /// <param name="target">The GameObject representing the player to which the power-up effect will be applied.</param>
    public override void Apply(GameObject target)
    {
        // Enable detonator functionality for the bomb
        target.GetComponent<BombController>().detonator = true;

    }

    /// <summary>
    /// No action needed to remove the power-up effect.
    /// </summary>
    /// <param name="target">The GameObject representing the bomb from which the power-up effect will be removed.</param>
    public override void Remove(GameObject target)
    {
        
    }
}
