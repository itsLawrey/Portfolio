using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the behavior of obstacles in the game.
/// </summary>
public class ObstacleController : MonoBehaviour
{
    #region Fields
    /// <summary>
    /// The total number of obstacles.
    /// </summary>
    public int obstacleAmount;

    /// <summary>
    /// The number of remaining obstacles that can be placed.
    /// </summary>
    public int remainingObstacles;

    /// <summary>
    /// The prefab representing the obstacle.
    /// </summary>
    public GameObject obstaclePrefab;

    /// <summary>
    /// The layer mask for detecting bombs.
    /// </summary>
    public LayerMask bombMask;

    /// <summary>
    /// The layer mask for detecting obstacles.
    /// </summary>
    public LayerMask obstacleMask;

    /// <summary>
    /// The layer mask for bombs and obstacles.
    /// </summary>
    private LayerMask combinedMask;

    /// <summary>
    /// List of placed obstacles in the game.
    /// </summary>
    private List<GameObject> placedObstacles = new List<GameObject>();
    #endregion

    #region Unity events
    /// <summary>
    /// Initializes obstacle-related properties.
    /// </summary>
    void Start()
    {
        obstacleAmount = 0;
        remainingObstacles = 0;
        combinedMask = bombMask | obstacleMask;
    }

    #endregion

    #region Public methods
    /// <summary>
    /// Places an obstacle at the current position.
    /// </summary>
    public void OnPlaceObstacle()
    {
        Debug.Log("Meghívjuk");
        CalculateRemainingObstacles();
        if(remainingObstacles > 0)
        {
            //lekerjuk a jatekos pozijat es kerekitjuk, hogy a bomba a racsra illeszkedjen
            Vector2 position = transform.position;
            position.x = Mathf.Round(position.x);
            position.y = Mathf.Round(position.y);

            //ellenorizzuk, hogy foglalt-e a pozicio
            Collider2D overlap = Physics2D.OverlapBox(position, Vector2.one, 0f, combinedMask);
            if (overlap == null)
            {
                GameObject obstacle = Instantiate(obstaclePrefab, position, Quaternion.identity);
                placedObstacles.Add(obstacle);
                CalculateRemainingObstacles();
            }
        }
        
    }

    /// <summary>
    /// Handles obstacle explosion event.
    /// </summary>
    /// <param name="obstacle">The GameObject representing the exploded obstacle.</param>
    public void OnObstacleExplode(GameObject obstacle)
    {
        if(placedObstacles.Contains(obstacle))
        {
            placedObstacles.Remove(obstacle);
            Destroy(obstacle);
            CalculateRemainingObstacles();

        }
    }

    /// <summary>
    /// Resets obstacle-related properties to default values.
    /// </summary>
    public void ResetProperties()
    {
        placedObstacles = null;
        remainingObstacles = 0;
        obstacleAmount = 0;
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Calculates the remaining obstacles.
    /// </summary>
    private void CalculateRemainingObstacles()
    {
        remainingObstacles = obstacleAmount - placedObstacles.Count;
    }

    /// <summary>
    /// Makes the obstacle solid when exiting the trigger zone.
    /// </summary>
    /// <param name="other">The Collider2D representing the other collider.</param>
    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            other.isTrigger = false;
        }
    }

    #endregion
}
