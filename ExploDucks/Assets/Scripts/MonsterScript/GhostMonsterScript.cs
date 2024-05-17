using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;


/// <summary>
/// Script controlling the behavior of the ghost monster in the game.
/// </summary>
public class GhostMonsterScript : MonoBehaviour
{

    #region Fields

    /// <summary>
    /// Script that handles default movement of the monster.
    /// </summary>
    private DefaultMonsterScript defaultMovement;

    /// <summary>
    /// Reference to the logic manager GameObject.
    /// </summary>
    private GameObject logicManager;

    /// <summary>
    /// Reference to the LogicScript component attached to the logic manager GameObject.
    /// </summary>
    private LogicScript logicScript;

    /// <summary>
    /// Array of Tilemap objects representing the tilemaps in the game.
    /// </summary>
    private Tilemap[] tilemaps;

    /// <summary>
    /// Indicates whether the ghost monster can make a decision about its movement.
    /// </summary>
    private bool canMakeDecision = true;

    /// <summary>
    /// The current position of the ghost monster in the grid.
    /// </summary>
    private Vector3Int currentPosition;

    #endregion

    #region Unity events
    /// <summary>
    /// Initializes the script and its associated GameObjects when the game starts.
    /// </summary>
    void Start()
    {
        
        logicManager = GameObject.FindGameObjectWithTag("LogicManager");
        if (logicManager != null)
        {
            logicScript = logicManager.GetComponent<LogicScript>();
        }
        else
        {
            logicScript = null;
        }

        defaultMovement = gameObject.GetComponent<DefaultMonsterScript>();

        // Create an array of Tilemap objects containing the playable area and indestructible area
        tilemaps = new Tilemap[] { logicScript.playableArea, logicScript.indestructableArea };

        defaultMovement.speed = 0.5f;
 
    }

    /// <summary>
    /// FixedUpdate is called at a fixed interval and is often used for physics calculations.
    /// This method updates the position of the ghost monster based on the playable area defined in the logic script.
    /// </summary>
    private void FixedUpdate()
    {
        Vector3Int newPosition = logicScript.playableArea.WorldToCell(gameObject.transform.position);

        if(newPosition != currentPosition)
        {
            currentPosition = newPosition;

            if(IsOccupiedTile(currentPosition))
            {
                GhostMovement();
             
            }

        }


    }

    #endregion

    #region Private methods

    /// <summary>
    /// Handles the movement logic for the ghost character.
    /// </summary>
    private void GhostMovement()
    {
        // Flag to indicate if there is a free path within range.
        bool gotFreePathInRange = false;

        // Number of rocks in the array.
        int rocksInArray = 0;

        // Flag to control whether the ghost should continue its current path.
        bool continuePath = true;

        // Check if there is a free path within the range of default movement direction.
        (gotFreePathInRange, rocksInArray) = CheckAllInRange(defaultMovement.direction);

        if (gotFreePathInRange)
        {
            // If there is a free path in the direction of movement after a wall,
            // then decide whether to continue in that direction with a certain probability.
            // In this case, do not make a new decision through rocksInArray amount of tiles!
            int randomNum = Random.Range(0, 10);

            if (randomNum < 3 && canMakeDecision)
            {
                continuePath = false;
            }

            if(continuePath)
            {
                // Continue in the current direction and prevent making new decisions.
                canMakeDecision = false;

                if(rocksInArray > 0)
                {
                    // If there are rocks in the path, do not make a decision yet.
                    return;
                } else
                {
                    // If there are no rocks in the path, the ghost can make a decision again.
                    canMakeDecision = true;
                }
                
            } else
            {
                // If a new decision is made, move in a random direction.
                defaultMovement.MoveToRandomDirection();
            }    


        }
        else if (!gotFreePathInRange)
        {
            // If there is no free path in the direction of movement, treat it as a collision
            // and move in a new random direction.
            continuePath = false;
            defaultMovement.MoveToRandomDirection();

        }



    }

