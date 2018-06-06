using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected Player player;

    protected float cooldown;
    protected float cooldownRemaining;

    public bool IsOnCooldown { get; protected set; }
    public bool IsHoldDownAbility { get; protected set; }

    protected virtual void Awake()
    {
        player = GetComponent<Player>();
    }

    protected virtual void Start() { }

    public virtual void UseAbility(Vector3 mousePosition, bool isPressed)
    {
        if (!IsHoldDownAbility || isPressed)
        {
            if (player.PhotonView.isMine)
            {
                StartCoroutine(PutAbilityOffCooldown());
            }
            UseAbilityEffect(mousePosition, isPressed);
        }
    }

    public virtual void UseAbilityOnNetwork(Vector3 mousePosition, bool isPressed)
    {
        UseAbility(mousePosition, isPressed);
    }

    protected abstract void UseAbilityEffect(Vector3 mousePosition, bool isPressed);

    public virtual void ChangeWeapon(int weapon) { }

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
}
