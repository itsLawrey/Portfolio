using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns power-up objects based on bomb explosion events.
/// </summary>
public class PowerUpSpawner : MonoBehaviour
{
    /// <summary>
    /// Array of power-up prefabs that can be spawned.
    /// </summary>
    public GameObject[] prefabList;


    /// <summary>
    /// Invoked when a bomb explodes, triggering power-up spawning.
    /// </summary>
    /// <param name="position">The position of the bomb explosion.</param>
    public void OnBombExploded(Vector2 position)
    {
        // Randomly determine if a power-up should spawn
        int chance = Random.Range(0, 5);
        int powerUpInd = Random.Range(0, prefabList.Length);
        if (chance == 1)
        {
            // Spawn a random power-up prefab at the bomb explosion position
            Instantiate(prefabList[powerUpInd], position, Quaternion.identity);
        }

    }
}
