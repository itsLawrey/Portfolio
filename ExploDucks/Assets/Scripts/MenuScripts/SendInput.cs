using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Sends input from player controls to corresponding controllers.
/// </summary>
public class SendInput : MonoBehaviour
{
    #region Fields
    /// <summary>
    /// The player input component.
    /// </summary>
    public PlayerInput input;

    /// <summary>
    /// The controller for player 1 movement.
    /// </summary>
    public PlayerController moveP1;

    /// <summary>
    /// The controller for player 2 movement.
    /// </summary>
    public PlayerController moveP2;

    /// <summary>
    /// The controller for player 1 bomb dropping.
    /// </summary>
    public BombController dropP1;

    /// <summary>
    /// The controller for player 2 bomb dropping.
    /// </summary>
    public BombController dropP2;

    /// <summary>
    /// The input action for player 1 movement.
    /// </summary>
    InputAction moveActionP1;

    /// <summary>
    /// The input action for player 2 movement.
    /// </summary>
    InputAction moveActionP2;

    /// <summary>
    /// The input action for player 1 bomb dropping.
    /// </summary>
    InputAction dropActionP1;

    /// <summary>
    /// The input action for player 2 bomb dropping.
    /// </summary>
    InputAction dropActionP2;

    #endregion

    #region Methods

    void Reset()
    {
        input = GetComponent<PlayerInput>();
    }
    /// <summary>
    /// Assign variables, set up listeners.
    /// </summary>
    void OnEnable()
    {
        moveActionP1 = input.actions.FindAction("MoveP1");
        moveActionP2 = input.actions.FindAction("MoveP2");
        dropActionP1 = input.actions.FindAction("DropBombP1");
        dropActionP2 = input.actions.FindAction("DropBombP2");
        input.onActionTriggered += OnAction;
    }
    /// <summary>
    /// Removes listeners for input actions.
    /// </summary>
    void OnDisable()
    {
        input.onActionTriggered -= OnAction;
    }

    /// <summary>
    /// Takes action when an input arrives, forwarding it to the appropriate controller.
    /// </summary>
    /// <param name="context">The type of the input</param>
    void OnAction(InputAction.CallbackContext context)
    {
        if (context.action == moveActionP1)
        {
            moveP1.OnMove(context.ReadValue<Vector2>());
        }
        else if (context.action == moveActionP2)
        {
            moveP2.OnMove(context.ReadValue<Vector2>());
        }
        else if (context.action == dropActionP1 && context.phase == InputActionPhase.Performed)
        {
            dropP1.OnDrop(context.ReadValue<float>());
        }
        else if (context.action == dropActionP2 && context.phase == InputActionPhase.Performed)
        {
            dropP2.OnDrop(context.ReadValue<float>());
        }
    }

    #endregion

}
