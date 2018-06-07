using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageLeftClick : Ability
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

    private MageMagic selectedMagic;

    private MageLeftClick()
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

        ChangeType((int)MageMagic.Classic);

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

    protected override void UseAbilityEffect(Vector3 mousePosition, bool isPressed)
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

    public override void UseAbility(Vector3 mousePosition, bool isPressed)
    {
        UseAbilityEffect(mousePosition, isPressed);
    }

    public override void UseAbilityOnNetwork(Vector3 mousePosition, bool isPressed)
    {
        base.UseAbilityOnNetwork(mousePosition, isPressed);
        if (isPressed)
        {
            ShootProjectile();
        }
    }

    public override void ChangeType(int magic)
    {
        if (magic == (int)MageMagic.Classic)
        {
            selectedMagic = MageMagic.Classic;
            speed = minigunSpeed;
            cooldown = minigunCooldown * cooldownReduction;
            range = minigunRange;
        }
        else if (magic == (int)MageMagic.Fire)
        {
            selectedMagic = MageMagic.Fire;
            speed = minigunSpeed;
            cooldown = minigunCooldown * cooldownReduction;
            range = minigunRange;
        }
        else if (magic == (int)MageMagic.Light)
        {
            selectedMagic = MageMagic.Light;
            speed = minigunSpeed;
            cooldown = minigunCooldown * cooldownReduction;
            range = minigunRange;
        }
        else
        {
            selectedMagic = MageMagic.Ice;
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
        if (selectedMagic == MageMagic.Classic)
        {
            cooldown = minigunCooldown * cooldownReduction;
        }
        else if (selectedMagic == MageMagic.Fire)
        {
            cooldown = minigunCooldown * cooldownReduction;
        }
        else if (selectedMagic == MageMagic.Light)
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
            StartCoroutine(PutAbilityOffCooldown());
        }
    }

    private void ShootProjectile()
    {
        Vector3 diff = lastMousePosition - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        Projectile projectile = Instantiate(GetProjectilePrefab(),
            transform.position + Vector3.back, Quaternion.Euler(0f, 0f, rot_z)).GetComponent<Projectile>();
        projectile.transform.position += projectile.transform.right * projectile.transform.localScale.x * 0.55f;
        projectile.ShootProjectile((int)selectedMagic, speed, range);
        projectile.OnProjectileHit += OnProjectileHit;
        projectile.OnProjectileReachedEnd += OnProjectileReachedEnd;
    }

    private GameObject GetProjectilePrefab()
    {
        if (selectedMagic == MageMagic.Classic)
        {
            return isAoE ? projectilePrefab2 : projectilePrefab1;
        }
        else if (selectedMagic == MageMagic.Fire)
        {
            return isAoE ? projectilePrefab2 : projectilePrefab1;
        }
        else if (selectedMagic == MageMagic.Light)
        {
            return isAoE ? projectilePrefab2 : projectilePrefab1;
        }
        else
        {
            return isAoE ? projectilePrefab4 : projectilePrefab3;
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
