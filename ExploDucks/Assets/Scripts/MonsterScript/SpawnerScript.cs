using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// Controls the spawning of monsters in the game.
/// </summary>
public class SpawnerScript : MonoBehaviour
{

    #region Fields
    [Header("Monsters")]
    /// <summary>
    /// Prefab for the default type of monster.
    /// </summary>
    public GameObject defaultMonster;

    /// <summary>
    /// Prefab for the fast type of monster.
    /// </summary>
    public GameObject fastMonster;

    /// <summary>
    /// Prefab for the smart type of monster.
    /// </summary>
    public GameObject smartMonster;

    /// <summary>
    /// Prefab for the ghost type of monster.
    /// </summary>
    public GameObject ghostMonster;

    [Header("Map")]

    /// <summary>
    /// Reference to the game grid GameObject.
    /// </summary>
    public GameObject grid;

    // TileMaps in the Grid

    /// <summary>
    /// Represents a Tilemap containing indestructible tiles.
    /// </summary>
    public Tilemap map_indestructible;

    /// <summary>
    /// Represents a second Tilemap containing indestructible tiles
    /// </summary>
    public Tilemap map_indestructible2;

    /// <summary>
    /// Represents a Tilemap containing destructible tiles
    /// </summary>
    public Tilemap map_destructible;


    /// <summary>
    /// An array containing references to Tilemaps used in the game grid.
    /// </summary>
    public Tilemap[] tilemaps;

    /// <summary>
    /// An array containing references to player GameObjects in the game.
    /// </summary>
    private GameObject[] players;

    /// <summary>
    /// A HashSet containing free positions within the game grid where monsters can spawn.
    /// </summary>
    private HashSet<Vector3> freePositions;

    /// <summary>
    /// Represents a merged Tilemap
    /// </summary>
    private Tilemap mergedTilemap;

    /// <summary>
    /// A HashSet containing destructible elements on the map, represented as tuples of TileBase and Vector3Int (position).
    /// </summary>
    private HashSet<(TileBase, Vector3Int)> destructibles;

    /// <summary>
    /// A matrix representation of the game grid, indicating the status of each cell (e.g., walkable, obstacle).
    /// </summary>
    private Vector3Int[,] matrixRepres;

    /// <summary>
    /// An array containing references to default type monster GameObjects.
    /// </summary>
    private GameObject[] defaultMonsters;

    /// <summary>
    /// An array containing references to fast type monster GameObjects.
    /// </summary>
    private GameObject[] fastMonsters;

    /// <summary>
    /// An array containing references to ghost type monster GameObjects.
    /// </summary>
    private GameObject[] ghostMonsters;

    /// <summary>
    /// An array containing references to smart type monster GameObjects.
    /// </summary>
    private GameObject[] smartMonsters;

    #endregion

    #region Unity events

    void Start()
    {
        // Gets all the player from the map 
        players = GameObject.FindGameObjectsWithTag("Player");
        // Gets the free positions in the intersect of the maps in the array.
        tilemaps = new Tilemap[] { map_destructible, map_indestructible2};
        freePositions = GetAllFreePositions(tilemaps);
        matrixRepres = RefreshFreePositionsAndMatrix(tilemaps).Item2;

       

        // Megn�zz�k, hogy j�t�k kezdetekor, hol milyen akad�lyok vannak.
        destructibles = GetDestructibles(map_destructible);


        fastMonsters = GameObject.FindGameObjectsWithTag("FastMonsterTag");
        defaultMonsters = GameObject.FindGameObjectsWithTag("DefaultMonsterTag");
        smartMonsters = GameObject.FindGameObjectsWithTag("SmartMonsterTag");
        ghostMonsters = GameObject.FindGameObjectsWithTag("GhostMonster");


    }

    #endregion

    #region Public methods

    /// <summary>
    /// Retrieves the array of Tilemaps used in the game grid.
    /// </summary>
    /// <returns>An array containing references to Tilemaps.</returns>
    public Tilemap[] GetTileMaps()
    { return tilemaps; }

