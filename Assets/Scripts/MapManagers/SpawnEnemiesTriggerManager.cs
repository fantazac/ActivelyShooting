using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesTriggerManager : MonoBehaviour
{
    [SerializeField]
    private SpawnerManager spawnerManager;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            if (collider.GetComponent<Player>() == StaticObjects.Player)
            {
                spawnerManager.StartSpawning();
            }
        }
    }
}
