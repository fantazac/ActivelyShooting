using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerLeftClick : Ability
{
    protected string projectilePrefabPath1;
    protected GameObject projectilePrefab1;

    protected string projectilePrefabPath2;
    protected GameObject projectilePrefab2;

    private float speed;
    private float range;

    private float minigunCooldown;
    private float minigunDamage;
    private float minigunSpeed;
    private float minigunRange;

    private float rocketCooldown;
    private float rocketDamage;
    private float rocketSpeed;
    private float rocketRange;
    private float rocketExplosionRadius;

    private bool isPressed;

    private float horizontalSpeedPercentOnLeftClickActive;

    private Vector3 lastMousePosition;

    private GunnerWeapon selectedWeapon;

    private GunnerLeftClick()
    {
        minigunCooldown = 0.12f;
        minigunDamage = 15;
        minigunSpeed = 12;
        minigunRange = 18;

        rocketCooldown = 0.8f;
        rocketDamage = 55;
        rocketSpeed = 9;
        rocketRange = 60;
        rocketExplosionRadius = 2.5f;

        horizontalSpeedPercentOnLeftClickActive = 0.5f;

        ChangeWeapon((int)GunnerWeapon.Minigun);

        projectilePrefabPath1 = "ProjectilePrefabs/GunnerMinigunBasicAttack";
        projectilePrefabPath2 = "ProjectilePrefabs/GunnerRocketLauncherBasicAttack";
    }

    protected override void Awake()
    {
        base.Awake();

        LoadPrefabs();
    }

    private void LoadPrefabs()
    {
        projectilePrefab1 = Resources.Load<GameObject>(projectilePrefabPath1);
        projectilePrefab2 = Resources.Load<GameObject>(projectilePrefabPath2);
    }

    protected override void UseAbilityEffect(Vector3 mousePosition, bool isPressed)
    {
        this.isPressed = isPressed;
        lastMousePosition = mousePosition;//todo
        player.PlayerMovement.ChangeHorizontalSpeed(isPressed ? horizontalSpeedPercentOnLeftClickActive : 1);
    }

    public override void UseAbility(Vector3 mousePosition, bool isPressed)
    {
        UseAbilityEffect(mousePosition, isPressed);
    }

    public override void ChangeWeapon(int weapon)
    {
        if (weapon == (int)GunnerWeapon.Minigun)
        {
            selectedWeapon = GunnerWeapon.Minigun;
            speed = minigunSpeed;
            cooldown = minigunCooldown;
            range = minigunRange;
        }
        else
        {
            selectedWeapon = GunnerWeapon.RocketLauncher;
            speed = rocketSpeed;
            cooldown = rocketCooldown;
            range = rocketRange;
        }
    }

    private void Update()
    {
        if (isPressed && !IsOnCooldown)
        {
            Vector3 diff = lastMousePosition - transform.position;
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

            Projectile projectile = Instantiate(selectedWeapon == GunnerWeapon.Minigun ? projectilePrefab1 : projectilePrefab2,
                transform.position + Vector3.back, Quaternion.Euler(0f, 0f, rot_z)).GetComponent<Projectile>();
            projectile.transform.position += projectile.transform.right * projectile.transform.localScale.x * 0.55f;
            projectile.ShootProjectile((int)selectedWeapon, speed, range);
            projectile.OnProjectileHit += OnProjectileHit;
            projectile.OnProjectileReachedEnd += OnProjectileReachedEnd;

            StartCoroutine(PutAbilityOffCooldown());
        }
    }

    private void OnProjectileHit(Projectile projectile, int weapon, GameObject targetHit)
    {
        if (weapon == 1)
        {
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(projectile.transform.position, rocketExplosionRadius))
            {
                if (collider.gameObject.tag == "Enemy")
                {
                    collider.GetComponent<Health>().Reduce(rocketDamage);
                }
            }
        }
        else
        {
            if (player.PhotonView.isMine && targetHit.tag == "Enemy")
            {
                targetHit.GetComponent<Health>().Reduce(minigunDamage);
            }
        }
        Destroy(projectile.gameObject);
    }

    private void OnProjectileReachedEnd(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }
}
