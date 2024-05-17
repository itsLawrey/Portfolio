using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;


/// <summary>
/// Controls the movement and behavior of a fast monster in the game.
/// </summary>
public class FastMonsterScript : MonoBehaviour
{
    #region Fields

    /// <summary>
    /// Reference to the player target.
    /// </summary>
    public GameObject playerTarget;

    /// <summary>
    /// Array of tilemaps used in the game.
    /// </summary>
    public Tilemap[] tilemaps;

    /// <summary>
    /// Reference to the destination setter component for the monster's pathfinding.
    /// </summary>
    private AIDestinationSetter setter;

    /// <summary>
    /// Script that handles the default movement behavior of the monster.
    /// </summary>
    private DefaultMonsterScript defaultMovement;

    /// <summary>
    /// Reference to the AI path component for the monster's pathfinding.
    /// </summary>
    private AIPath path;

    /// <summary>
    /// Reference to the Collider2D component attached to the monster.
    /// </summary>
    private Collider2D myCollider;

    /// <summary>
    /// Reference to the logic manager GameObject in the scene.
    /// </summary>
    private GameObject logicManager;

    /// <summary>
    /// Reference to the LogicScript component attached to the logic manager GameObject.
    /// </summary>
    private LogicScript logicScript;

    /// <summary>
    /// Indicates whether the target player is reachable by the monster.
    /// </summary>
    private bool _isTargetReachable;

    /// <summary>
    /// The current position of the monster in the game world.
    /// </summary>
    private Vector3Int currentPosition;

    /// <summary>
    /// Reference to the first player GameObject.
    /// </summary>
    private GameObject player1;

    /// <summary>
    /// Reference to the second player GameObject.
    /// </summary>
    private GameObject player2;

    /// <summary>
    /// Array containing references to player GameObjects in the scene.
    /// </summary>
    private GameObject[] players;
    
    /// <summary>
    /// Timer for controlling the interval of certain actions.
    /// </summary>
    private float timer = 0f;

    /// <summary>
    /// The interval at which certain actions are repeated.
    /// </summary>
    private float repeatInterval = 4f;


    #endregion

    #region Unity events
    /// <summary>
    /// Initializes the monster's behavior and movement upon starting the game.
    /// </summary>
    void Start()
    {
        // Initialize...
        players = GameObject.FindGameObjectsWithTag("Player");

        // Assign players to player1 and player2
        player1 = players[1];
        player2 = players[0];

        // Find and assign the logic manager GameObject
        logicManager = GameObject.FindGameObjectWithTag("LogicManager");

        // Get the LogicScript component from the logic manager if it exists
        if (logicManager != null)
        {
            logicScript = logicManager.GetComponent<LogicScript>();
        }
        else
        {
            logicScript = null;
        }

        // Get the required components for movement
        setter = gameObject.GetComponent<AIDestinationSetter>();
        path = gameObject.GetComponent<AIPath>();
        defaultMovement = gameObject.GetComponent<DefaultMonsterScript>();



        // Choose player1 as default destination
        setTarget(player1);
        MoveToTarget();
    }

    /// <summary>
    /// Updates the monster's behavior and movement once per frame.
    /// </summary>
    void Update()
    {
        if(gameObject.CompareTag("SmartMonsterTag"))
        {
            // Update the timer with the time since the last frame
            timer += Time.deltaTime;

            // Check if the timer has reached the repeat interval
            if (timer >= repeatInterval)
            {
                // Call the extendedMovement method and reset the timer
                extendedMovement(logicScript.playableArea.WorldToCell(gameObject.transform.position)); // Call the method when the timer reaches the repeat interval
                timer = 0f;
            }
        }
        

    }

    /// <summary>
    /// Updates the monster's movement and behavior at fixed time intervals.
    /// </summary>
    void FixedUpdate()
    {
        
        _isTargetReachable = CheckIfReachable();

        if (!_isTargetReachable)
        {
            setter.enabled = false;
            defaultMovement.enabled = true;

        }
        else
        {
            path.SearchPath();
            setter.enabled = true;
            defaultMovement.enabled = true;
        }

    }

    #endregion

    #region Public methods

