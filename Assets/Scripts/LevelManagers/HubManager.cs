using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    [SerializeField]
    private TriggerGroupManager nextLevelTriggerGroupManager;
    [SerializeField]
    private TriggerGroupManager spawnLevelsTriggerGroupManager;

    [SerializeField]
    private WallManager nextLevelWallManager;

    private List<GameObject> maps;

    private List<GameObject> availableMaps;

    private HubManager()
    {
        maps = new List<GameObject>();
        availableMaps = new List<GameObject>();
    }

    private void Awake()
    {
        nextLevelTriggerGroupManager.OnTriggerGroupPressed += OnNextLevelTriggerGroupPressed;
        spawnLevelsTriggerGroupManager.OnTriggerGroupPressed += OnSpawnLevelsTriggerGroupPressed;

        LoadPrefabs();
    }

    private void LoadPrefabs()
    {
        availableMaps.Add(Resources.Load<GameObject>("LevelPrefabs/Defense/Level_Defense_1_1"));
        availableMaps.Add(Resources.Load<GameObject>("LevelPrefabs/Defense/Level_Defense_1_2"));
        availableMaps.Add(Resources.Load<GameObject>("LevelPrefabs/Defense/Level_Defense_1_3"));

        availableMaps.Add(Resources.Load<GameObject>("LevelPrefabs/Puzzle/Level_Puzzle_1_1"));
    }

    private void OnNextLevelTriggerGroupPressed(bool activated, int id)
    {
        nextLevelTriggerGroupManager.gameObject.SetActive(false);
        nextLevelWallManager.OpenWall();
    }

    private void OnSpawnLevelsTriggerGroupPressed(bool activated, int id)
    {
        spawnLevelsTriggerGroupManager.gameObject.SetActive(false);
        nextLevelTriggerGroupManager.gameObject.SetActive(true);
        if (maps.Count > 0)
        {
            foreach (GameObject map in maps)
            {
                Destroy(map);
            }
            maps = new List<GameObject>();
        }
        for (int i = 1; i <= 10; i++)
        {
            maps.Add(Instantiate(GetAvailableMap(), new Vector2(i * 46, 0), Quaternion.identity));
        }
    }

    private GameObject GetAvailableMap()
    {
        return availableMaps[Random.Range(0, availableMaps.Count)];
    }
}
