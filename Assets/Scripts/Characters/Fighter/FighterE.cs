using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterE : Ability
{
    private float duration;
    private float durationRemaining;

    private FighterLeftClick fighterLeftClick;

    private FighterE()
    {
        duration = 5;

        baseCooldown = 20;
        cooldown = baseCooldown;
    }

    protected override void Start()
    {
        base.Start();

        fighterLeftClick = GetComponent<FighterLeftClick>();
    }

    protected override void UseAbilityEffect(Vector3 mousePosition, bool isPressed)
    {
        fighterLeftClick.SetAoE(true);
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

        fighterLeftClick.SetAoE(false);
    }
}
