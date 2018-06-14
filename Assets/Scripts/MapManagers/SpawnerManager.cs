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
    private bool goLeft;
    [SerializeField]
    private bool goRight;
    [SerializeField]
    private bool jumpOnSpawn;

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
            EnemyMovementManager emm = PhotonNetwork.Instantiate("Enemy", transform.position, Quaternion.identity, 0).GetComponent<EnemyMovementManager>();
            if (goLeft)
            {
                emm.GoLeftFromTrigger();
            }
            else if (goRight)
            {
                emm.GoRightFromTrigger();
            }
            if (jumpOnSpawn)
            {
                emm.JumpFromTrigger();
            }

            yield return delayBetweenEnemySpawns;
        }

        frontOfSpawner.SetActive(false);
    }
}
