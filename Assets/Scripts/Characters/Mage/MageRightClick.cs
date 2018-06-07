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

    protected override void UseAbilityEffect(Vector3 mousePosition, bool isPressed)
    {
        foreach (Player p in player.Party)
        {
            //Apply damage reduction to player in stats
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
            //Remove damage reduction to player in stats
        }
    }
}
