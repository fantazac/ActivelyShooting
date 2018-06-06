using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected float speed;
    protected float range;
    protected int weapon;

    protected Vector3 initialPosition;

    public delegate void OnProjectileReachedEndHandler(Projectile projectile);
    public event OnProjectileReachedEndHandler OnProjectileReachedEnd;

    public delegate void OnProjectileHitHandler(Projectile projectile, int weapon, GameObject targetHit);
    public event OnProjectileHitHandler OnProjectileHit;

    protected IEnumerator ActivateAbilityEffect()
    {
        while (Vector2.Distance(transform.position, initialPosition) < range)
        {
            transform.position += transform.right * Time.deltaTime * speed;

            yield return null;
        }

        OnProjectileReachedEndOfRange();
    }

    public void ShootProjectile(int weapon, float speed, float range)
    {
        this.speed = speed;
        this.range = range;
        this.weapon = weapon;
        initialPosition = transform.position;
        StartCoroutine(ActivateAbilityEffect());
    }

    protected void ProjectileHit(GameObject targetHit)
    {
        OnProjectileHit(this, weapon, targetHit);
    }

    protected void ProjectileReachedEnd()
    {
        OnProjectileReachedEnd(this);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider) { }

    protected void OnProjectileReachedEndOfRange()
    {
        if (OnProjectileReachedEnd != null)
        {
            OnProjectileReachedEnd(this);
        }
    }
}
