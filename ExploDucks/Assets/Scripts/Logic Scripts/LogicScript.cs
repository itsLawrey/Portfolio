using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Manages the game logic, including enemy spawning and player interactions.
/// </summary>
public class LogicScript : MonoBehaviour
{
    #region Fields
    [Header("Monsters")]
    /// <summary>
    /// Reference to the spawner GameObject used for spawning monsters.
    /// </summary>
    public GameObject spawner;

    /// <summary>
    /// Reference to the fast monster GameObject.
    /// </summary>
    public GameObject fastMonster;

    /// <summary>
    /// Reference to the default monster GameObject.
    /// </summary>
    public GameObject defaultMonster;

    /// <summary>
    /// Reference to the smart monster GameObject.
    /// </summary>
    public GameObject smartMonster;

    /// <summary>
    /// Reference to the ghost monster GameObject.
    /// </summary>
    public GameObject ghostMonster;

    /// <summary>
    /// Reference to the FastMonsterScript component attached to the fast monster GameObject.
    /// </summary>
    private FastMonsterScript fastMonsterScript;

    /// <summary>
    /// Reference to the SpawnerScript component attached to the spawner GameObject.
    /// </summary>
    private SpawnerScript spawnerScript;

    [Header("Tilemaps")]
    /// <summary>
    /// Reference to the Tilemap representing the playable area.
    /// </summary>
    public Tilemap playableArea;

    /// <summary>
    /// Reference to the Tilemap representing the indestructible area.
    /// </summary>
    public Tilemap indestructableArea;

    
    //Players

    /// <summary>
    /// Reference to the first player GameObject.
    /// </summary>
    private GameObject player1;


    /// <summary>
    /// Reference to the second player GameObject.
    /// </summary>
    private GameObject player2;


    /// <summary>
    /// Array containing references to all player GameObjects.
    /// </summary>
    private GameObject[] players;

    
    //Monsters

    /// <summary>
    /// Array containing references to all types of monsters.
    /// </summary>
    private GameObject[] allMonsters;

    /// <summary>
    /// Array containing references to default monsters.
    /// </summary>
    private GameObject[] defaultMonsters;

    /// <summary>
    /// Array containing references to fast monsters.
    /// </summary>
    private GameObject[] fastMonsters;

    /// <summary>
    /// Array containing references to ghost monsters.
    /// </summary>
    private GameObject[] ghostMonsters;

    /// <summary>
    /// Array containing references to smart monsters.
    /// </summary>
    private GameObject[] smartMonsters;

    
    //Time

    /// <summary>
    /// Timer for controlling monster spawn intervals.
    /// </summary>
    private float timer = 0f;


    /// <summary>
    /// Interval at which monsters are spawned.
    /// </summary>
    private float repeatInterval = 7f;

    #endregion

    #region Unity events

    /// <summary>
    /// Initializes the game objects and variables at the start of the game.
    /// </summary>
    void Start()
    {
        // Initialize monster arrays   
        allMonsters = new GameObject[] {defaultMonster, fastMonster, smartMonster, ghostMonster };

        players = GameObject.FindGameObjectsWithTag("Player");
        fastMonsters = GameObject.FindGameObjectsWithTag("FastMonsterTag");
        defaultMonsters = GameObject.FindGameObjectsWithTag("DefaultMonsterTag");
        smartMonsters = GameObject.FindGameObjectsWithTag("SmartMonsterTag");
        ghostMonsters = GameObject.FindGameObjectsWithTag("GhostMonster");

        // Swap player assignments
        player1 = players[1];
        player2 = players[0];

        // Initialize spawner and free positions
        if (spawner != null)
        {
            spawnerScript = spawner.GetComponent<SpawnerScript>();
            spawnerScript.InitFreePositions();
        }
        else
        {
            Debug.Log("Spawner not found!");
        }
        InitSpawn();
    }

    /// <summary>
    /// Updates the logic of the game each frame.
    /// </summary>
    void Update()
    {
        // Check and update player radius
        ChangePlayerInRadius();

        // Update timer
        timer += Time.deltaTime; // Update the timer with the time since the last frame

        // Spawn monsters at regular intervals
        if (timer >= repeatInterval)
        {
            MonsterRegularSpawn();
            timer = 0f; // Reset the timer
        }

        
    }

    #endregion

    #region Public methods
    /// <summary>
    /// Spawns one of each monster type.
    /// </summary>
    public void InitSpawn()
    {
        spawnerScript.Spawn(defaultMonster);
        spawnerScript.Spawn(fastMonster);
        spawnerScript.Spawn(smartMonster);
        spawnerScript.Spawn(ghostMonster);
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Spawns a random enemy if the total number of monsters is less than 4.
    /// </summary>
    private void MonsterRegularSpawn()
    {
        fastMonsters = GameObject.FindGameObjectsWithTag("FastMonsterTag");
        defaultMonsters = GameObject.FindGameObjectsWithTag("DefaultMonsterTag");
        smartMonsters = GameObject.FindGameObjectsWithTag("SmartMonsterTag");
        ghostMonsters = GameObject.FindGameObjectsWithTag("GhostMonster");

        // Check total number of monsters
        if ((fastMonsters.Length + defaultMonsters.Length + smartMonsters.Length + ghostMonsters.Length) < 4)
        {
            // Choose a random monster type
            int randomNumber = Random.Range(0, allMonsters.Length);

            // Spawn the chosen monster
            spawnerScript.Spawn(allMonsters[randomNumber]);
        }

        
    }


    private void ChangePlayerInRadius()
    {
        foreach (GameObject monster in fastMonsters) // ha meghal egy szörny akkor bennemarad még itt:(
        {
            if(monster != null) // ez valamilyen szinten javitja, de nemtudom probléma marad/mennyire jo ha azokat is megvizsgálja akik már nem élnek
            {
                fastMonsterScript = monster.GetComponent<FastMonsterScript>();
                checkDistance(monster);
            }
            
        }
    }

    private void checkDistance(GameObject monster)
    {
        Vector3 pos1 = player1.transform.position;
        Vector3 pos2 = player2.transform.position;
        Vector3 distEnemy = monster.transform.position;

        float dist1 = Vector3.Distance(pos1, distEnemy);
        float dist2 = Vector3.Distance(pos2, distEnemy);

        if (dist1 > dist2)
        {
            fastMonsterScript.ChangePlayer(2);
        }
        else
        {
            fastMonsterScript.ChangePlayer(1);
        }
    }

    #endregion

}
