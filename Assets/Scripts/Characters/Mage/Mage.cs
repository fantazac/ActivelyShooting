using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Player
{
    private Mage()
    {
        maxHealth = 150;
    }

    protected override void Awake()
    {
        base.Awake();

        PlayerAbilityManager = gameObject.AddComponent<MageAbilityManager>();
        PlayerMovementManager = gameObject.AddComponent<PlayerMovementManager>();
    }
}
