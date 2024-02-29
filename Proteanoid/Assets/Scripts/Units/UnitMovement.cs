using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    private Vector2 originalPos;
    [SerializeField] private AnimationCurve movementCurve;
    [SerializeField] private Transform aggroPos;

    private bool isAtAggroPos;
    private void Awake()
    {
        originalPos = transform.position;
    }

    /// <summary>
    /// Lerps this unit to its aggro pos (the position it should move to when attacking).
    /// </summary>
    public IEnumerator MoveToAggroPos()
    {
        if (!isAtAggroPos)
        {
            yield return LerpToPos(originalPos, aggroPos.position);
            isAtAggroPos = true;
        }
        else
            yield return null;
    }

    /// <summary>
    /// Lerps this enemy to the position it was in at the start of the fight.
    /// </summary>
    public IEnumerator MoveToOriginalPos()
    {
        if (isAtAggroPos)
            yield return LerpToPos(transform.position, originalPos);
        yield return null;
        isAtAggroPos = false;
    }

    /// <summary>
    /// Lerps an enemy to a given pos
    /// </summary>
    /// <param name="start">Where the enemy should start (typically wherever it currently is)</param>
    /// <param name="end">Where the enemy should end up.</param>
    private IEnumerator LerpToPos(Vector2 start, Vector2 end)
    {
        float timeElapsed = 0;
        const float movementTime = 0.8f;

        while (timeElapsed < movementTime)
        {
            timeElapsed += Time.deltaTime;
            float percentComplete = movementCurve.Evaluate(timeElapsed / movementTime);
            transform.position = Vector2.Lerp(start, end, percentComplete);
            yield return null;
        }
    }
}
