using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Defines a power-up that slows down the player's movement speed.
/// </summary>
[CreateAssetMenu(menuName = "PowerUps/SlowPowerUp")]
public class SlowPowerUp : PowerUp
{
    /// <summary>
    /// The amount by which the player's speed is reduced.
    /// </summary>
    public float minusSpeed;

    /// <summary>
    /// The base speed of the player before applying the power-up.
    /// </summary>
    public float speedBase;

    /// <summary>
    /// Slows down the player's movement speed.
    /// </summary>
    /// <param name="target">The GameObject representing the player to which the power-up effect will be applied.</param>
    public override void Apply(GameObject target)
    {
        // Retrieve current speed
        float speed = target.GetComponent<PlayerController>().speed;

        // Apply speed reduction if not already reduced by the same amount
        if (speed != speedBase - minusSpeed)
        {
            target.GetComponent<PlayerController>().speed -= minusSpeed;
        }
    }

    /// <summary>
    /// Restores the player's movement speed to its base value.
    /// </summary>
    /// <param name="target">The GameObject representing the player from which the power-up effect will be removed.</param>
    public override void Remove(GameObject target)
    {
        target.GetComponent<PlayerController>().speed = speedBase;
    }

    
    
    
    public void ApplyTest(GameObject playa)
    {
        playa.GetComponent<PlayerController>().speed -= minusSpeed;
    }

    public void RemoveTest(GameObject playa)
    {
        playa.GetComponent<PlayerController>().speed = speedBase;
    }

}
