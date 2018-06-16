using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject frontOfSpawner;
    [SerializeField]
    private int numberOfEnemiesToSpawn;
    [SerializeField]
    private float timeBeforeFirstEnemySpawn;
    [SerializeField]
    private float timeBetweenEnemySpawns;
    [SerializeField]
    private bool enemiesGoLeftOnSpawn;
    [SerializeField]
    private bool enemiesGoRightOnSpawn;
    [SerializeField]
    private bool enemiesJumpOnSpawn;
    [SerializeField]
    private Vector2 towerPosition;

    private WaitForSeconds delayBeforeFirstEnemySpawn;
    private WaitForSeconds delayBetweenEnemySpawns;

    private void Awake()
    {
        delayBeforeFirstEnemySpawn = new WaitForSeconds(timeBeforeFirstEnemySpawn);
        delayBetweenEnemySpawns = new WaitForSeconds(timeBetweenEnemySpawns);
    }

    public void StartSpawning()
    {
        frontOfSpawner.SetActive(true);
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        yield return delayBeforeFirstEnemySpawn;

        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            string enemyType = "Enemy";
            if (enemiesJumpOnSpawn)
            {
                if (enemiesGoLeftOnSpawn)
                {
                    enemyType = "Enemy_JumpLeft";
                }
                else if (enemiesGoRightOnSpawn)
                {
                    enemyType = "Enemy_JumpRight";
                }
                else
                {
                    enemyType = "Enemy_Jump";
                }
            }
            else if (enemiesGoLeftOnSpawn)
            {
                enemyType = "Enemy_Left";
            }
            else if (enemiesGoRightOnSpawn)
            {
                enemyType = "Enemy_Right";
            }
            PhotonNetwork.Instantiate(enemyType, transform.position, Quaternion.identity, 0);

            yield return delayBetweenEnemySpawns;
        }

        frontOfSpawner.SetActive(false);
    }
}
