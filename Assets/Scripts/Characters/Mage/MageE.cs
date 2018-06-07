using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageE : Ability
{
    private float duration;
    private float durationRemaining;

    private MageLeftClick mageLeftClick;

    private MageE()
    {
        duration = 5;

        baseCooldown = 20;
        cooldown = baseCooldown;
    }

    protected override void Start()
    {
        base.Start();

        mageLeftClick = GetComponent<MageLeftClick>();
    }

    protected override void UseAbilityEffect(Vector3 mousePosition, bool isPressed)
    {
        mageLeftClick.SetAoE(true);
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

        mageLeftClick.SetAoE(false);
    }
}
