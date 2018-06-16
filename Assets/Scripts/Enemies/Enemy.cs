using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField]
    private bool goLeftOnSpawn;
    [SerializeField]
    private bool goRightOnSpawn;
    [SerializeField]
    private bool jumpOnSpawn;

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

    protected override void Start()
    {
        base.Start();

        if (goLeftOnSpawn)
        {
            EnemyMovementManager.GoLeftFromTrigger();
        }
        else if (goRightOnSpawn)
        {
            EnemyMovementManager.GoRightFromTrigger();
        }
        if (jumpOnSpawn)
        {
            EnemyMovementManager.JumpFromTrigger();
        }
    }
}