    /// <summary>
    /// Refreshes the list of free positions and matrix representation based on the provided tilemaps.
    /// </summary>
    /// <param name="tilemaps">An array of Tilemaps used in the game grid.</param>
    /// <returns>A tuple containing the intersection of free positions and the matrix representation of the game grid.</returns>
    public (HashSet<Vector3>, Vector3Int[,]) RefreshFreePositionsAndMatrix(Tilemap[] tilemaps)
    {
        // List to store individual sets of free positions
        List<HashSet<Vector3>> freePositionsList = new List<HashSet<Vector3>>();

        // Matrix representation of the game grid
        Vector3Int[,] map_matrix = new Vector3Int[map_destructible.cellBounds.size.x, map_destructible.cellBounds.size.y];

        // Initialize the matrix with default values
        for (int i = 0; i < map_matrix.GetLength(0); i++)
        {
            for (int j = 0; j < map_matrix.GetLength(1); j++)
            {
                map_matrix[i, j] = new Vector3Int(0, 0, -1);
            }
        }

        // Iterate through each provided Tilemap
        foreach (Tilemap tilemap in tilemaps)
        {
            if (tilemap != null)
            {
                // Set to store free positions within the current Tilemap
                HashSet<Vector3> freePositions = new HashSet<Vector3>();

                // Bounds of the Tilemap
                BoundsInt bounds = map_destructible.cellBounds;

                // Origin position of the Tilemap
                Vector3 tilemapOrigin = tilemap.transform.position;


                // Iterate through each cell in the Tilemap
                for (int x = bounds.xMin, i = 0; i < (bounds.size.x); x++, i++)
                {
                    for (int y = bounds.yMin, j = 0; j < (bounds.size.y); y++, j++)
                    {
                        // Check if the current cell is empty (no tile)
                        if (!tilemap.HasTile(new Vector3Int(x, y, 0)))
                        {
                            // Calculate world position of the cell and add it to the free positions
                            Vector3 place = tilemap.CellToWorld(new Vector3Int(x, y, 0)) + tilemapOrigin;
                            freePositions.Add(place);
                            

                            // Update the matrix representation to mark the cell as walkable (1)
                            if (map_matrix[i, j].z == -1)
                            {
                                map_matrix[i, j] = new Vector3Int(x, y, 1);
                            }
                            else if (map_matrix[i, j].z == 0)
                            {
                                continue;
                            }

                        }
                        else
                        {
                            // Update the matrix representation to mark the cell as an obstacle (0)
                            if (map_matrix[i, j].z == -1 || map_matrix[i, j].z == 1)
                            {
                                map_matrix[i, j] = new Vector3Int(x, y, 0);
                            }
                        }

                        if (i == 0 && map_matrix[i, j].z == 1)
                        {
                            //...
                        }

                    }
                }

                // Add the set of free positions to the list
                freePositionsList.Add(freePositions);
            }


        }

        // Find the intersection of all sets of free positions
        HashSet<Vector3> intersection = new HashSet<Vector3>(freePositionsList[0]);

        for (int i = 1; i < freePositionsList.Count; i++)
        {
            intersection.IntersectWith(freePositionsList[i]);
        }

        // Return the intersection and the matrix representation
        return (intersection, map_matrix);
    }

