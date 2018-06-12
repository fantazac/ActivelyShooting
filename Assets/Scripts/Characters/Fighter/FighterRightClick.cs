using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterRightClick : Ability
{
    protected string projectilePrefabPath;
    protected GameObject projectilePrefab;

    private float speed;
    private float range;
    private float damage;

    private float damageReduction;

    private float duration;
    private float durationRemaining;

    private FighterMode selectedMode;

    private FighterRightClick()
    {
        speed = 13;
        range = 25;
        damage = 40;

        baseCooldown = 8;
        cooldown = baseCooldown;

        damageReduction = 0.8f;
        duration = 2;

        ChangeType((int)FighterMode.Swordsman);

        projectilePrefabPath = "ProjectilePrefabs/FighterSpecialAttack";
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

    public override void ChangeType(int mode)
    {
        selectedMode = (FighterMode)mode;
    }

    protected override void UseAbilityEffect(Vector3 mousePosition, bool isPressed)
    {
        if (selectedMode == FighterMode.Swordsman)
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
        else
        {
            player.SetDamageReduction(damageReduction);
            StartCoroutine(EndBuff());
        }
    }

    private IEnumerator EndBuff()
    {
        durationRemaining = duration;

        yield return null;

        while (durationRemaining > 0)
        {
            durationRemaining -= Time.deltaTime;

            yield return null;
        }

        player.SetDamageReduction(-damageReduction);
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
