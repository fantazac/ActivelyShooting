using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageQ : Ability
{
    private float duration;
    private float durationRemaining;

    private float damageReduction;
    private float healPercentOnCast;

    private MageQ()
    {
        usesLeft = 1;

        duration = 10;

        damageReduction = 1;
        healPercentOnCast = 100;

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
        foreach (Player p in player.Party)
        {
            p.Health.Restore(healPercentOnCast, true);
        }
        player.SetDamageReduction(damageReduction);
        player.PlayerMovementManager.SetCanMove(false);
        player.PlayerAbilityManager.SetAbilityActive(1, false);
        player.PlayerAbilityManager.SetAbilityActive(2, false);
        player.PlayerAbilityManager.SetAbilityActive(3, false);
        player.PlayerAbilityManager.UseAbilityWithoutLimitation(1, new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
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

        player.PlayerAbilityManager.SetAbilityActive(1, true);
        player.PlayerAbilityManager.SetAbilityActive(2, true);
        player.PlayerAbilityManager.SetAbilityActive(3, true);
        player.PlayerMovementManager.SetCanMove(true);
        player.SetDamageReduction(-damageReduction);
    }
}
