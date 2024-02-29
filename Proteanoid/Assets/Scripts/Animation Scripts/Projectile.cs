using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] private ParticleSystem destructionParticlePrefab;
    public void FireProjectile(Action DestroyAction)
    {
        StartCoroutine(LifetimeRoutine(DestroyAction));
    }
    protected abstract IEnumerator LifetimeRoutine(Action DestroyAction);
    protected void DestroyProjectile()
    {
        if (destructionParticlePrefab != null)
            Instantiate(destructionParticlePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
