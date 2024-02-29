using System.Collections;
using UnityEngine;

public class OscillateMovement : MonoBehaviour
{
    [SerializeField] private Vector2 oscillateSpeed;
    [SerializeField] private float oscillateDuration;
    [SerializeField] private AnimationCurve curve;
    private void Start()
    {
        StartCoroutine(OscillateRoutine());
    }

    //Infinitely locally move this object back and forth at its oscillateSpeed.
    private IEnumerator OscillateRoutine()
    {
        while(true)
        {
            float timeElapsed = 0;
            while (timeElapsed < oscillateDuration)
            {
                timeElapsed += Time.deltaTime;
                float speed = curve.Evaluate(timeElapsed / oscillateDuration);
                transform.Translate(speed * oscillateSpeed * Time.deltaTime, Space.World);
                yield return null;
            }

            timeElapsed = 0;
            while (timeElapsed < oscillateDuration)
            {
                timeElapsed += Time.deltaTime;
                float speed = curve.Evaluate(timeElapsed / oscillateDuration);
                transform.Translate(speed * -oscillateSpeed * Time.deltaTime, Space.World);
                yield return new WaitForSeconds(0);
            }
        }
    }
}
