using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindScript : MonoBehaviour
{
    public InputActionReference MoveRef, DropRef; 
    /// <summary>
    /// When the script is enabled, disable inputs to safely rebind.
    /// </summary>
    private void OnEnable()
    {
        MoveRef.action.Disable();
        DropRef.action.Disable();
    }
    /// <summary>
    /// When the script is disabled, enable inputs to be able to move.
    /// </summary>
    private void OnDisable()
    {
        MoveRef.action.Enable();
        DropRef.action.Enable();
    }
}
