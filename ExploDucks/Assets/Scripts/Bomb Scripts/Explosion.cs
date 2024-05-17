using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the visual appearance and orientation of an explosion effect.
/// </summary>
public class Explosion : MonoBehaviour
{
    /// <summary>
    /// SpriteRenderer for the start of the explosion.
    /// </summary>
    public SpriteRenderer start;

    /// <summary>
    /// SpriteRenderer for the middle part of the explosion.
    /// </summary>
    public SpriteRenderer middle;

    /// <summary>
    /// SpriteRenderer for the end of the explosion.
    /// </summary>
    public SpriteRenderer end;


    /// <summary>
    /// Sets the active SpriteRenderer based on the given renderer.
    /// </summary>
    /// <param name="renderer">The SpriteRenderer to be set as active.</param>
    public void SetActiveRenderer(SpriteRenderer renderer)
    {
        start.enabled = renderer == start;
        middle.enabled = renderer == middle;
        end.enabled = renderer == end;
    }

    /// <summary>
    /// Sets the rotation of the explosion based on the given direction.
    /// </summary>
    /// <param name="direction">The direction vector for the explosion.</param>
    public void SetDirection(Vector2 direction){
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }
}
