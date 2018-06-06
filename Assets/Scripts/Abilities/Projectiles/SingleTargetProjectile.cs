using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargetProjectile : Projectile
{
    private bool alreadyHitATarget;//This is to prevent OnTriggerEnter to cast multiple times if multiple targets enter the collider at the same time

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (!alreadyHitATarget)
        {
            if (collider.gameObject.tag == "Platform" || collider.gameObject.tag == "Wall" || collider.gameObject.tag == "Ceiling" || collider.gameObject.tag == "Floor" ||
                collider.gameObject.tag == "Enemy")
            {
                ProjectileHit(collider.gameObject);
                alreadyHitATarget = true;
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}
