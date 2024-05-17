using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using Pathfinding;
using Unity.VisualScripting;

/// <summary>
/// Serializable class representing an event that carries a Vector2 parameter.
/// </summary>
[System.Serializable]
public class UnityVector2Event : UnityEvent<Vector2> { }

/// <summary>
/// Serializable class representing an event that carries two GameObject parameters, typically used for bomb-related events involving players and bombs.
/// </summary>
[System.Serializable]
public class UnityPlayerBombEvent : UnityEvent<GameObject, GameObject> { }

/// <summary>
/// Serializable class representing an event that carries two GameObject parameters, typically used for bomb-related events involving players and bombs.
/// </summary>
[System.Serializable]
public class UnityObstacleEvent : UnityEvent<GameObject> { }

public class BombController : MonoBehaviour
{
    #region Fields
    [Header("Bomb")]

    /// <summary>
    /// Bombprefab used to instantiate bomb objects.
    /// </summary>
    public GameObject bombPrefab;

    /// <summary>
    /// How much seconds passes before the bomb explodes.
    /// </summary>
    public float bombFuseTime = 1f;

    /// <summary>
    /// How many bombs can be placed overall per player.
    /// </summary>
    public int bombAmount = 1;

    [HideInInspector]
    /// <summary>
    /// Number of bombs left to place.
    /// </summary>
    public int bombsRemaining = 0;

    /// <summary>
    /// List of already placed bombs.
    /// </summary>
    private List<GameObject> placedBombs = new List<GameObject>();

    /// <summary>
    /// Layermask of bombs.
    /// </summary>
    public LayerMask bombMask;

    [Header("Explosion")]

    /// <summary>
    /// Explosionprefab used to instantiate explosion objects.
    /// </summary>
    public Explosion explosionPrefab;

    /// <summary>
    /// Duration of explosion in seconds;
    /// </summary>
    public float explosionDuration = 0.5f;

    /// <summary>
    /// The distance represents the maximum reach of the explosion.
    /// </summary>
    public int explosionRadius;

    /// <summary>
    /// The layermask that collides with explosions.
    /// </summary>
    public LayerMask explosionMask;

    [Header("Power Ups")]

    /// <summary>
    /// Layermask of powerups.
    /// </summary>
    public LayerMask powerUpMask;

    [HideInInspector]
    /// <summary>
    /// True, if the player picked up BombRadiusMinus power up and the power-up effect is still active, false otherwise.
    /// </summary>
    public bool isReducedRadius;

    [HideInInspector]
    /// <summary>
    /// True, if the player picked up NoBomb power up and the power-up effect is still active, false otherwise.
    /// </summary>
    public bool noBomb;

    [HideInInspector]
    /// <summary>
    /// True, if the player picked up Detonator power up and the power-up effect is still active, false otherwise.
    /// </summary>
    public bool detonator;

    [HideInInspector]
    /// <summary>
    /// True, if the player picked up InstantBombDrop power up and the power-up effect is still active, false otherwise.
    /// </summary>
    public bool instantBombDrop;


    [Header("Others")]

    /// <summary>
    /// Tilemap of destructible tiles in the game.
    /// </summary>
    public Tilemap destructibleTiles;

    /// <summary>
    /// Layermask of obstacles.
    /// </summary>
    public LayerMask obstacleMask;

    /// <summary>
    /// Layermask that represents the bombmask and obstaclemask together.
    /// </summary>
    private LayerMask combinedMask;


    #endregion

    #region Events
    /// <summary>
    /// Event invoked when a bomb explodes, providing the position of the explosion.
    /// </summary>
    public UnityVector2Event onBombExplode;
    
    /// <summary>
    /// Event invoked when a specific type of bomb explodes, providing additional information about the player that placed the bomb.
    /// </summary>
    public UnityPlayerBombEvent onSomeBombExplode;

    /// <summary>
    /// Event triggered when there are no remaining bombs and the bomb dropping key is pressed.
    /// </summary>
    public UnityEvent onNoBombsRemaining;
    public UnityObstacleEvent onObstacleExplode;

    #endregion

