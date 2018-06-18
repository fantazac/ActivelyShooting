using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoRightTriggerManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            collider.GetComponent<EnemyMovementManager>().GoRightFromTrigger();
        }
    }
}
