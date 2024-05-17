using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


/// <summary>
/// Manages the progression of rounds, player states, and UI updates throughout the game.
/// </summary>
public class RoundManager : MonoBehaviour
{
    #region Fields
    [Header("Display")]
    /// <summary>
    /// Reference to the TextMeshProUGUI for displaying the current round number.
    /// </summary>
    [SerializeField] private TextMeshProUGUI _text_rounds;

    /// <summary>
    /// Reference to the TextMeshProUGUI for displaying player 1's statistics.
    /// </summary>
    [SerializeField] private TextMeshProUGUI _text_p1_stats;
    
    /// <summary>
    /// Reference to the TextMeshProUGUI for displaying player 2's statistics.
    /// </summary>
    [SerializeField] private TextMeshProUGUI _text_p2_stats;

    /// <summary>
    /// Reference to the TextMeshProUGUI for displaying a message when a player dies.
    /// </summary>
    [SerializeField] private TextMeshProUGUI _text_playerdied;

    /// <summary>
    /// Reference to the TextMeshProUGUI for displaying the game-over message when a game is won.
    /// </summary>
    [SerializeField] private TextMeshProUGUI _text_gameover_won;

    /// <summary>
    /// Reference to the TextMeshProUGUI for displaying the remaining time during rounds.
    /// </summary>
    [SerializeField] private TextMeshProUGUI timer;

    [Header("Players")]
    /// <summary>
    /// Reference to player 1 GameObject.
    /// </summary>
    [SerializeField] public GameObject P1;

    /// <summary>
    /// Reference to player 2 GameObject.
    /// </summary>
    [SerializeField] public GameObject P2;

    [Header("Game")]
    /// <summary>
    /// Reference to the game info GameObject.
    /// </summary>
    [SerializeField] private GameObject gameInfo;

    /// <summary>
    /// Reference to the GameObject representing the game itself.
    /// </summary>
    [SerializeField] private GameObject game;

    /// <summary>
    /// Reference to the MainMenu script controlling the main camera.
    /// </summary>
    [SerializeField] private MainMenu maincam;

    /// <summary>
    /// GameObject representing the round-over menu.
    /// </summary>
    public GameObject roundOverMenu;

    /// <summary>
    /// GameObject representing the game-over menu.
    /// </summary>
    public GameObject gameOverMenu;

    /// <summary>
    /// List of GameObjects to keep in the scene when resetting.
    /// </summary>
    public List<GameObject> objectsToKeep = new List<GameObject>();

    /// <summary>
    /// Reference to the SpawnerScript for spawning objects.
    /// </summary>
    public SpawnerScript spawner;

    /// <summary>
    /// Reference to the main logic GameObject.
    /// </summary>
    public GameObject logic;

    /// <summary>
    /// Reference to the LogicScript for game logic.
    /// </summary>
    public LogicScript logicScript;

    /// <summary>
    /// Flag indicating whether a player has died.
    /// </summary>
    private bool playerHasDied = false;

    /// <summary>
    /// Current round number.
    /// </summary>
    private int currentRound = 1;

    /// <summary>
    /// Array containing all player controllers.
    /// </summary>
    private PlayerController[] players;
    //kiszedjuk a beallitasokbol amit beraktunk a menuben

    /// <summary>
    /// Maximum number of rounds in the game.
    /// </summary>
    int maxRound;

    /// <summary>
    /// Starting positions of the players.
    /// </summary>
    private Vector3[] startingPositions;

    // <summary>
    /// Coroutine for managing the end of a round.
    /// </summary>
    private Coroutine roundEndCoroutine;

    #endregion

    #region Unity events

    /// <summary>
    /// Initializes the game state at the start of the scene. Called before the first frame update
    /// </summary>
    void Start()
    {
        players = FindObjectsOfType<PlayerController>();
        logic = GameObject.FindGameObjectWithTag("LogicManager");
        logicScript = logic.GetComponent<LogicScript>();

        maxRound = PlayerPrefs.GetInt("NumberOfRounds", 99);// alapbol legyen 99 h lasd h nem allitottal be semmit
        _text_rounds.text = "Round " + currentRound;
        StoreStartingPositions();
        DisplayPlayerData();
    }

    /// <summary>
    /// Checks for player death and updates the player data display.
    /// </summary>
    void Update()
    {
        if (!playerHasDied)
        {
            CheckPlayerAlive();
        }
        DisplayPlayerData();
    }
    #endregion

