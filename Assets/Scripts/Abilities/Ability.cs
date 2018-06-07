using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected Player player;

    protected float baseCooldown;
    protected float cooldown;
    protected float cooldownRemaining;

    protected float cooldownReduction;
    protected float damageAmplification;

    protected int usesLeft;

    protected bool active;

    public bool HasLimitedUsesPerLevel { get; private set; }
    public bool IsOnCooldown { get; protected set; }
    public bool IsHoldDownAbility { get; protected set; }

    protected Ability()
    {
        cooldownReduction = 1;
        damageAmplification = 1;

        active = true;
    }

    protected virtual void Awake()
    {
        player = GetComponent<Player>();

        HasLimitedUsesPerLevel = usesLeft > 0;
    }

    protected virtual void Start() { }

    public virtual void UseAbility(Vector3 mousePosition, bool isPressed)
    {
        if (!IsHoldDownAbility || isPressed)
        {
            if (player.PhotonView.isMine)
            {
                if (HasLimitedUsesPerLevel)
                {
                    usesLeft--;
                }
                if (!HasLimitedUsesPerLevel || usesLeft > 0)
                {
                    StartCooldown();
                }
            }
            UseAbilityEffect(mousePosition, isPressed);
        }
    }

    protected void StartCooldown()
    {
        if (cooldownRemaining > 0)
        {
            cooldownRemaining = cooldown;
        }
        else
        {
            StartCoroutine(PutAbilityOffCooldown());
        }
    }

    public void SetActive(bool active)
    {
        this.active = active;
        if (!active && IsHoldDownAbility)
        {
            UseAbilityEffect(Vector2.zero, false);
        }
    }

    public virtual void UseAbilityOnNetwork(Vector3 mousePosition, bool isPressed)
    {
        UseAbility(mousePosition, isPressed);
    }

    protected abstract void UseAbilityEffect(Vector3 mousePosition, bool isPressed);

    public virtual void ChangeType(int type) { }

    protected IEnumerator PutAbilityOffCooldown()
    {
        IsOnCooldown = true;
        cooldownRemaining = cooldown;

        yield return null;

        //player.AbilityUIManager.SetAbilityOnCooldown(AbilityCategory, ID, IsBlocked);

        while (cooldownRemaining > 0)
        {
            cooldownRemaining -= Time.deltaTime;

            //player.AbilityUIManager.UpdateAbilityCooldown(AbilityCategory, ID, cooldownOnStart, cooldownRemaining);

            yield return null;
        }

        //player.AbilityUIManager.SetAbilityOffCooldown(AbilityCategory, ID, UsesResource, IsEnabled, IsBlocked);
        IsOnCooldown = false;
    }

    public virtual void SetCooldownReduction(float cooldownReduction)
    {
        this.cooldownReduction = cooldownReduction;
        cooldown = baseCooldown * cooldownReduction;
        if (cooldownRemaining > cooldown)
        {
            cooldownRemaining = cooldown;
        }
    }

    public void SetDamageAmplification(float damageAmplification)
    {
        this.damageAmplification = damageAmplification;
    }

    public bool IsAvailable()
    {
        return active && (!HasLimitedUsesPerLevel || usesLeft > 0);
    }
}