    /// <summary>
    /// Retrieves all free positions within the provided array of Tilemaps.
    /// </summary>
    /// <param name="tilemaps">An array of Tilemaps to search for free positions.</param>
    /// <returns>A HashSet containing all free positions.</returns>
    public HashSet<Vector3> GetAllFreePositions(Tilemap[] tilemaps)
    {
        // List to store individual sets of free positions
        List<HashSet<Vector3>> freePositionsList = new List<HashSet<Vector3>>();

        // Iterate through each provided Tilemap
        foreach (Tilemap tilemap in tilemaps)
        {
            if (tilemap != null)
            {
                // Set to store free positions within the current Tilemap
                HashSet<Vector3> freePositions = new HashSet<Vector3>();

                // Bounds of the Tilemap
                BoundsInt bounds = tilemap.cellBounds;

                // Origin position of the Tilemap
                Vector3 tilemapOrigin = tilemap.transform.position;

                // Iterate through each cell in the Tilemap
                foreach (Vector3Int pos in bounds.allPositionsWithin)
                {

                    if (!tilemap.HasTile(pos))
                    {
                        // No tile at the current position
                        Vector3 bugFix = new Vector3(0.5f, 0.5f, 0);
                        if(gameObject.tag == "GameMap2")
                        {
                            Vector3 place = tilemap.CellToWorld(pos) + tilemapOrigin + bugFix;
                            freePositions.Add(place);
                        } else
                        {
                            Vector3 place = tilemap.CellToWorld(pos) + tilemapOrigin;
                            freePositions.Add(place);
                        }
                        
                    }
                    else
                    {
                        // Tile found at the current position
                    }

                    // Retrieve the tile at the current position
                    TileBase tile = tilemap.GetTile(pos);

                }

                // Add the set of free positions from the current Tilemap to the list
                freePositionsList.Add(freePositions);
            }
        }

        // Calculate the intersection of all free position sets
        HashSet<Vector3> intersection = new HashSet<Vector3>(freePositionsList[0]);

        for (int i = 1; i < freePositionsList.Count; i++)
        {
            intersection.IntersectWith(freePositionsList[i]);
        }


        return intersection;
    }

    /// <summary>
    /// Merges the provided array of Tilemaps into a single Tilemap.
    /// </summary>
    /// <param name="tilemaps">An array of Tilemaps to merge.</param>
    /// <returns>The merged Tilemap containing all elements from the provided Tilemaps.</returns>
    public Tilemap MergeTilemaps(Tilemap[] tilemaps)
    {
        // Create a new Tilemap for merging
        mergedTilemap = new GameObject("MergedTilemap").AddComponent<Tilemap>();
        mergedTilemap.gameObject.AddComponent<TilemapRenderer>();


        // Settings for the newly generated tilemap (Set as a child of the grid, toggle visibility, and set location).
        mergedTilemap.transform.SetParent(grid.transform);
        mergedTilemap.transform.position = map_destructible.transform.position;

        // Disable visibility
        mergedTilemap.GetComponent<TilemapRenderer>().enabled = false;

        mergedTilemap.gameObject.AddComponent<TilemapRenderer>();


        // Iterate through each Tilemap in the provided array
        foreach (Tilemap tilemap in tilemaps)
        {
            if (tilemap != null)
            {
                BoundsInt bounds = tilemap.cellBounds;
                foreach (Vector3Int pos in bounds.allPositionsWithin)
                {
                    TileBase tile = tilemap.GetTile(pos);
                    if (tile != null)
                    {
                        // The current position within the Tilemap is occupied by a tile
                        // Add this position and tile to the merged Tilemap
                        mergedTilemap.SetTile(pos, tile);

                    }
                    else
                    {
                        // The current position within the Tilemap is free
                        // No action needed

                    }
                }
            }
        }

        // The merged Tilemap containing all elements from the provided Tilemaps is now ready

        return mergedTilemap;
    }

    /// <summary>
    /// Spawns a monster of the given type.
    /// </summary>
    /// <param name="monsterPrefab">The prefab of the monster to spawn.</param>
    public void Spawn(GameObject monsterPrefab)
    {
        // Retrieve all existing monsters of different types
        fastMonsters = GameObject.FindGameObjectsWithTag("FastMonsterTag");
        defaultMonsters = GameObject.FindGameObjectsWithTag("DefaultMonsterTag");
        smartMonsters = GameObject.FindGameObjectsWithTag("SmartMonsterTag");
        ghostMonsters = GameObject.FindGameObjectsWithTag("GhostMonster");

        // Create a 2D array to hold all existing monsters
        GameObject[][] monstersSpawned = new GameObject[][]
        {
            fastMonsters,
            defaultMonsters,
            smartMonsters,
            ghostMonsters
        };



        Vector3 randomPosition;
        bool goodDistance;
        bool noMonstersNear;

        // Keep trying to find a suitable position until one is found
        do
        {
            goodDistance = true;

            // Get a random free position
            randomPosition = GetRandomTilePosition(freePositions);

            // Check distance between players and the random position
            foreach (GameObject player in players)
            {
                float distance = Vector3.Distance(player.transform.position, randomPosition);
                if (distance < 6)
                {
                    // If any player is too close, mark the distance as not good
                    goodDistance = false;
                }
            }


            noMonstersNear = true;

            // Check distance between existing monsters and the random position
            foreach (GameObject[] monstertype in monstersSpawned)
            {

                foreach (GameObject currentMonster in monstertype)
                {
                    float distance = Vector3.Distance(currentMonster.transform.position, randomPosition);
                    if (distance < 3)
                    {
                        // If any existing monster is too close, mark the position as not suitable
                        noMonstersNear = false;
                    }
                }


            }

        } while (!goodDistance || !noMonstersNear);

        // Once a suitable position is found, spawn the monster at that position
        SpawnMonsterAtPosition(monsterPrefab, randomPosition);
    }

