using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : Projectile
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float speed;
    public Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void MoveRotationTowardTarget()
    {
        if (target == null)
            return;
        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    protected override IEnumerator LifetimeRoutine(Action DestroyAction)
    {
        float timeElapsed = 0;
        while (Vector2.Distance(transform.position, target.position) > 0.5f && timeElapsed < 2.5f) 
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime, Space.Self);
            MoveRotationTowardTarget();

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        DestroyAction();
        DestroyProjectile();
    }
}
