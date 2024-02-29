using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves an object to a local position of (0,0) plus a random offset.
/// Use this class only on child objects of a moving parent.
/// </summary>
public class ShakeMovement : MonoBehaviour
{
    public bool isShaking;
    [SerializeField] private Vector2 shakeMagnitude;
    [SerializeField] private Vector2 posOffset;
    private void FixedUpdate()
    {
        if (isShaking)
            transform.localPosition = posOffset + (Random.insideUnitSphere * shakeMagnitude);
    }

    /// <summary>
    /// Shakes an object for a given number of seconds.
    /// </summary>
    /// <param name="duration">The effectDuration the object should shake for.</param>
    public IEnumerator ShakeForDuration(float duration)
    {
        StartShaking();
        yield return new WaitForSeconds(duration);
        StopShaking();
    }

    /// <summary>
    /// Starts shaking the object indefinitely.
    /// </summary>
    public void StartShaking()
    {
        isShaking = true;
    }

    /// <summary>
    /// Stops shaking the object.
    /// </summary>
    public void StopShaking()
    {
        isShaking = false;
        transform.localPosition = posOffset;
    }
}
