using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Player
{
    protected override void Awake()
    {
        base.Awake();

        PlayerAbilityManager = gameObject.AddComponent<MageAbilityManager>();
    }
}
