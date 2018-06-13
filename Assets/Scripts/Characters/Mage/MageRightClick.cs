using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageRightClick : Ability
{
    private float damageReduction;

    private float duration;
    private float durationRemaining;

    private MageRightClick()
    {
        damageReduction = 0.5f;

        duration = 5;

        baseCooldown = 15;
        cooldown = baseCooldown;
    }

    protected override void UseAbilityEffect(Vector3 mousePosition, bool isPressed, bool forceAbility = false)
    {
        foreach (Player p in player.Party)
        {
            p.SetDamageReduction(damageReduction);
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

        foreach (Player p in player.Party)
        {
            p.SetDamageReduction(-damageReduction);
        }
    }
}
