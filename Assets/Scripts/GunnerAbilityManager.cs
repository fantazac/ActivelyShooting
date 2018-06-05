using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerAbilityManager : PlayerAbilityManager
{
    protected override void SwitchWeapon()
    {

    }

    protected override void UseBasicAttack(Vector3 position, bool isPressed)
    {

    }

    protected override void UseEAbility()
    {

    }

    protected override void UseQAbility()
    {
        Debug.Log(player.Party.Length);
    }

    protected override void UseSpecialAttack(Vector3 position)
    {

    }
}
