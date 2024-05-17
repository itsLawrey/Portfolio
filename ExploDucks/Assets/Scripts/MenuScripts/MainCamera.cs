using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Sets the backround color of the camera.
    /// </summary>
    /// <param name="color">The color to set to.</param>
    public void SetCameraBackgroundColor(Color color)
    {
        Camera.main.backgroundColor = color;
    }

    
}
