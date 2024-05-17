using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;


/// <summary>
/// Controls the behavior of the player character.
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region Fields
    [Header("Layers and tilemaps")]
    public Tilemap destructibles;
    public Tilemap indestructibles;
    public LayerMask obstacleLayer;

    [Header("Movement")]
    public float speed;
    private Vector2 movementInput;

    [Header("Game stats")]
    public int points;
    private bool isDead = false;
    public bool invincibility;

    [Header("Sprites")]
    public SpriteRenderer spriteRenderer;
    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite rightSprite;
    public Sprite leftSprite;

    [Header("Power Ups")]
    public VisiblePowerUp ghost;
    public VisiblePowerUp inv;


    #endregion

    #region Unity events

    /// <summary>
    /// Initializes the player's spriteRenderer component and sets the initial speed.
    /// </summary>
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        speed = 4f;
    }

    #endregion

    #region Public methods
    /// <summary>
    /// Updates the player's movement and sprite direction if the player is alive.
    /// </summary>
    public void FixedUpdate()
    {
        if (!isDead) // Only move if the player is alive
        {
            MovePlayer(movementInput);
            CheckDirection();
        }
    }

    /// <summary>
    /// Handles player movement input.
    /// </summary>
    /// <param name="context">The movement input context.</param>
    public void OnMove(Vector2 context)
    {
        if (!isDead)
        {
            movementInput = context;
        }

    }

    

    /// <summary>
    /// Checks if the player is dead.
    /// </summary>
    /// <returns>True if the player is dead, false otherwise.</returns>
    public bool IsDead()
    {
        return isDead;
    }
    
    /// <summary>
    /// Kills the player.
    /// </summary>
    public void Die()
    {
        isDead = true;
        ResetMovementInput();
    }


    /// <summary>
    /// Handles collision with explosions.
    /// </summary>
    /// <param name="collision">The collision object.</param>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion") && !invincibility)
        {
            Die();
        }

    }


    /// <summary>
    /// Revives the player, resetting properties to default values.
    /// </summary>
    public void Revive()
    {
        isDead = false;
        speed = 4f;
        invincibility = false;
        GetComponent<Rigidbody2D>().excludeLayers = LayerMask.GetMask("Nothing");
        GetComponent<Rigidbody2D>().includeLayers = LayerMask.GetMask("Nothing");
        ghost.Remove(gameObject);
        inv.Remove(gameObject);

    }

    /// <summary>
    /// Handles the end of the ghost mode power-up.
    /// </summary>
    public void OnGhostModeOver()
    {
        Debug.Log("exit event");
        Vector2 position = transform.position;

        Vector3Int cell1 = destructibles.WorldToCell(position);
        Vector3Int cell2 = indestructibles.WorldToCell(position);

        if (Physics2D.OverlapBox(position, Vector2.one, 0f, obstacleLayer) != null || destructibles.GetTile(cell1) != null || indestructibles.GetTile(cell2) != null)
        {
            Die();
        }

    }

    #endregion

    #region Private methods

    /// <summary>
    /// Updates the player sprite based on movement direction.
    /// </summary>
    private void CheckDirection()
    {
        // Get the horizontal and vertical components of the movementInput vector
        float horizontal = movementInput.x;
        float vertical = movementInput.y;

        // Determine the direction based on the sign of the horizontal and vertical components
        if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
        {
            // Horizontal movement is dominant
            if (horizontal > 0)
            {
                spriteRenderer.sprite = rightSprite;
                // Handle moving right
            }
            else if (horizontal < 0)
            {
                spriteRenderer.sprite = leftSprite;
                // Handle moving left
            }
        }
        else
        {
            // Vertical movement is dominant
            if (vertical > 0)
            {
                spriteRenderer.sprite = upSprite;
                // Handle moving up
            }
            else if (vertical < 0)
            {
                spriteRenderer.sprite = downSprite;
                // Handle moving down
            }
        }
    }


    /// <summary>
    /// Move the player object.
    /// </summary>
    /// <param name="direction">The direction to move in.</param>
    private void MovePlayer(Vector2 direction)
    {
        direction.Normalize();
        transform.Translate(direction * speed * Time.deltaTime);

    }

    /// <summary>
    /// Resets the movement input buffer.
    /// </summary>
    private void ResetMovementInput()
    {
        movementInput = Vector2.zero;
    }

    #endregion
}

