using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base abstract class for defining power-up behavior.
/// </summary>
public abstract class PowerUp : ScriptableObject
{
    /// <summary>
    /// Applies the effects of the power-up to the specified target GameObject.
    /// </summary>
    /// <param name="target">The GameObject to which the power-up effects will be applied.</param>
    public abstract void Apply(GameObject target);

    /// <summary>
    /// Removes the effects of the power-up from the specified target GameObject.
    /// </summary>
    /// <param name="target">The GameObject from which the power-up effects will be removed.</param>
    public abstract void Remove(GameObject target);
}
