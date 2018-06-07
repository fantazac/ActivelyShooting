using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerE : Ability
{
    private float duration;
    private float durationRemaining;

    private GunnerLeftClick gunnerLeftClick;

    private GunnerE()
    {
        duration = 5;

        baseCooldown = 20;
        cooldown = baseCooldown;
    }

    protected override void Start()
    {
        base.Start();

        gunnerLeftClick = GetComponent<GunnerLeftClick>();
    }

    protected override void UseAbilityEffect(Vector3 mousePosition, bool isPressed)
    {
        gunnerLeftClick.SetAoE(true);
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

        gunnerLeftClick.SetAoE(false);
    }
}
