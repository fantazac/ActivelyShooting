using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelTriggerGroupManager : MonoBehaviour
{
    private TriggerManager[] triggers;

    private int triggersPressedCount;
    private int triggersCount;

    public delegate void OnPressedTriggersHandler();
    public event OnPressedTriggersHandler OnPressedTriggers;

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
        if(triggersPressedCount == triggersCount)
        {
            if (OnPressedTriggers != null)
            {
                OnPressedTriggers();
            }
            gameObject.SetActive(false);
        }
    }

    private void OnPressedTrigger(TriggerManager trigger, bool isPressed, GameObject player)
    {
        triggersPressedCount += isPressed ? 1 : -1;
    }
}
