using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

/// <summary>
/// Defines a power-up that grants ghost mode to the player.
/// </summary>
[CreateAssetMenu(menuName = "PowerUps/GhostPowerUp")]
public class GhostPowerUp : VisiblePowerUp
{
    /// <summary>
    /// The layers to include for collision detection during ghost mode.
    /// </summary>
    public LayerMask includeLayers;

    /// <summary>
    /// The layers to exclude from collision detection during ghost mode.
    /// </summary>
    public LayerMask excludeLayers;

    /// <summary>
    /// Event invoked when the ghost mode ends.
    /// </summary>
    public UnityEvent onGhostModeOver;

    /// <summary>
    /// The opacity level of the ghost mode.
    /// </summary>
    [HideInInspector]
    public float opacity;

    /// <summary>
    /// Applies the ghost mode effect to the player.
    /// </summary>
    /// <param name="target">The GameObject representing the player to which the power-up effect will be applied.</param>
    public override void Apply(GameObject target)
    {
        // Subscribe to the ghost mode end event
        onGhostModeOver.AddListener(target.GetComponent<PlayerController>().OnGhostModeOver);

        // Adjust Rigidbody2D layers for collision detection
        target.GetComponent<Rigidbody2D>().excludeLayers = excludeLayers;
        target.GetComponent<Rigidbody2D>().includeLayers = includeLayers;

        // Adjust player's appearance to reflect ghost mode
        Color reducedOpacity = target.GetComponent<PlayerController>().spriteRenderer.color;
        reducedOpacity.a = opacity;
        target.GetComponent<PlayerController>().spriteRenderer.color = reducedOpacity;

    }

    /// <summary>
    /// Removes the ghost mode effect from the player.
    /// </summary>
    /// <param name="target">The GameObject representing the player from which the power-up effect will be removed.</param>
    public override void Remove(GameObject target)
    {
        // Reset Rigidbody2D layers for collision detection
        target.GetComponent<Rigidbody2D>().excludeLayers = LayerMask.GetMask("Nothing");
        target.GetComponent<Rigidbody2D>().includeLayers = LayerMask.GetMask("Nothing");

        // Invoke the ghost mode end event
        onGhostModeOver.Invoke();

        // Reset player's appearance to normal
        Color currentColor = target.GetComponent<PlayerController>().spriteRenderer.color;
        currentColor.a = 255f;
        target.GetComponent<PlayerController>().spriteRenderer.color = currentColor;
    }

    /// <summary>
    /// Modifies the player's appearance during ghost mode wear-off animation.
    /// </summary>
    /// <param name="target">The GameObject representing the player.</param>
    /// <param name="secs">The elapsed time since the power up was picked up.</param>
    public override void WearOff(GameObject target, float secs)
    {
        // Round the elapsed seconds for calculation
        float seconds = Mathf.Round(secs*10);

        // Adjust player's appearance during ghost mode wear-off animation
        Color reducedOpacity = target.GetComponent<PlayerController>().spriteRenderer.color;
        Color halfReducedOpacity = reducedOpacity;
        reducedOpacity.a = opacity;
        halfReducedOpacity.a = opacity * 2f;

        // Calculate the opacity based on the elapsed time
        target.GetComponent<PlayerController>().spriteRenderer.color = seconds % 2f == 0 ? halfReducedOpacity : reducedOpacity;

    }
}