    #region Public Functions
    /// <summary>
    /// Handles the action of dropping a bomb when bomb dropping key is pressed.
    /// </summary>
    public void OnDrop(float context)
    {
        // Check if the GameObject is active, there are bombs remaining, bomb placement is allowed, and instant bomb drop is not enabled
        if (gameObject.activeSelf && bombsRemaining > 0 && !noBomb && !instantBombDrop)
        {
            // Start a coroutine to place a bomb
            StartCoroutine(PlaceBomb());
        }
        // If there are no bombs remaining and detonator is available, start the detonator
        else if (bombsRemaining == 0 && detonator)
        {
            StartDetonator();
        }
        // If there are no bombs remaining or bomb placement is restricted, invoke the no bombs remaining event
        else if (bombsRemaining == 0 || noBomb)
        {
            onNoBombsRemaining.Invoke();
        }

    }

    /// <summary>
    /// Resets the properties related to bomb management to their default values (powerups, bombamount, radius, placedbombs).
    /// </summary>
    public void ResetProperties()
    {
        bombAmount = 1;
        explosionRadius = 1;
        placedBombs = new List<GameObject>();
        CalculateBombsRemaining();
        detonator = false;
        noBomb = false;
        instantBombDrop = false;
        isReducedRadius = false;

    }

    #endregion

    #region Private Functions
    /// <summary>
    /// Initializes the object by setting up event listeners for bomb explosion and obstacle placement events,
    /// and calculates a combined mask for physics interactions involving bombs and obstacles.
    /// </summary>
    private void Start()
    {
        onBombExplode.AddListener(GameObject.FindGameObjectWithTag("PowerUpSpawner").GetComponent<PowerUpSpawner>().OnBombExploded);
        onNoBombsRemaining.AddListener(gameObject.GetComponent<ObstacleController>().OnPlaceObstacle);

        combinedMask = bombMask | obstacleMask;
    }

    /// <summary>
    /// Called by Unity when the GameObject this script is attached to becomes active or enabled.
    /// Resets the number of bombs remaining to the initial bomb amount.
    /// </summary>
    private void OnEnable()
    {
        bombsRemaining = bombAmount;
    }

    /// <summary>
    /// Executes a coroutine to handle timed events such as bomb placement.
    /// </summary>
    private void FixedUpdate()
    {
        if (instantBombDrop)
        {
            if (bombsRemaining > 0 && !noBomb)
            {
                StartCoroutine(PlaceBomb());
            }
        }
    }

    /// <summary>
    /// Performs asynchronous operations over multiple frames to place a bomb at the player's position.
    /// </summary>
    /// <returns>An IEnumerator to handle the asynchronous execution.</returns>
    private IEnumerator PlaceBomb()
    {
        // Get the player's position and round it to align with the grid
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        // Check if the position is not occupied
        Collider2D overlap = Physics2D.OverlapBox(position, Vector2.one, 0f, combinedMask);

        if (overlap == null)
        {
            // Instantiate the bomb
            GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
            bomb.GetComponent<Bomb>().radius = explosionRadius;
            bomb.GetComponent<Bomb>().player = gameObject;

            // Add the bomb to the list of placed bombs
            placedBombs.Add(bomb);

            // Decrease the number of remaining bombs
            CalculateBombsRemaining();

            // Wait for the bomb fuse time
            yield return new WaitForSeconds(bombFuseTime);

            // Destroy the bomb
            Destroy(bomb);
            placedBombs.Remove(bomb);

            // Start the explosion process
            if (bomb != null)
            {
                StartExplosion(bomb);
            }

            // Recalculate the number of remaining bombs (in case of chain explosion)
            CalculateBombsRemaining();

            // Update the grid graph for pathfinding
            var graphToScan = AstarPath.active.data.gridGraph;
            GraphUpdateObject gr = new GraphUpdateObject(new Bounds(new Vector3(-2, 1, 0), new Vector3(24, 24, 5)));
            AstarPath.active.UpdateGraphs(gr);

        }

    }

