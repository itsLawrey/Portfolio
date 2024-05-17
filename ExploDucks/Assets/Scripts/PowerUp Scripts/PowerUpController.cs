using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controls the behavior of power-up objects in the game.
/// </summary>
public class PowerUpController : MonoBehaviour
{
    #region Fields
    /// <summary>
    /// The type of power-up associated with this controller.
    /// </summary>
    public PowerUp powerUpType;

    /// <summary>
    /// Determines if the power-up has a timed duration.
    /// </summary>
    public bool isTimed;

    /// <summary>
    /// The duration of the timed power-up if applicable.
    /// </summary>
    public float duration;

    /// <summary>
    /// The SpriteRenderer component of the power-up object.
    /// </summary>
    public SpriteRenderer spriteRenderer;

    /// <summary>
    /// The BoxCollider2D component of the power-up object.
    /// </summary>
    public BoxCollider2D boxCollider;

    #endregion

    /// <summary>
    /// Triggered when another collider enters the power-up object's trigger zone.
    /// </summary>
    /// <param name="other">The collider that entered the trigger zone.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player
        if (other.gameObject.tag == "Player")
        {
            // Disable rendering and collision of the power-up object
            spriteRenderer.enabled = false;
            boxCollider.enabled = false;

            // Apply the power-up effect
            if (isTimed)
            {
                Debug.Log("Timed power up picked up");
                StartCoroutine(ApplyTimedPowerUp(other.gameObject));
            }
            else
            {
                powerUpType.Apply(other.gameObject);
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Applies the power-up effect for a timed duration.
    /// </summary>
    /// <param name="target">The GameObject representing the target of the power-up effect.</param>
    /// <returns>An IEnumerator to handle the asynchronous execution.</returns>
    private IEnumerator ApplyTimedPowerUp(GameObject target)
    {
        // Apply the power-up effect
        powerUpType.Apply(target);

        // Wait for the specified duration
        yield return new WaitForSeconds(10);

        // Remove the power-up effect and destroy the power-up object
        powerUpType.Remove(target);
        Destroy(gameObject);
    }

    
}
