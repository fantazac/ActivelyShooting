using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefenseLevelManager : MonoBehaviour
{
    private Player player;

    [SerializeField]
    private TriggerGroupManager nextLevelTriggerGroupManager;
    [SerializeField]
    private TriggerGroupManager spawnEnemiesTriggerGroupManager;

    [SerializeField]
    private GameObject spawners;

    private List<SpawnerManager> spawnerManagers;

    [SerializeField]
    private GameObject tower;

    private Health towerHealth;

    [SerializeField]
    private WallManager nextLevelWallManager;

    private List<Enemy> enemiesSpawned;

    private int spawnersActiveCount;

    private DefenseLevelManager()
    {
        enemiesSpawned = new List<Enemy>();
    }

    private void Awake()
    {
        nextLevelTriggerGroupManager.OnTriggerGroupPressed += OnNextLevelTriggerGroupPressed;
        spawnEnemiesTriggerGroupManager.OnTriggerGroupPressed += OnSpawnEnemiesTriggerGroupPressed;

        player = StaticObjects.Player;
        player.OnPartyUpdated += OnPartyUpdated;
        OnPartyUpdated();

        spawnerManagers = spawners.GetComponentsInChildren<SpawnerManager>().ToList();
    }

    private void OnPartyUpdated()
    {
        foreach (Player p in player.Party)
        {
            p.PlayerLevelInfoTransmitter.OnChangeEnemyHealthInfoReceived += ChangeEnemyHealthFromServer;
            p.PlayerLevelInfoTransmitter.OnChangeTowerHealthInfoReceived += ChangeTowerHealthFromServer;
        }
    }

    private void OnNextLevelTriggerGroupPressed()
    {
        nextLevelTriggerGroupManager.gameObject.SetActive(false);
        nextLevelWallManager.OpenWall();
    }

    private void OnSpawnEnemiesTriggerGroupPressed()
    {
        spawnEnemiesTriggerGroupManager.gameObject.SetActive(false);
        tower.SetActive(true);
        towerHealth = tower.GetComponent<Health>();
        towerHealth.OnHealthChanged += OnTowerHealthChanged;

        for (int i = 0; i < spawnerManagers.Count; i++)
        {
            SpawnerManager spawnerManager = spawnerManagers[i];
            spawnerManager.StartSpawning(i);
            spawnersActiveCount++;
            spawnerManager.OnEnemySpawned += OnEnemySpawned;
            spawnerManager.OnAllEnemiesSpawned += OnAllEnemiesSpawned;
        }
    }

    private void OnEnemySpawned(Enemy enemy)
    {
        enemiesSpawned.Add(enemy);
        enemy.GetComponent<Health>().OnHealthChanged += OnEnemyHealthChanged;
    }

    private void OnAllEnemiesSpawned(SpawnerManager spawnerManager)
    {
        if (--spawnersActiveCount == 0 && enemiesSpawned.Count == 0)
        {
            LevelComplete();
        }
    }

    private void OnEnemyHealthChanged(int id, bool reduce, float amount, bool isPercent)
    {
        ChangeEnemyHealth(id, reduce, amount, isPercent, false);
        player.PlayerLevelInfoTransmitter.ChangeEnemyHealthOnServer(id, reduce, amount, isPercent);
    }

    private void ChangeEnemyHealthFromServer(int id, bool reduce, float amount, bool isPercent)
    {
        ChangeEnemyHealth(id, reduce, amount, isPercent, true);
    }

    private void ChangeEnemyHealth(int id, bool reduce, float amount, bool isPercent, bool changeHealth)
    {
        Enemy enemyToChangeHealth = null;
        foreach (Enemy enemy in enemiesSpawned)
        {
            if (enemy.ID == id)
            {
                enemyToChangeHealth = enemy;
                if (changeHealth)
                {
                    if (reduce)
                    {
                        enemy.Health.ReduceFromServer(amount);
                    }
                    else
                    {
                        enemy.Health.RestoreFromServer(amount, isPercent);
                    }
                }
                break;
            }
        }
        if (enemyToChangeHealth && enemyToChangeHealth.Health.GetCurrentHealth() <= 0)
        {
            enemiesSpawned.Remove(enemyToChangeHealth);
            Destroy(enemyToChangeHealth.gameObject);
            if (spawnersActiveCount == 0 && enemiesSpawned.Count == 0)
            {
                LevelComplete();
            }
        }
    }

    private void OnTowerHealthChanged(int id, bool reduce, float amount, bool isPercent)
    {
        ChangeTowerHealth(reduce, amount, isPercent);
        player.PlayerLevelInfoTransmitter.ChangeTowerHealthOnServer(reduce, amount, isPercent);
    }

    private void ChangeTowerHealthFromServer(bool reduce, float amount, bool isPercent)
    {
        ChangeTowerHealth(reduce, amount, isPercent);
    }

    private void ChangeTowerHealth(bool reduce, float amount, bool isPercent)
    {
        if (reduce)
        {
            towerHealth.ReduceFromServer(amount);
        }
        else
        {
            towerHealth.RestoreFromServer(amount, isPercent);
        }
        if (towerHealth.GetCurrentHealth() <= 0)
        {
            Destroy(tower);
            LevelFail();
        }
    }

    private void LevelComplete()
    {
        nextLevelTriggerGroupManager.gameObject.SetActive(true);
    }

    private void LevelFail()
    {
        Debug.Log("You suck lmao");
    }
}
