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

    private bool canShootInCycle;
    private bool readyToShoot;

    private bool isPressed;

    private float classicDamage;
    private float classicHeal;

    private float fireDamage;
    private float fireHeal;

    private float iceDamage;
    private float iceHeal;
    private float iceSlowPercent;
    private float iceSlowDuration;

    private float lightDamage;
    private float lightHeal;

    private float horizontalSpeedPercentOnLeftClickActive;

    private Vector3 lastMousePosition;

    private MageMagic selectedMagic;

    private MageLeftClick()
    {
        baseCooldown = 1;
        cooldown = baseCooldown;

        horizontalSpeedPercentOnLeftClickActive = 0f;

        speed = 14;
        range = 35;

        classicDamage = 35;
        classicHeal = 10;

        fireDamage = 50;
        fireHeal = 0;

        iceDamage = 35;
        iceHeal = 0;
        iceSlowPercent = 0.4f;
        iceSlowDuration = 2;

        lightDamage = 0;
        lightHeal = 35;

        ChangeType((int)MageMagic.Classic);

        IsHoldDownAbility = true;

        projectilePrefabPath1 = "ProjectilePrefabs/MageClassicBasicAttack";
        projectilePrefabPath2 = "ProjectilePrefabs/MageFireBasicAttack";
        projectilePrefabPath3 = "ProjectilePrefabs/MageIceBasicAttack";
        projectilePrefabPath4 = "ProjectilePrefabs/MageLightBasicAttack";
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

    public override bool IsAvailable()
    {
        return base.IsAvailable() && !player.PlayerMovementManager.PlayerIsMovingVertically();
    }

    protected override void UseAbilityEffect(Vector3 mousePosition, bool isPressed, bool forceAbility = false)
    {
        if (this.isPressed != isPressed && !IsOnCooldown)
        {
            player.PlayerMovementManager.ChangeHorizontalSpeed(isPressed ? horizontalSpeedPercentOnLeftClickActive : 1);
            player.PlayerMovementManager.SetCanJump(!isPressed);
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
            StartCoroutine(ShootOnNetwork());
        }
    }

    private IEnumerator ShootOnNetwork()
    {
        cooldownRemaining = cooldown;

        yield return null;

        while (cooldownRemaining > cooldown * 0.5f)
        {
            cooldownRemaining -= Time.deltaTime;

            yield return null;
        }

        ShootProjectile();
        isPressed = false;
        player.PlayerMovementManager.ChangeHorizontalSpeed(isPressed ? horizontalSpeedPercentOnLeftClickActive : 1);
        player.PlayerMovementManager.SetCanJump(!isPressed);
    }

    public override void ChangeType(int magic)
    {
        if (magic == (int)MageMagic.Classic)
        {
            selectedMagic = MageMagic.Classic;

        }
        else if (magic == (int)MageMagic.Fire)
        {
            selectedMagic = MageMagic.Fire;

        }
        else if (magic == (int)MageMagic.Ice)
        {
            selectedMagic = MageMagic.Ice;

        }
        else
        {
            selectedMagic = MageMagic.Light;

        }
    }

    private void Update()
    {
        if (player.PhotonView.isMine)
        {
            if (isPressed && !IsOnCooldown)
            {
                StartCooldown();
            }
            else if (IsOnCooldown && readyToShoot)
            {
                readyToShoot = false;
                ShootProjectile();
                isPressed = false;
                player.PlayerMovementManager.ChangeHorizontalSpeed(isPressed ? horizontalSpeedPercentOnLeftClickActive : 1);
                player.PlayerMovementManager.SetCanJump(!isPressed);
            }
        }
    }

    protected override IEnumerator PutAbilityOffCooldown()
    {
        IsOnCooldown = true;
        cooldownRemaining = cooldown;
        canShootInCycle = true;

        yield return null;

        //player.AbilityUIManager.SetAbilityOnCooldown(AbilityCategory, ID, IsBlocked);

        while (cooldownRemaining > 0)
        {
            cooldownRemaining -= Time.deltaTime;
            if (canShootInCycle && cooldownRemaining <= cooldown * 0.5f)
            {
                canShootInCycle = false;
                readyToShoot = true;
            }

            //player.AbilityUIManager.UpdateAbilityCooldown(AbilityCategory, ID, cooldownOnStart, cooldownRemaining);

            yield return null;
        }

        //player.AbilityUIManager.SetAbilityOffCooldown(AbilityCategory, ID, UsesResource, IsEnabled, IsBlocked);
        IsOnCooldown = false;
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
            return projectilePrefab1;
        }
        else if (selectedMagic == MageMagic.Fire)
        {
            return projectilePrefab2;
        }
        else if (selectedMagic == MageMagic.Ice)
        {
            return projectilePrefab3;
        }
        else
        {
            return projectilePrefab4;
        }
    }

    private float GetDamage(int magic)
    {
        if (magic == (int)MageMagic.Classic)
        {
            return classicDamage;
        }
        else if (magic == (int)MageMagic.Fire)
        {
            return fireDamage;
        }
        else if (magic == (int)MageMagic.Ice)
        {
            return iceDamage;
        }
        else
        {
            return lightDamage;
        }
    }

    private float GetHeal(int magic)
    {
        if (magic == (int)MageMagic.Classic)
        {
            return classicHeal;
        }
        else if (magic == (int)MageMagic.Fire)
        {
            return fireHeal;
        }
        else if (magic == (int)MageMagic.Ice)
        {
            return iceHeal;
        }
        else
        {
            return lightHeal;
        }
    }

    private void OnProjectileHit(Projectile projectile, int magic, GameObject targetHit)
    {
        if (player.PhotonView.isMine)
        {
            if (targetHit.tag == "Player")
            {
                targetHit.GetComponent<Health>().Restore(GetHeal(magic));
            }
            else if (targetHit.tag == "Enemy")
            {
                targetHit.GetComponent<Health>().Reduce(GetDamage(magic) * damageAmplification);
            }
        }
        if (targetHit.tag == "Enemy" && magic == (int)MageMagic.Ice)
        {
            targetHit.GetComponent<EnemyMovementManager>().SetSlow(this, iceSlowDuration, iceSlowPercent);
        }
    }

    private void OnProjectileReachedEnd(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }
}