    #region Menus
    /// <summary>
    /// Makes the round-over / game-over display visible when a round is over in the game depending on the points of the players.
    /// </summary>
    void ActivateMenuPopUp()
    {
        PlayerController player1 = P1.GetComponent<PlayerController>();
        PlayerController player2 = P2.GetComponent<PlayerController>();

        if (player1.points < maxRound && player2.points < maxRound)//roundover menu
        {
            PauseGame(roundOverMenu);

            PlayerController deadPlayer = null;
            PlayerController otherPlayer = null;
            

            if (!IsAnyPlayerAlive(players))
            {
                _text_playerdied.text = $"Both players have died.\n" +
                                         $"No points awarded.";
            }
            else
            {
                if (player1.IsDead())
                {
                    deadPlayer = player1;
                    otherPlayer = player2;
                }
                else if (player2.IsDead())
                {
                    deadPlayer = player2;
                    otherPlayer = player1;
                }

                if (deadPlayer != null && otherPlayer != null)
                {
                    int deadPlayerNumber = (deadPlayer == player1) ? 1 : 2;
                    int otherPlayerNumber = (otherPlayer == player1) ? 1 : 2;

                    _text_playerdied.text = $"Player {deadPlayerNumber} has died.\n" +
                                             $"Player {otherPlayerNumber} has been awarded a point.";
                }
            }

        }
        else//gameover menu
        {
            PauseGame(gameOverMenu);

            if (player1.points > player2.points)
            {
                _text_gameover_won.text = $"Player 1 won the game with {player1.points} points.\n" +
                                            $"Player 2 had {player2.points} points. Too bad.";
            }
            if (player1.points < player2.points)
            {
                _text_gameover_won.text = $"Player 2 won the game with {player2.points} points.\n" +
                                            $"Player 1 had {player1.points} points. Too bad.";
            }
            if (player1.points == player2.points)
            {
                _text_gameover_won.text = $"Draw.\n" +
                                            $"Both players had {player1.points} points.";
            }
        }

    }

    /// <summary>
    /// Sets the menu gameobject to visible and the game gameobject to not visible.
    /// </summary>
    /// <param name="menu">The menu you want to display.</param>
    void PauseGame(GameObject menu)
    {
        menu.SetActive(true);
        game.SetActive(false);
        gameInfo.SetActive(false);
        maincam.SetCameraBackgroundColor(Color.black);
    }
    
