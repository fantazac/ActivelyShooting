using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterE : Ability
{
    private float stunDuration;

    private FighterE()
    {
        stunDuration = 5;

        baseCooldown = 25;
        cooldown = baseCooldown;
    }

    protected override void UseAbilityEffect(Vector3 mousePosition, bool isPressed, bool forceAbility = false)
    {
        foreach (Collider2D collider in Physics2D.OverlapBoxAll(mousePosition, new Vector2(46, 26), 0, LayerMask.GetMask("Enemies")))
        {
            collider.GetComponent<EnemyMovementManager>().SetStun(this, stunDuration);
        }
    }
}
