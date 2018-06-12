using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float maxHealth;

    private Enemy()
    {
        maxHealth = 1000;
    }

    private void Awake()
    {
        gameObject.AddComponent<Health>().SetMaxHealth(maxHealth);
    }
}
