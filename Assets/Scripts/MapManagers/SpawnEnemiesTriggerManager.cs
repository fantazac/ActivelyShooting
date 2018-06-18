using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesTriggerManager : MonoBehaviour
{
    [SerializeField]
    private SpawnerManager spawnerManager;
    [SerializeField]
    private GameObject tower;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            tower.SetActive(true);
            spawnerManager.StartSpawning(collider.GetComponent<Player>() == StaticObjects.Player);
        }
    }
}
