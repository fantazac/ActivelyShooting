using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelTriggerGroupManager : MonoBehaviour
{
    [SerializeField]
    private WallManager wallManager;

    private TriggerManager[] triggers;

    private int triggersPressedCount;
    private int triggersCount;

    private void Awake()
    {
        triggers = GetComponentsInChildren<TriggerManager>();
        triggersCount = triggers.Length;

        foreach (TriggerManager trigger in triggers)
        {
            trigger.OnPressedTrigger += OnPressedTrigger;
        }
    }

    private void OnPressedTrigger(TriggerManager trigger, bool isPressed, GameObject player)
    {
        triggersPressedCount += isPressed ? 1 : -1;
        if (triggersPressedCount == triggersCount)
        {
            wallManager.OpenWall();
            gameObject.SetActive(false);
        }
    }
}
