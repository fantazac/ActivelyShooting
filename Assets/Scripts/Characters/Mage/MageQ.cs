using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageQ : Ability
{
    private float duration;
    private float durationRemaining;

    private MageQ()
    {
        usesLeft = 1;

        duration = 10;

        baseCooldown = 45;
        cooldown = baseCooldown;
    }

    protected override void Awake()
    {
        base.Awake();

        cooldownReduction = 0.5f;
        damageAmplification = 1.5f;
    }

    protected override void UseAbilityEffect(Vector3 mousePosition, bool isPressed)
    {
        player.PlayerAbilityManager.SetDamageAmplificationForAbilities(damageAmplification);
        foreach (Player p in player.Party)
        {
            p.PlayerAbilityManager.SetCooldownReductionForAbilities(cooldownReduction);
        }
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

        player.PlayerAbilityManager.SetDamageAmplificationForAbilities(1);
        foreach (Player p in player.Party)
        {
            p.PlayerAbilityManager.SetCooldownReductionForAbilities(1);
        }
    }
}
