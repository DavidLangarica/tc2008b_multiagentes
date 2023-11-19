/// <summary>
/// The ProjectileDestroy class is responsible for destroying projectiles that have traveled too far.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDestroy : MonoBehaviour
{
    public float destroyDistance = 1000f;

    /// <summary>
    /// Update is called once per frame. It destroys the projectile if it has traveled too far.
    /// </summary>
    void Update()
    {
        if (Vector3.Distance(transform.position, Vector3.zero) > destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}
