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

    private WaitForSeconds delayBeforeFirstEnemySpawn;
    private WaitForSeconds delayBetweenEnemySpawns;

    private GameObject enemyPrefab;
    private GameObject enemyJumpPrefab;
    private GameObject enemyJumpLeftPrefab;
    private GameObject enemyJumpRightPrefab;
    private GameObject enemyLeftPrefab;
    private GameObject enemyRightPrefab;

    private string enemyPrefabPath;
    private string enemyJumpPrefabPath;
    private string enemyJumpLeftPrefabPath;
    private string enemyJumpRightPrefabPath;
    private string enemyLeftPrefabPath;
    private string enemyRightPrefabPath;

    public delegate void OnEnemySpawnedHandler(Enemy enemy);
    public event OnEnemySpawnedHandler OnEnemySpawned;

    public delegate void OnAllEnemiesSpawnedHandler(SpawnerManager spawnerManager);
    public event OnAllEnemiesSpawnedHandler OnAllEnemiesSpawned;

    private SpawnerManager()
    {
        enemyPrefabPath = "EnemyPrefabs/Enemy";
        enemyJumpPrefabPath = "EnemyPrefabs/Enemy_Jump";
        enemyJumpLeftPrefabPath = "EnemyPrefabs/Enemy_JumpLeft";
        enemyJumpRightPrefabPath = "EnemyPrefabs/Enemy_JumpRight";
        enemyLeftPrefabPath = "EnemyPrefabs/Enemy_Left";
        enemyRightPrefabPath = "EnemyPrefabs/Enemy_Right";
    }

    private void Awake()
    {
        delayBeforeFirstEnemySpawn = new WaitForSeconds(timeBeforeFirstEnemySpawn);
        delayBetweenEnemySpawns = new WaitForSeconds(timeBetweenEnemySpawns);

        LoadPrefabs();
    }

    private void LoadPrefabs()
    {
        enemyPrefab = Resources.Load<GameObject>(enemyPrefabPath);
        enemyJumpPrefab = Resources.Load<GameObject>(enemyJumpPrefabPath);
        enemyJumpLeftPrefab = Resources.Load<GameObject>(enemyJumpLeftPrefabPath);
        enemyJumpRightPrefab = Resources.Load<GameObject>(enemyJumpRightPrefabPath);
        enemyLeftPrefab = Resources.Load<GameObject>(enemyLeftPrefabPath);
        enemyRightPrefab = Resources.Load<GameObject>(enemyRightPrefabPath);
    }

    public void StartSpawning(int spawnerId)
    {
        StartCoroutine(SpawnEnemies(spawnerId));
    }

    private IEnumerator SpawnEnemies(int spawnerId)
    {
        frontOfSpawner.SetActive(true);
        int firstId = spawnerId * 100;

        yield return delayBeforeFirstEnemySpawn;

        GameObject enemyType;
        if (enemiesJumpOnSpawn)
        {
            if (enemiesGoLeftOnSpawn)
            {
                enemyType = enemyJumpLeftPrefab;
            }
            else if (enemiesGoRightOnSpawn)
            {
                enemyType = enemyJumpRightPrefab;
            }
            else
            {
                enemyType = enemyJumpPrefab;
            }
        }
        else if (enemiesGoLeftOnSpawn)
        {
            enemyType = enemyLeftPrefab;
        }
        else if (enemiesGoRightOnSpawn)
        {
            enemyType = enemyRightPrefab;
        }
        else
        {
            enemyType = enemyPrefab;
        }

        for (int i = firstId; i < firstId + numberOfEnemiesToSpawn; i++)
        {
            Enemy enemy = Instantiate(enemyType, transform.position, Quaternion.identity).GetComponent<Enemy>();
            enemy.ID = i;
            OnEnemySpawned(enemy);

            yield return delayBetweenEnemySpawns;
        }

        OnAllEnemiesSpawned(this);
        frontOfSpawner.SetActive(false);
    }
}