    /// <summary>
    /// Changes the target player of the monster to the specified player in the players array.
    /// </summary>
    /// <param name="n">Index of the player in the players array (1 or 2).</param>
    public void ChangePlayer(int n)
    {
        if (setter != null) // Ensure there are at least two players in the array
        {
            // Toggle between player1 and player2 based on the provided index
            if (n == 1)
            {
                setTarget(player1);
                MoveToTarget();
            }
            else
            {
                setTarget(player2);
                MoveToTarget();
            }
        }
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Called when the monster collides with another collider in 2D.
    /// </summary>
    /// <param name="collision">The Collision2D data associated with this collision.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the target player is reachable
        if (_isTargetReachable)
        {
            Vector2 newDirection = Vector2.down;

            // Search for a path to the target
            path.SearchPath();
            var dir = path.desiredVelocity;
            
            // <
            if (dir.x >= 0 && dir.y >= 0)
            {
                // ++
                if(dir.x < dir.y)
                {
                    // Go up
                    defaultMovement.changeDirection(Vector2.up);
                    newDirection = Vector2.up;

                } else
                {
                    // Go right
                    defaultMovement.changeDirection(Vector2.right);
                    newDirection = Vector2.right;
                }


            } else if(dir.x < 0 && dir.y > 0)
            {
                // -+

                if(Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
                {
                    // Go up
                    defaultMovement.changeDirection(Vector2.up);
                    newDirection = Vector2.up;
                } else
                {
                    // Go left
                    defaultMovement.changeDirection(Vector2.left);
                    newDirection = Vector2.left;
                }

            }
            else if (dir.x < 0 && dir.y < 0)
            {
                // --
                if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
                {
                    // Go down
                    defaultMovement.changeDirection(Vector2.down);
                    newDirection = Vector2.down;
                }
                else
                {
                    // Go left
                    defaultMovement.changeDirection(Vector2.left);
                    newDirection = Vector2.left;
                }

            }
            else if (dir.x > 0 && dir.y < 0)
            {
                // +-

                if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                {
                    // Go right
                    defaultMovement.changeDirection(Vector2.right);
                    newDirection = Vector2.right;
                }
                else
                {
                    // Go down
                    defaultMovement.changeDirection(Vector2.down);
                    newDirection = Vector2.down;
                }

            }
            
            // Adjust the collider offset based on the new direction
            myCollider = GetComponent<Collider2D>();

            float x = newDirection.x;
            float y = newDirection.y;


            myCollider.offset = new Vector2(x * 0.1f, y * 0.1f);

            // Update the sprite direction
            defaultMovement.SpriteUpdate(newDirection);
        }

    }

    /// <summary>
    /// Performs extended movement for the monster based on the player's position.
    /// </summary>
    /// <param name="playerpos">The position of the player.</param>
    private void extendedMovement(Vector3Int playerpos)
    {
        // Get the bounds of the playable area
        BoundsInt bounds = logicScript.playableArea.cellBounds;

        // Define movement directions
        Vector2 direction = defaultMovement.direction;
        Vector2 left = Vector2.left;
        Vector2 right = Vector2.right;
        Vector2 up = Vector2.up;
        Vector2 down = Vector2.down;

        // List to store available directions
        List<Vector2> goodDirections = new List<Vector2>();

        // Extract boundaries
        float minX = bounds.min.x;
        float maxX = bounds.max.x-1;
        float minY = bounds.min.y;
        float maxY = bounds.max.y-1;

        
        int crossCounter = 0;

        // Check player position relative to boundaries
        // and determine available movement directions
        if ((playerpos.x == minX && playerpos.y == minY) || (playerpos.x == minX && playerpos.y == maxY) || (playerpos.x == maxX && playerpos.y == minY) || (playerpos.x == maxX && playerpos.y == maxY))
        {
            //..
        }                        
        else if(playerpos.x == minX)
        {
            if(direction == up)
            {
                if (!logicScript.playableArea.HasTile(new Vector3Int((playerpos.x + 1), playerpos.y, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int((playerpos.x + 1), playerpos.y, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(right);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y + 1, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y + 1, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(up);
                }
            }
            else if (direction == down)
            {
                if (!logicScript.playableArea.HasTile(new Vector3Int((playerpos.x + 1), playerpos.y, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int((playerpos.x + 1), playerpos.y, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(right);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y - 1, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y - 1, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(down);
                }
            }
            else if(direction == right)
            {
                if (!logicScript.playableArea.HasTile(new Vector3Int((playerpos.x + 1), playerpos.y, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int((playerpos.x + 1), playerpos.y, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(right);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y + 1, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y + 1, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(up);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y - 1, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y - 1, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(down);
                }
            }

            

        }
        else if(playerpos.x == maxX)
        {
            if (direction == up)
            {
                if (!logicScript.playableArea.HasTile(new Vector3Int((playerpos.x - 1), playerpos.y, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int((playerpos.x - 1), playerpos.y, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(left);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y + 1, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y + 1, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(up);
                }
            }
            else if (direction == down)
            {
                if (!logicScript.playableArea.HasTile(new Vector3Int((playerpos.x - 1), playerpos.y, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int((playerpos.x - 1), playerpos.y, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(left);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y - 1, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y - 1, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(down);
                }
            }
            else if (direction == left)
            {
                if (!logicScript.playableArea.HasTile(new Vector3Int((playerpos.x - 1), playerpos.y, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int((playerpos.x - 1), playerpos.y, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(left);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y + 1, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y + 1, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(up);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y - 1, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y - 1, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(down);
                }
            }
        }
        else if (playerpos.y == minY)
        {
            if (direction == up)
            {
                if (!logicScript.playableArea.HasTile(new Vector3Int((playerpos.x), playerpos.y + 1, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int((playerpos.x), playerpos.y + 1, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(up);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x - 1, playerpos.y, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x - 1, playerpos.y, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(left);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x + 1, playerpos.y, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x + 1, playerpos.y, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(right);
                }
            }
            else if (direction == left)
            {
                if (!logicScript.playableArea.HasTile(new Vector3Int((playerpos.x), playerpos.y + 1, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int((playerpos.x), playerpos.y + 1, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(up);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x - 1, playerpos.y, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x - 1, playerpos.y, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(left);
                }
            }
            else if (direction == right)
            {
                if (!logicScript.playableArea.HasTile(new Vector3Int((playerpos.x), playerpos.y + 1, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int((playerpos.x), playerpos.y + 1, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(up);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x + 1, playerpos.y, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x + 1, playerpos.y, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(right);
                }
            }
        }
        else if (playerpos.y == maxY)
        {
            if (direction == down)
            {
                if (!logicScript.playableArea.HasTile(new Vector3Int((playerpos.x), playerpos.y - 1, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int((playerpos.x), playerpos.y - 1, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(down);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x - 1, playerpos.y, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x - 1, playerpos.y, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(left);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x + 1, playerpos.y, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x + 1, playerpos.y, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(right);
                }
            }
            else if (direction == left)
            {
                if (!logicScript.playableArea.HasTile(new Vector3Int((playerpos.x), playerpos.y - 1, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int((playerpos.x), playerpos.y - 1, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(down);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x - 1, playerpos.y, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x - 1, playerpos.y, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(left);
                }
            }
            else if (direction == right)
            {
                if (!logicScript.playableArea.HasTile(new Vector3Int((playerpos.x), playerpos.y - 1, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int((playerpos.x), playerpos.y - 1, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(down);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x + 1, playerpos.y, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x + 1, playerpos.y, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(right);
                }
            }
        }
        else
        {
            if(direction == right)
            {
               

                if (!logicScript.playableArea.HasTile(new Vector3Int((playerpos.x + 1), playerpos.y, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int((playerpos.x + 1), playerpos.y, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(right);

                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y + 1, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y + 1, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(up);

                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y - 1, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y - 1, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(down);

                }
            }
            else if (direction == left)
            {
                if (!logicScript.playableArea.HasTile(new Vector3Int((playerpos.x - 1), playerpos.y, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int((playerpos.x - 1), playerpos.y, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(left);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y + 1, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y + 1, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(up);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y - 1, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x, playerpos.y - 1, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(down);
                }
            }
            else if (direction == up)
            {
                if (!logicScript.playableArea.HasTile(new Vector3Int((playerpos.x), playerpos.y + 1, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int((playerpos.x), playerpos.y + 1, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(up);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x + 1, playerpos.y, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x + 1, playerpos.y, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(right);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x - 1, playerpos.y, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x - 1, playerpos.y, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(left);
                }
            }
            else if (direction == down)
            {
                if (!logicScript.playableArea.HasTile(new Vector3Int((playerpos.x), playerpos.y - 1, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int((playerpos.x), playerpos.y - 1, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(down);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x + 1, playerpos.y, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x + 1, playerpos.y, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(right);
                }
                if (!logicScript.playableArea.HasTile(new Vector3Int(playerpos.x - 1, playerpos.y, 0)) && !logicScript.indestructableArea.HasTile(new Vector3Int(playerpos.x - 1, playerpos.y, 0)))
                {
                    crossCounter++;
                    goodDirections.Add(left);
                }
            }

            
        }

        // If there are multiple available directions, choose one randomly
        int chance = Random.Range(0, 10);

        if(crossCounter >= 2)
        {


            int index = Random.Range(0, goodDirections.Count);

            defaultMovement.changeDirection(goodDirections[index]);
            defaultMovement.SpriteUpdate(goodDirections[index]);
            Debug.Log("Been called");

        }



    }

    /// <summary>
    /// Sets the target player for the monster.
    /// </summary>
    /// <param name="player">The GameObject representing the player.</param>
    public void setTarget(GameObject player)
    {
        playerTarget = player;
        
    }

    /// <summary>
    /// Moves the monster towards its target player.
    /// </summary>
    private void MoveToTarget()
    {
        if (setter != null)
        {
            setter.SetTarget(playerTarget);
            setter.targetGameObject = playerTarget;

        }
        
    }

    /// <summary>
    /// Checks if the monster can reach its target player using pathfinding.
    /// </summary>
    /// <returns>True if reachable, false otherwise.</returns>
    private bool CheckIfReachable()
    {
        // Get positions of the current player and the monster
        Vector3 currentPlayerPosition = playerTarget.transform.position;
        Vector3 selfPosition = gameObject.transform.position;

        // Get the nearest graph nodes to the player and the monster
        GraphNode node1 = AstarPath.active.GetNearest(currentPlayerPosition, NNConstraint.Default).node;
        GraphNode node2 = AstarPath.active.GetNearest(selfPosition, NNConstraint.Default).node;


        // Check if there is a path between the two nodes
        if (PathUtilities.IsPathPossible(node1, node2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion


}
