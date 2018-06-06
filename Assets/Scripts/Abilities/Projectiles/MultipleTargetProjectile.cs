using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTargetProjectile : Projectile
{
    protected List<Collider2D> alreadyHitTargets;

    private MultipleTargetProjectile()
    {
        alreadyHitTargets = new List<Collider2D>();
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy" && !alreadyHitTargets.Contains(collider))
        {
            alreadyHitTargets.Add(collider);
            ProjectileHit(collider.gameObject);
        }
    }
}
