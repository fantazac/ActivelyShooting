using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerRightClick : Ability
{
    protected string projectilePrefabPath;
    protected GameObject projectilePrefab;

    private float speed;
    private float range;
    private float damage;

    private GunnerRightClick()
    {
        speed = 13;
        range = 60;
        damage = 75;

        baseCooldown = 10;
        cooldown = baseCooldown;

        projectilePrefabPath = "ProjectilePrefabs/GunnerSpecialAttack";
    }

    protected override void Awake()
    {
        base.Awake();

        LoadPrefabs();
    }

    private void LoadPrefabs()
    {
        projectilePrefab = Resources.Load<GameObject>(projectilePrefabPath);
    }

    protected override void UseAbilityEffect(Vector3 mousePosition, bool isPressed)
    {
        Vector3 diff = mousePosition - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        Projectile projectile = Instantiate(projectilePrefab, transform.position + Vector3.back, Quaternion.Euler(0f, 0f, rot_z)).GetComponent<Projectile>();
        projectile.transform.position += projectile.transform.right * projectile.transform.localScale.x * 0.55f;
        projectile.ShootProjectile(0, speed, range);
        projectile.OnProjectileHit += OnProjectileHit;
        projectile.OnProjectileReachedEnd += OnProjectileReachedEnd;
    }

    private void OnProjectileHit(Projectile projectile, int weapon, GameObject targetHit)
    {
        if (player.PhotonView.isMine && targetHit.tag == "Enemy")
        {
            targetHit.GetComponent<Health>().Reduce(damage * damageAmplification);
        }
    }

    private void OnProjectileReachedEnd(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }
}
