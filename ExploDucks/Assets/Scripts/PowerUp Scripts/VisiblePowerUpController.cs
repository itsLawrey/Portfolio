using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the behavior of visible power-up objects in the game.
/// </summary>
public class VisiblePowerUpController : MonoBehaviour
{
    /// <summary>
    /// The type of visible power-up associated with this controller.
    /// </summary>
    public VisiblePowerUp powerUpType;

    /// <summary>
    /// The duration of the power-up effect.
    /// </summary>
    public float duration;

    /// <summary>
    /// The SpriteRenderer component responsible for rendering the power-up object.
    /// </summary>
    public SpriteRenderer spriteRenderer;

    /// <summary>
    /// The BoxCollider2D component representing the collision area of the power-up object.
    /// </summary>
    public BoxCollider2D boxCollider;

    /// <summary>
    /// Triggered when another collider enters the collision area of the visible power-up object.
    /// </summary>
    /// <param name="other">The Collider2D representing the other collider.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other collider belongs to the player
        if (other.gameObject.tag == "Player")
        {
            // Disable rendering and collisions of the power-up object
            spriteRenderer.enabled = false;
            boxCollider.enabled = false;

            // Apply the power-up effect to the player
            StartCoroutine(ApplyTimedPowerUp(other.gameObject));
            
        }
    }

    /// <summary>
    /// Applies the power-up effect to the player for a certain duration and calls the powerup's WearOff effect after a certain time.
    /// </summary>
    /// <param name="target">The GameObject representing the player.</param>
    /// <returns>An enumerator representing the coroutine.</returns>
    private IEnumerator ApplyTimedPowerUp(GameObject target)
    {
        // Apply the power-up effect to the player
        powerUpType.Apply(target);

        float elapsedTime = 0f;

        // Wait for the duration of the power-up effect
        while (elapsedTime < duration)
        {
            yield return null; // Wait for one frame

            // If nearing the end of the duration, invoke the WearOff method to modify the power-up effect
            if (elapsedTime > duration - 3f)
            {
                powerUpType.WearOff(target, elapsedTime);
            }
            
            // Remove the power-up effect from the player and destroy the power-up object
            elapsedTime += Time.deltaTime;

        }

        powerUpType.Remove(target);
        Destroy(gameObject);
    }
}
