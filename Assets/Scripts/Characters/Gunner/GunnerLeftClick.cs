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

    protected string projectilePrefabPath3;
    protected GameObject projectilePrefab3;

    protected string projectilePrefabPath4;
    protected GameObject projectilePrefab4;

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
    private bool isAoE;

    private float horizontalSpeedPercentOnLeftClickActive;
    private bool cancelHorizontalSlow;

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
        rocketExplosionRadius = 2;

        horizontalSpeedPercentOnLeftClickActive = 0.5f;

        ChangeType((int)GunnerWeapon.Minigun);

        IsHoldDownAbility = true;

        projectilePrefabPath1 = "ProjectilePrefabs/GunnerMinigunBasicAttack";
        projectilePrefabPath2 = "ProjectilePrefabs/GunnerMinigunBasicAttackAoE";
        projectilePrefabPath3 = "ProjectilePrefabs/GunnerRocketLauncherBasicAttack";
        projectilePrefabPath4 = "ProjectilePrefabs/GunnerRocketLauncherBasicAttackAoE";
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
        projectilePrefab3 = Resources.Load<GameObject>(projectilePrefabPath3);
        projectilePrefab4 = Resources.Load<GameObject>(projectilePrefabPath4);
    }

    protected override void UseAbilityEffect(Vector3 mousePosition, bool isPressed, bool forceAbility = false)
    {
        if (this.isPressed != isPressed)
        {
            if (cancelHorizontalSlow)
            {
                player.PlayerMovementManager.ChangeHorizontalSpeed(1);
            }
            else
            {
                player.PlayerMovementManager.ChangeHorizontalSpeed(isPressed ? horizontalSpeedPercentOnLeftClickActive : 1);
            }
        }
        this.isPressed = isPressed;
        lastMousePosition = mousePosition;
    }

    public override void UseAbility(Vector3 mousePosition, bool isPressed, bool forceAbility = false)
    {
        UseAbilityEffect(mousePosition, isPressed, forceAbility);
    }

    public override void UseAbilityOnNetwork(Vector3 mousePosition, bool isPressed, bool forceAbility)
    {
        base.UseAbilityOnNetwork(mousePosition, isPressed, forceAbility);
        if (isPressed)
        {
            ShootProjectile();
        }
    }

    public override void ChangeType(int weapon)
    {
        if (weapon == (int)GunnerWeapon.Minigun)
        {
            selectedWeapon = GunnerWeapon.Minigun;
            speed = minigunSpeed;
            cooldown = minigunCooldown * cooldownReduction;
            range = minigunRange;
        }
        else
        {
            selectedWeapon = GunnerWeapon.RocketLauncher;
            speed = rocketSpeed;
            cooldown = rocketCooldown * cooldownReduction;
            range = rocketRange;
        }
    }

    public void SetAoE(bool isAoE)
    {
        this.isAoE = isAoE;
    }

    public override void SetCooldownReduction(float cooldownReduction)
    {
        this.cooldownReduction = cooldownReduction;
        if (selectedWeapon == GunnerWeapon.Minigun)
        {
            cooldown = minigunCooldown * cooldownReduction;
        }
        else
        {
            cooldown = rocketCooldown * cooldownReduction;
        }
        cancelHorizontalSlow = cooldownReduction < 1;
        if (cancelHorizontalSlow)
        {
            player.PlayerMovementManager.ChangeHorizontalSpeed(1);
        }
        else
        {
            player.PlayerMovementManager.ChangeHorizontalSpeed(isPressed ? horizontalSpeedPercentOnLeftClickActive : 1);
        }
        if (cooldownRemaining > cooldown)
        {
            cooldownRemaining = cooldown;
        }
    }

    private void Update()
    {
        if (player.PhotonView.isMine && isPressed && !IsOnCooldown)
        {
            ShootProjectile();
            StartCooldown();
        }
    }

    private void ShootProjectile()
    {
        Vector3 diff = lastMousePosition - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        Projectile projectile = Instantiate(selectedWeapon == GunnerWeapon.Minigun ? (isAoE ? projectilePrefab2 : projectilePrefab1) : (isAoE ? projectilePrefab4 : projectilePrefab3),
            transform.position + Vector3.back, Quaternion.Euler(0f, 0f, rot_z)).GetComponent<Projectile>();
        projectile.transform.position += projectile.transform.right * projectile.transform.localScale.x * 0.55f;
        projectile.ShootProjectile((int)selectedWeapon, speed, range);
        projectile.OnProjectileHit += OnProjectileHit;
        projectile.OnProjectileReachedEnd += OnProjectileReachedEnd;
    }

    private void OnProjectileHit(Projectile projectile, int weapon, GameObject targetHit)
    {
        if (weapon == (int)GunnerWeapon.RocketLauncher)
        {
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(targetHit.transform.position, rocketExplosionRadius))
            {
                if (collider.gameObject.tag == "Enemy")
                {
                    collider.GetComponent<Health>().Reduce(rocketDamage * damageAmplification);
                }
            }
        }
        else
        {
            if (player.PhotonView.isMine && targetHit.tag == "Enemy")
            {
                targetHit.GetComponent<Health>().Reduce(minigunDamage * damageAmplification);
            }
        }

        if (projectile is SingleTargetProjectile)
        {
            Destroy(projectile.gameObject);
        }
    }

    private void OnProjectileReachedEnd(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }
}
