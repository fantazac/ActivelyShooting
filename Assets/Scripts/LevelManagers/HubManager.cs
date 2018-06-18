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

    private GameObject defense_1_1_Prefab;

    private string defense_1_1_PrefabPath;

    private HubManager()
    {
        maps = new List<GameObject>();

        defense_1_1_PrefabPath = "LevelPrefabs/Level_Defense_1_1";
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
            maps.Add(Instantiate(defense_1_1_Prefab, new Vector2(i * 46, 0), Quaternion.identity));
        }
    }
}
