using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : Player
{
    private Gunner()
    {
        maxHealth = 100;
    }

    protected override void Awake()
    {
        base.Awake();

        PlayerAbilityManager = gameObject.AddComponent<GunnerAbilityManager>();
        PlayerMovementManager = gameObject.AddComponent<GunnerMovementManager>();
    }
}