    /// <summary>
    /// Destroys every gameobject in the scene which is not included in the objectToKeep list.
    /// </summary>
    void DestroyObjectsNotInList()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj != null && !IsInObjectsToKeep(obj) && obj != GameObject.Find("Music"))
            {
                Destroy(obj);
            }
        }
    }
    /// <summary>
    /// Determines whether an object should be kept.
    /// </summary>
    /// <param name="obj">The object to check.</param>
    /// <returns>True if the object needs to be kept; False if it needs to be deleted.</returns>
    bool IsInObjectsToKeep(GameObject obj)
    {
        foreach (GameObject go in objectsToKeep)
        {
            if (obj == go || IsChildOf(obj, go))
            {
                return true;
            }
        }
        return false;
    }
    
    /// <summary>
    /// Decide if an object is a child of another object.
    /// </summary>
    /// <param name="obj">The child object.</param>
    /// <param name="parent">The parent object.</param>
    /// <returns>True if obj is a child of parent, False otherwise.</returns>
    bool IsChildOf(GameObject obj, GameObject parent)
    {
        Transform t = obj.transform;
        while (t.parent != null)
        {
            if (t.parent.gameObject == parent)
            {
                return true;
            }
            t = t.parent.transform;
        }
        return false;
    }
    #endregion

    #region Player location and state

    /// <summary>
    /// Contains actions to take if not all players are alive in a round.
    /// </summary>
    void CheckPlayerAlive()
    {
        if (!IsBothPlayersAlive())
        {
            PlayerController survivor = GetSurvivingPlayer();
            PlayerController loser = (survivor == P1.GetComponent<PlayerController>()) ? P2.GetComponent<PlayerController>() : P1.GetComponent<PlayerController>();

            // Deactivates the loser's game object without destroying it.
            loser.gameObject.SetActive(false);

            playerHasDied = true;
            timer.gameObject.SetActive(true);
            roundEndCoroutine = StartCoroutine(EndRoundAfterDelay(5f));
        }
    }

    /// <summary>
    /// Stores the starting locations of the player objects.
    /// </summary>
    public void StoreStartingPositions()
    {

        startingPositions = new Vector3[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            startingPositions[i] = players[i].transform.position;
        }
    }
    
    /// <summary>
    /// Resets the player objects and their scripts for a new round.
    /// </summary>
    void ResetPlayers()
    {

        for (int i = 0; i < players.Length; i++)
        {
            // Revives the player.
            players[i].Revive();

            // Sets the player's position to the starting position.
            players[i].transform.position = startingPositions[i];

            // Activates the player's game object.
            players[i].gameObject.SetActive(true);

            // Reset bomb properties
            BombController bombController = players[i].GetComponent<BombController>();
            ObstacleController obstacleController = players[i].GetComponent<ObstacleController>();
            if (bombController != null)
            {
                // Resets bomb properties.
                // Remove power ups
                bombController.ResetProperties();
                obstacleController.ResetProperties();
            }
        }
    }

    /// <summary>
    /// Displays up to date information about the players.
    /// </summary>
    private void DisplayPlayerData()
    {
        PlayerController player1 = P1.GetComponent<PlayerController>();
        PlayerController player2 = P2.GetComponent<PlayerController>();
        BombController player1bomb = player1.GetComponent<BombController>();
        BombController player2bomb = player2.GetComponent<BombController>();

        _text_p1_stats.text = "Player 1\n\n" +
                              $"Points: {player1.points} / {maxRound}\n" +
                              $"Bombs: {player1bomb.bombAmount}\n" +
                              $"Range: {player1bomb.explosionRadius}\n" +
                              $"Speed: {player1.speed}";
        _text_p2_stats.text = "Player 2\n\n" +
                              $"Points: {player2.points} / {maxRound}\n" +
                              $"Bombs: {player2bomb.bombAmount}\n" +
                              $"Range: {player2bomb.explosionRadius}\n" +
                              $"Speed: {player2.speed}";
    }

    /// <summary>
    /// Decides if any of the 2 players are alive.
    /// </summary>
    /// <param name="players">List of player objects</param>
    /// <returns>True if there is at least one player object alive, false otherwise</returns>
    bool IsAnyPlayerAlive(PlayerController[] players)
    {
        foreach (PlayerController player in players)
        {
            if (player.IsDead() == false)
            {
                return true; // At least one player is alive
            }
        }
        return false; // No player is alive
    }

    /// <summary>
    /// Decides if all of the 2 players are alive.
    /// </summary>
    /// <returns>True if there is 2 player objects alive, false otherwise</returns>
    bool IsBothPlayersAlive()
    {
        PlayerController player1 = P1.GetComponent<PlayerController>();
        PlayerController player2 = P2.GetComponent<PlayerController>();

        return !player1.IsDead() && !player2.IsDead();
    }
   /// <summary>
   /// Find the script of the player who is alive
   /// </summary>
   /// <returns>PlayerController object belonging to the surviving player.</returns>
    PlayerController GetSurvivingPlayer()
    {
        PlayerController player1 = P1.GetComponent<PlayerController>();
        PlayerController player2 = P2.GetComponent<PlayerController>();

        return player1.IsDead() ? player2 : player1;
    }
    #endregion

    #region Rounds
    /// <summary>
    /// Returns current round.
    /// </summary>
    /// <returns>int</returns>
    public int CurrentRound()
    {
        return currentRound;
    }

    /// <summary>
    /// Actions to take when a new round is about to begin.
    /// </summary>
    public void AdvanceRound()
    {
        // Resets the flag indicating if a player has died.
        playerHasDied = false;

        // Resets player objects, rebuilds the map, and initializes spawn logic.
        ResetPlayers();
        spawner.RebuildMap();
        logicScript.InitSpawn();

        // Hides round-over menu, timer, and sets camera background color.
        roundOverMenu.SetActive(false);
        timer.gameObject.SetActive(false);
        maincam.SetCameraBackgroundColor(new Color(0.447f, 0.764f, 0.478f)); // RGB values of 72C37A

        // Activates the game and game info UI elements.
        game.SetActive(true);
        gameInfo.SetActive(true);

        // Updates the round count and displays it.
        UpdateRoundCount();
        _text_rounds.text = "Round " + currentRound;


    }

    /// <summary>
    /// Increments the currentRound property.
    /// </summary>
    public void UpdateRoundCount()
    {
        currentRound++;
    }

    /// <summary>
    /// Unity coroutine used to wait for 5 seconds after a player has died, meanwhile displaying the remaining time every frame.
    /// </summary>
    /// <param name="delay">The duration to wait in seconds.</param>
    /// <returns>An IEnumerator representing the coroutine.</returns>
    IEnumerator EndRoundAfterDelay(float delay)
    {
        float elapsedTime = 0f;


        while (elapsedTime < delay)
        {

            yield return null; // Wait for one frame

            float remainingTime = delay - elapsedTime;
            string formattedTime = remainingTime.ToString("0.00");
            timer.text = "Time remaining: " + formattedTime;


            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            // Check if any player dies during the delay
            if (!IsAnyPlayerAlive(players))
            {
                // If both players are dead or the second player died during the delay, end the game instantly
                ActivateMenuPopUp();
                DestroyObjectsNotInList();
                yield break; // Exit the coroutine
            }
        }
        // Increment points for the surviving player and activate the appropriate menu
        PlayerController survivor = GetSurvivingPlayer();
        survivor.points++;

        ActivateMenuPopUp();
        DestroyObjectsNotInList();
    }

    #endregion
}
