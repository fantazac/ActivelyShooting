using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyMovementManager EnemyMovementManager { get; protected set; }

    private Enemy()
    {
        maxHealth = 1000;
    }

    protected override void Awake()
    {
        base.Awake();

        EnemyMovementManager = gameObject.AddComponent<EnemyMovementManager>();
    }
}
