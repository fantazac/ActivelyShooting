using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Player
{
    protected override void Awake()
    {
        base.Awake();

        PlayerAbilityManager = gameObject.AddComponent<FighterAbilityManager>();
    }
}