    /// <summary>
    /// Checks if a given position on any of the tilemaps is occupied by a tile.
    /// </summary>
    /// <param name="position">The position to check for tile occupation.</param>
    /// <returns>True if the position is occupied by a tile; otherwise, false.</returns>
    private bool IsOccupiedTile(Vector3Int position)
    {
        // Initialize a flag to indicate if the tile at the position is occupied.
        bool isOccupiedTile = false;

        // Iterate through each tilemap to check for tile occupation.
        foreach (Tilemap tilemap in tilemaps)
        {
            // Check if the tilemap exists.
            if (tilemap != null)
            {
                // Check if the tilemap has a tile at the given position.
                if (tilemap.HasTile(position))
                {
                    // If a tile is found at the position, mark it as occupied and exit the loop.
                    isOccupiedTile = true;
                    break;
                }
            }
        }

        // Return the flag indicating whether the position is occupied by a tile.
        return isOccupiedTile;

        
    }

    /// <summary>
    /// Checks for walkable paths in the given direction within the playable area.
    /// </summary>
    /// <param name="direction">The direction to check for walkable paths (right, left, up, or down).</param>
    /// <returns>
    /// A tuple containing two values:
    /// - A boolean indicating whether there is a walkable path in the given direction.
    /// - An integer representing the number of consecutive decisions to skip if a walkable path is not found.
    /// </returns>
    private (bool, int) CheckAllInRange(Vector2 direction)
    {
        // Current position of the ghost.
        int x_curr = currentPosition.x;
        int y_curr = currentPosition.y;


        // Get the bounds of the playable area.
        BoundsInt bounds = logicScript.playableArea.cellBounds;

        float minX = bounds.min.x;
        float maxX = bounds.max.x - 1;
        float minY = bounds.min.y;
        float maxY = bounds.max.y - 1;

        // Flag to indicate if a walkable path is found within range.
        bool gotWalkableInRange = false;
        bool isArray = true;
        // Number of consecutive decisions to skip.
        int decisionsToSkip = 0;

        // Check for walkable paths based on the given direction.
        if (direction == Vector2.right)
        {
            // Iterate through the right-hand columns in the same row.
            for (int x = x_curr + 1; x <= maxX; x++)
            {

                // Check if there is a series of obstacles after encountering an obstacle, and determine its length.
                if (!IsOccupiedTile(new Vector3Int(x, y_curr, 0)))
                {
                    // If the tile is not occupied, there is a walkable path.
                    gotWalkableInRange = true;
                    isArray = false;
                    break;

                } else
                {
                    // If the tile is occupied, increment the count of consecutive decisions to skip.
                    if (isArray)
                    {
                        decisionsToSkip++;
                    }
                }
            }
                
        }
        else if(direction == Vector2.up)
        {
            for (int y = y_curr + 1; y <= maxY; y++)
            {
                // Check if there is a series of obstacles after encountering an obstacle, and determine its length.

                if (!IsOccupiedTile(new Vector3Int(x_curr, y, 0)))
                {
                    gotWalkableInRange = true;

                    isArray = false;
                    break;

                }
                else
                {
                    if (isArray)
                    {
                        decisionsToSkip++;
                    }
                }
            }
        }
        else if(direction == Vector2.down)
        {
            for (int y = y_curr - 1; y >= minY; y--)
            {
                // Check if there is a series of obstacles after encountering an obstacle, and determine its length.

                if (!IsOccupiedTile(new Vector3Int(x_curr, y, 0)))
                {
                    gotWalkableInRange = true;

                    isArray = false;
                    break;

                }
                else
                {
                    if (isArray)
                    {
                        decisionsToSkip++;
                    }
                }
            }
        }
        else if(direction == Vector2.left)
        {
            for (int x = x_curr - 1; x >= minX; x--)
            {
                // Check if there is a series of obstacles after encountering an obstacle, and determine its length.
                
                if (!IsOccupiedTile(new Vector3Int(x, y_curr, 0)))
                {
                    gotWalkableInRange = true;

                    isArray = false;
                    break;

                }
                else
                {
                    if (isArray)
                    {
                        decisionsToSkip++;
                    }
                }
            }
        }

        // Return a tuple containing the result of walkable paths check and the count of consecutive decisions to skip.
        return (gotWalkableInRange, decisionsToSkip);
    }

    #endregion

}
