using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [HideInInspector]
    /// <summary>
    /// The distance represents the maximum reach of the bomb.
    /// </summary>
    public int radius;

    [HideInInspector]
    /// <summary>
    /// The player who placed the bomb.
    /// </summary>
    public GameObject player;
}
