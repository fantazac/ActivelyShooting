﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStopMovementTriggerManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            collider.GetComponent<EnemyMovementManager>().StopMovementFromTrigger();
        }
    }
}