    /// <summary>
    /// Initializes the set of free positions based on the destructible and indestructible tilemaps.
    /// </summary>
    public void InitFreePositions()
    {
        // Define the tilemaps to consider for free positions
        tilemaps = new Tilemap[] { map_destructible, map_indestructible2 };

        // Get all free positions from the specified tilemaps
        freePositions = GetAllFreePositions(tilemaps);
    }

    /// <summary>
    /// Restores the destructibles on the map.
    /// </summary>
    public void RebuildMap()
    {
        map_destructible.ClearAllTiles();//wipe
        RestoreOriginalDestructible(destructibles);//rebuild
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Helper function used in RebuildMap.
    /// </summary>
    /// <param name="destructibles">Destructible layer of a map.</param>
    void RestoreOriginalDestructible(HashSet<(TileBase, Vector3Int)> destructibles)
    {
        foreach ((TileBase tile, Vector3Int pos) in destructibles)
        {

            Vector3 worldPosition = map_destructible.GetCellCenterWorld(pos);
            map_destructible.SetTile(pos, tile);
        }
    }

    /// <summary>
    /// Retrieves destructible elements from the given Tilemap and returns their positions as a HashSet of tuples containing TileBase and Vector3Int.
    /// </summary>
    /// <param name="tilemap">The Tilemap containing destructible elements.</param>
    /// <returns>A HashSet containing positions and corresponding tiles of destructible elements.</returns>
    private HashSet<(TileBase, Vector3Int)> GetDestructibles(Tilemap tilemap)
    {
        HashSet<(TileBase, Vector3Int)> rockPositions = new HashSet<(TileBase, Vector3Int)>();

        if (tilemap != null)
        {
            BoundsInt bounds = tilemap.cellBounds;

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {

                if (tilemap.HasTile(pos))
                {
                    // We check what type of tile is there, in our case, it can only be a rock.

                    TileBase tile = tilemap.GetTile(pos);

                    // The position here is not a world position but rather a cell position(?). CellToWorld conversion is needed to get the position in the scene.
                    rockPositions.Add((tile, pos));

                }



            }

        }

        return rockPositions;

    }

    /// <summary>
    /// Returns a random position from the provided set of free positions.
    /// </summary>
    /// <param name="freePositions">A HashSet containing free positions.</param>
    /// <returns>A randomly selected position from the set of free positions.</returns>
    Vector3 GetRandomTilePosition(HashSet<Vector3> freePositions)
    {
        // Convert the HashSet to a list to access elements by index
        List<Vector3> list = new List<Vector3>(freePositions);

        // Get the count of available positions in the list
        int count = list.Count;

        // Generate a random index within the range of available positions
        int randomIndex = Random.Range(0, count);

        // Check if there are no available coordinates
        if (count == 0)
        {
            //...
        }

        // Retrieve the position at the randomly selected index
        Vector3 randomPosition = list[randomIndex];

        // Return the randomly selected position
        return randomPosition;

    }

    /// <summary>
    /// Instantiates a monster at a given location.
    /// </summary>
    /// <param name="monsterPrefab">The monster to spawn.</param>
    /// <param name="position">The position to spawn on.</param>
    void SpawnMonsterAtPosition(GameObject monsterPrefab, Vector3 position)
    {
        Instantiate(monsterPrefab, position, Quaternion.identity);
    }

    #endregion


}
