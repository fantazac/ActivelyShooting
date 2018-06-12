using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Player
{
    private Fighter()
    {
        maxHealth = 300;
    }

    protected override void Awake()
    {
        base.Awake();

        PlayerAbilityManager = gameObject.AddComponent<FighterAbilityManager>();
    }
}
