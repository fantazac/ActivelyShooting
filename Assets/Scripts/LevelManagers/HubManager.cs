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

    private GameObject defense_1_1_Prefab;
    private GameObject defense_1_2_Prefab;
    private GameObject defense_1_3_Prefab;

    private string defense_1_1_PrefabPath;
    private string defense_1_2_PrefabPath;
    private string defense_1_3_PrefabPath;

    private HubManager()
    {
        maps = new List<GameObject>();
        availableMaps = new List<GameObject>();

        defense_1_1_PrefabPath = "LevelPrefabs/Defense/Level_Defense_1_1";
        defense_1_2_PrefabPath = "LevelPrefabs/Defense/Level_Defense_1_2";
        defense_1_3_PrefabPath = "LevelPrefabs/Defense/Level_Defense_1_3";
    }

    private void Awake()
    {
        nextLevelTriggerGroupManager.OnTriggerGroupPressed += OnNextLevelTriggerGroupPressed;
        spawnLevelsTriggerGroupManager.OnTriggerGroupPressed += OnSpawnLevelsTriggerGroupPressed;

        LoadPrefabs();
    }

    private void LoadPrefabs()
    {
        defense_1_1_Prefab = Resources.Load<GameObject>(defense_1_1_PrefabPath);
        defense_1_2_Prefab = Resources.Load<GameObject>(defense_1_2_PrefabPath);
        defense_1_3_Prefab = Resources.Load<GameObject>(defense_1_3_PrefabPath);

        availableMaps.Add(defense_1_1_Prefab);
        availableMaps.Add(defense_1_2_Prefab);
        availableMaps.Add(defense_1_3_Prefab);
    }

    private void OnNextLevelTriggerGroupPressed()
    {
        nextLevelTriggerGroupManager.gameObject.SetActive(false);
        nextLevelWallManager.OpenWall();
    }

    private void OnSpawnLevelsTriggerGroupPressed()
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
