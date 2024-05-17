using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the behavior and movement of a default monster in the game.
/// </summary>
public class DefaultMonsterScript : MonoBehaviour
{
    #region Fields

    [Header("Movement")]
    
    /// <summary>
    /// Represents the speed of the object.
    /// </summary>
    public float speed;

    /// <summary>
    /// Represents the direction of movement.
    /// </summary>
    public Vector2 direction { get; private set; }

    /// <summary>
    /// Represents the Collider2D component attached to the monster.
    /// </summary>
    private Collider2D myCollider;

    /// <summary>
    /// Represents the SpriteRenderer component attached to the monster.
    /// </summary>
    private SpriteRenderer spriteRenderer;

    [Header("Sprites")]
    /// <summary>
    /// The sprite used when the monster is moving upwards.
    /// </summary>
    public Sprite upSprite;

    /// <summary>
    /// The sprite used when the monster is moving downwards.
    /// </summary>
    public Sprite downSprite;

    /// <summary>
    /// The sprite used when the monster is moving rightwards.
    /// </summary>
    public Sprite rightSprite;

    /// <summary>
    /// The sprite used when the monster is moving leftwards.
    /// </summary>
    public Sprite leftSprite;

    /// <summary>
    /// Reference to the monster GameObject.
    /// </summary>
    private GameObject monster;

    #endregion


    /// <summary>
    /// Initializes the monster's sprite renderer and sets its initial direction.
    /// </summary>
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the initial direction to move leftwards
        direction = new Vector2(-1, 0);

        // Get a reference to the monster GameObject
        monster = gameObject;

        // If this monster is the default type, start repeating random movement
        // Since this movement script serves as the base for other monsters,
        // the random movement function should only be invoked for the base monster,
        // as the other monsters do not require random movement.
        if (gameObject.tag == "DefaultMonsterTag") 
        {
            InvokeRepeating("RandomMovement", 0f, 3f);
        }

    }

    #region Public methods

    

    /// <summary>
    /// Changes the movement direction of the monster.
    /// </summary>
    /// <param name="direction">The direction vector.</param>
    public void changeDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    /// <summary>
    /// Moves the monster in the specified direction.
    /// </summary>
    /// <param name="direction">The direction vector to move the monster.</param>
    public void Move(Vector2 direction)
    {
        direction.Normalize();
        transform.Translate(direction * speed * Time.deltaTime);

    }

    /// <summary>
    /// Actions to take when the monster collides with another object.
    /// </summary>
    /// <param name="collision">The Collision2D data associated with this collision.</param>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with an object tagged as "Explosion"
        if (collision.gameObject.CompareTag("Explosion"))
        {
            Destroy(gameObject);



        }
        // If a player collides with the monster
        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playa = collision.gameObject.GetComponent<PlayerController>();
            
            // Player dies if not already dead and not invincible
            if (!playa.IsDead() && !playa.invincibility)
            {
                playa.Die();
            }

        }

    }

    

    /// <summary>
    /// Actions to take when the monster enters a trigger collider.
    /// </summary>
    /// <param name="other">The Collider2D data associated with this trigger collision.</param>
    public void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the trigger collision is with an object tagged as "Explosion"
        if (other.gameObject.CompareTag("Explosion"))
        {
            // Destroy the monster
            Destroy(gameObject);
            // Cancel the random movement invoked previously
            CancelInvoke("RandomMovement");
        }
    }

    /// <summary>
    /// Uses DestroyImmediate for testing purposes.
    /// </summary>
    /// <param name="other">The object to collide with.</param>
    public void OnTriggerEnter2DTEST(Collider2D other)
    {
        if (other.gameObject.CompareTag("Explosion"))
        {
            DestroyImmediate(gameObject); // Destroy the monster
            speed = 99f;
        }
    }


    /// <summary>
    /// Updates the sprite of the monster based on its direction.
    /// </summary>
    /// <param name="newDirection">The new direction of the monster.</param>
    public void SpriteUpdate(Vector2 newDirection)
    {
        if (spriteRenderer == null)
        {
            {
                return; // Exit the method if the spriteRenderer is null
            }
        }

        // Assign the appropriate sprite based on the new direction
        if (newDirection == Vector2.up)
        {
            spriteRenderer.sprite = upSprite; // Assign the sprite directly
        }
        else if (newDirection == Vector2.down)
        {
            spriteRenderer.sprite = downSprite; // Assign the sprite directly
        }
        else if (newDirection == Vector2.right)
        {
            spriteRenderer.sprite = rightSprite; // Assign the sprite directly
        }
        else if (newDirection == Vector2.left)
        {
            spriteRenderer.sprite = leftSprite; // Assign the sprite directly
        }
    }

    /// <summary>
    /// Moves the monster to a random direction.
    /// </summary>
    public void MoveToRandomDirection()
    {
        direction = RandomDirection();

    }

    /// <summary>
    /// Generates a random direction for the monster to move.
    /// </summary>
    /// <returns>A random direction vector.</returns>
    public Vector2 RandomDirection()
    {
        // Define all possible movement directions
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.right, Vector2.left };

        // Get a random index for selecting a direction
        int randomIndex = Random.Range(0, 4);

        // Get the new direction
        Vector2 newDirection = directions[randomIndex];

        // Check if the new direction is the same as the current direction
        while (newDirection == direction)
        {
            // If it is, generate a new random index and get a new direction
            randomIndex = Random.Range(0, 4);
            newDirection = directions[randomIndex];
        }


        myCollider = GetComponent<Collider2D>();

        float x = newDirection.x;
        float y = newDirection.y;



        myCollider.offset = new Vector2(x * 0.1f, y * 0.1f);

        SpriteUpdate(newDirection);

        //Assets / Sprites / Monsters / BotEnemyRight.png

        // Return the new direction vector
        return newDirection;
    }

    /// <summary>
    /// Initiates random movement for the monster.
    /// </summary>
    public void RandomMovement()
    {
        int randomNum = Random.Range(0, 10);

        if (randomNum < 3)
        {
            direction = RandomDirection();
        }
    }

    /// <summary>
    /// Sets the direction of movement for the monster.
    /// </summary>
    /// <param name="v">The new direction vector.</param>
    public void SetDirection(Vector2 v)
    {
        direction = v;
    }

    /// <summary>
    /// Sets the sprite renderer for the monster.
    /// </summary>
    /// <param name="sr">The sprite renderer component.</param>
    public void SetSpriteRenderer(SpriteRenderer sr)
    {
        spriteRenderer = sr;
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Called every fixed frame-rate frame.
    /// Updates the sprite direction and moves the monster.
    /// </summary>
    private void FixedUpdate()
    {
        SpriteUpdate(direction);
        Move(direction);
    }

    /// <summary>
    /// Actions to take when the monster continues to collide with another object.
    /// </summary>
    /// <param name="collision">The Collision2D data associated with this collision.</param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        // Change direction randomly when colliding with another object
        direction = RandomDirection();
    }
    #endregion
}