    /// <summary>
    /// Handles collision detection when exiting a trigger zone, making the bomb solid again if it is detected as being in the "Bomb" layer.
    /// </summary>
    /// <param name="other">The collider representing the other object that exited the trigger zone.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the collider belongs to an object in the "Bomb" layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            // Make the bomb solid again by setting isTrigger to false
            other.isTrigger = false;
        }
    }

    /// <summary>
    /// Initiates an explosion from a given position in a specified direction with a certain length recursively.
    /// </summary>
    /// <param name="position">The position from which the explosion originates.</param>
    /// <param name="direction">The direction in which the explosion propagates.</param>
    /// <param name="length">The length of the explosion.</param>
    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        // Check if the explosion length is valid
        if (length <= 0)
        {
            return;
        }

        // Move the explosion position in the specified direction
        position += direction;

        // Check if the explosion hits a destructible tile
        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionMask))
        {
            // Destroy the destructible tile and spawn an explosion visual effect
            Vector3Int cell = destructibleTiles.WorldToCell(position);
            TileBase tile = destructibleTiles.GetTile(cell);
            if (tile != null)
            {
                destructibleTiles.SetTile(cell, null);
                Explosion expl = Instantiate(explosionPrefab, position, Quaternion.identity);
                expl.SetActiveRenderer(expl.end);
                expl.SetDirection(direction);
                Destroy(expl.gameObject, explosionDuration);

                // Invoke the bomb explode event
                onBombExplode.Invoke(position);
            }
            return;
        }

        // Check if the explosion hits another bomb
        Collider2D bombNearby = Physics2D.OverlapBox(position, Vector2.one, 0f, bombMask);

        if (bombNearby != null)
        {
            // Invoke the event for exploding another bomb
            onSomeBombExplode.Invoke(bombNearby.gameObject.GetComponent<Bomb>().player, bombNearby.gameObject);
            StartCoroutine(OnBombExplodeManually(bombNearby.gameObject, 0.2f));
            return;
        }

        // Check if the explosion hits an obstacle
        Collider2D obstacle = Physics2D.OverlapBox(position, Vector2.one, 0f, obstacleMask);

        if (obstacle != null)
        {
            // Invoke the obstacle explode event and spawn an explosion visual effect
            onObstacleExplode.Invoke(obstacle.gameObject);

            Explosion expl = Instantiate(explosionPrefab, position, Quaternion.identity);
            expl.SetActiveRenderer(expl.end);
            expl.SetDirection(direction);
            Destroy(expl.gameObject, explosionDuration);

            return;
        }

        // Check if the explosion hits a power-up
        Collider2D powerUpNearby = Physics2D.OverlapBox(position, Vector2.one, 0f, powerUpMask);
        if (powerUpNearby != null)
        {
            // Destroy the power-up
            Destroy(powerUpNearby.gameObject);
        }

        // Spawn an explosion visual effect and recursively call Explode for the next position
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        Destroy(explosion.gameObject, explosionDuration);

        Explode(position, direction, length - 1);
    }

    /// <summary>
    /// Initiates an explosion at the position of a bomb GameObject and propagates it in four directions based on the bomb's explosion radius.
    /// </summary>
    /// <param name="bomb">The bomb GameObject whose position serves as the origin of the explosion.</param>
    private void StartExplosion(GameObject bomb)
    {
        // Get the position and explosion radius of the bomb
        Vector3 position = bomb.transform.position;
        int radius = bomb.GetComponent<Bomb>().radius;

        // Destroy the bomb and remove it from the list of placed bombs
        Destroy(bomb);
        placedBombs.Remove(bomb);

        // Recalculate the number of remaining bombs
        CalculateBombsRemaining();

        // Spawn the explosion visual effect at the bomb's position
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        Destroy(explosion.gameObject, explosionDuration);

        // Propagate the explosion in four directions (up, down, left, right) based on the bomb's explosion radius
        Explode(position, Vector2.up, radius);
        Explode(position, Vector2.down, radius);
        Explode(position, Vector2.left, radius);
        Explode(position, Vector2.right, radius);
    }

    /// <summary>
    /// Delays the initiation of the bomb explosion by a specified duration before triggering the explosion.
    /// </summary>
    /// <param name="bomb">The bomb GameObject whose explosion is delayed.</param>
    /// <param name="duration">The duration to delay the explosion in seconds.</param>
    /// <returns>An IEnumerator to handle the asynchronous execution.</returns>
    private IEnumerator OnBombExplodeManually(GameObject bomb, float duration)
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Initiate the explosion of the bomb after the delay
        StartExplosion(bomb);
    }

    /// <summary>
    /// Initiates the detonation sequence, triggering the explosion of all placed bombs immediately.
    /// </summary>
    private void StartDetonator()
    {
        // Disable detonator functionality (end of power up)
        detonator = false;

        // Trigger the explosion of each placed bomb immediately
        foreach (GameObject bomb in placedBombs)
        {
            StartCoroutine(OnBombExplodeManually(bomb, 0f));
        }

    }

    /// <summary>
    /// Calculates the number of remaining bombs based on the total bomb amount and the number of bombs already placed.
    /// </summary>
    private void CalculateBombsRemaining()
    {
        // Subtract the number of placed bombs from the total bomb amount to determine the remaining bombs
        bombsRemaining = bombAmount - placedBombs.Count;
    }

    #endregion

}



