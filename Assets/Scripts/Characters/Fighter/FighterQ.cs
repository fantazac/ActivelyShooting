using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterQ : Ability
{
    private float duration;
    private float durationRemaining;

    private float damageReduction;

    private FighterLeftClick fighterLeftClick;
    private FighterMovementManager fighterMovementManager;

    private FighterQ()
    {
        usesLeft = 1;

        duration = 10;

        damageReduction = 0.8f;

        baseCooldown = 45;
        cooldown = baseCooldown;
    }

    protected override void Awake()
    {
        base.Awake();

        cooldownReduction = 0.5f;
        damageAmplification = 1.5f;
    }

    protected override void Start()
    {
        fighterLeftClick = GetComponent<FighterLeftClick>();
        fighterMovementManager = GetComponent<FighterMovementManager>();
    }

    protected override void UseAbilityEffect(Vector3 mousePosition, bool isPressed, bool forceAbility = false)
    {
        player.PlayerAbilityManager.SetAbilityActive(3, false);
        fighterLeftClick.SetFighterQIsActive(true);
        fighterMovementManager.SetFighterQIsActive(true);
        player.SetDamageReduction(damageReduction);
        StartCoroutine(EndBuff());
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

        player.PlayerAbilityManager.SetAbilityActive(3, true);
        fighterLeftClick.SetFighterQIsActive(false);
        fighterMovementManager.SetFighterQIsActive(false);
        player.SetDamageReduction(-damageReduction);
    }
}
