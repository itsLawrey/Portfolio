using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class defining a visible power-up.
/// </summary>
public abstract class VisiblePowerUp : ScriptableObject
{
    /// <summary>
    /// Applies the power-up effect to the target GameObject.
    /// </summary>
    /// <param name="target">The GameObject representing the entity to which the power-up effect will be applied.</param>
    public abstract void Apply(GameObject target);

    /// <summary>
    /// Removes the power-up effect from the target GameObject.
    /// </summary>
    /// <param name="target">The GameObject representing the entity from which the power-up effect will be removed.</param>
    public abstract void Remove(GameObject target);

    /// <summary>
    /// Modifies the power-up effect over time.
    /// </summary>
    /// <param name="target">The GameObject representing the entity affected by the power-up.</param>
    /// <param name="secs">The estimated time of the power-up wear off effect in seconds.</param>
    public abstract void WearOff(GameObject target, float secs);
}
