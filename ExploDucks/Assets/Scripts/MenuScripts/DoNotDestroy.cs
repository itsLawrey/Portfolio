using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    /// <summary>
    /// Called when a scene loads.
    /// </summary>
    private void Awake()
    {
        GameObject[] musicobj = GameObject.FindGameObjectsWithTag("GameMusic");
        if (musicobj.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
