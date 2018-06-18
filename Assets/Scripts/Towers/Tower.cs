using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Entity
{
    private Tower()
    {
        maxHealth = 1000;
    }

    protected override void Start()
    {
        base.Start();

        if (StaticObjects.Player && StaticObjects.Player.SpawnedMap)
        {
            Health.OnHealthChanged += OnHealthChanged;
        }
    }

    private void OnHealthChanged()
    {
        if (Health.IsDead())
        {
            //game over on network
        }
    }
}
