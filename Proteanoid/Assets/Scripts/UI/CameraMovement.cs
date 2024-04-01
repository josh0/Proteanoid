using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : Singleton<CameraMovement>
{
    [SerializeField] private AnimationCurve moveCurve;
    [field: SerializeField] public Transform mapPos { get; private set; }

    /// <summary>
    /// Lerps the camera to a particular position on a movement curve.
    /// </summary>
    /// <param name="pos">The position the camera should move to.</param>
    /// <param name="moveTime">The time it should take for the camera to move.</param>
    public IEnumerator MoveToPos(Vector3 pos, float moveTime)
    {
        float timeElapsed = 0;
        Vector3 startingPos = transform.position;

        while(timeElapsed < moveTime)
        {
            timeElapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startingPos, pos, moveCurve.Evaluate(timeElapsed / moveTime));
            yield return null;
        }
    }
}
