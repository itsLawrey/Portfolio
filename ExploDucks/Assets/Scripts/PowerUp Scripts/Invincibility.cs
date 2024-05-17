using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a power-up that grants invincibility to the player.
/// </summary>
[CreateAssetMenu(menuName = "PowerUps/Invincibility")]
public class Invincibility : VisiblePowerUp
{
    /// <summary>
    /// The color of the player sprite during invincibility.
    /// </summary>
    public Color powerUpColor;

    /// <summary>
    /// The color of the player sprite when invincibility wears off.
    /// </summary>
    public Color wearOffColor;

    /// <summary>
    /// Applies invincibility to the player.
    /// </summary>
    /// <param name="target">The GameObject representing the player to which the power-up effect will be applied.</param>
    public override void Apply(GameObject target)
    {
        // Enable invincibility for the player
        target.GetComponent<PlayerController>().invincibility = true;

        // Adjust appearance to reflect invincibility
        UpdateColors(target);
        target.GetComponent<PlayerController>().spriteRenderer.color = powerUpColor;
    }

    /// <summary>
    /// Removes invincibility from the player.
    /// </summary>
    /// <param name="target">The GameObject representing the player from which the power-up effect will be removed.</param>
    public override void Remove(GameObject target)
    {
        // Disable invincibility for the player
        target.GetComponent<PlayerController>().invincibility = false;

        // Restore original appearance
        Color white = Color.white;
        white.a = target.GetComponent<PlayerController>().spriteRenderer.color.a;

        target.GetComponent<PlayerController>().spriteRenderer.color = white;
    }

    /// <summary>
    /// Modifies the player's appearance during invincibility wear-off animation.
    /// </summary>
    /// <param name="target">The GameObject representing the player.</param>
    /// <param name="secs">The elapsed time since the power up was picked up.</param>
    public override void WearOff(GameObject target, float secs)
    {
        // Update appearance during wear-off animation
        UpdateColors(target);

        float seconds = Mathf.Round(secs*10);
        target.GetComponent<PlayerController>().spriteRenderer.color = seconds % 2f == 0 ? wearOffColor : powerUpColor;
    }

    /// <summary>
    /// Updates the colors used for invincibility and wear-off effects.
    /// </summary>
    /// <param name="target">The GameObject representing the player.</param>
    private void UpdateColors(GameObject target)
    {
        Color currentColor = target.GetComponent<PlayerController>().spriteRenderer.color;
        wearOffColor.a = currentColor.a;
        powerUpColor.a = currentColor.a;
    }

    public void applytest(GameObject target)
    {
        target.GetComponent<PlayerController>().invincibility = true;
    }
    public void removetest(GameObject target)
    {
        target.GetComponent<PlayerController>().invincibility = false;
    }
}
