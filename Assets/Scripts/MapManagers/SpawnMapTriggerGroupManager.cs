using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMapTriggerGroupManager : MonoBehaviour
{
    [SerializeField]
    private GameObject nextLevelTriggerGroup;

    private TriggerManager[] triggers;

    private int triggersPressedCount;
    private int triggersCount;

    private GameObject playerToSpawnMap;

    private List<GameObject> maps;

    private SpawnMapTriggerGroupManager()
    {
        maps = new List<GameObject>();
    }

    private void Awake()
    {
        triggers = GetComponentsInChildren<TriggerManager>();
        triggersCount = triggers.Length;

        foreach (TriggerManager trigger in triggers)
        {
            trigger.OnPressedTrigger += OnPressedTrigger;
        }
    }

    private void Update()
    {
        if (triggersPressedCount == triggersCount)
        {
            if (maps.Count > 0)
            {
                foreach (GameObject map in maps)
                {
                    PhotonNetwork.Destroy(map);
                }
                maps = new List<GameObject>();
            }
            nextLevelTriggerGroup.SetActive(true);
            gameObject.SetActive(false);
            if (StaticObjects.Player == playerToSpawnMap.GetComponent<Player>())
            {
                StaticObjects.Player.SpawnedMap = true;
                for (int i = 1; i <= 10; i++)
                {
                    maps.Add(PhotonNetwork.Instantiate("Map_Defense_1_1", new Vector2(i * 46, 0), Quaternion.identity, 0));
                }
            }
        }
    }

    private void OnPressedTrigger(TriggerManager trigger, bool isPressed, GameObject player)
    {
        if (isPressed && trigger == triggers[0])
        {
            playerToSpawnMap = player;
        }
        triggersPressedCount += isPressed ? 1 : -1;
    }
}
