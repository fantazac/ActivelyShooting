using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGroupManager : MonoBehaviour
{
    private TriggerManager[] triggers;

    private int triggersPressedCount;
    private int triggersCount;

    public delegate void OnTriggerGroupPressedHandler();
    public event OnTriggerGroupPressedHandler OnTriggerGroupPressed;

    private void Awake()
    {
        triggers = GetComponentsInChildren<TriggerManager>();
        triggersCount = triggers.Length;

        foreach (TriggerManager trigger in triggers)
        {
            trigger.OnPressedTrigger += OnPressedTrigger;
        }
    }

    private void OnPressedTrigger(bool isPressed)
    {
        triggersPressedCount += isPressed ? 1 : -1;
        if (triggersPressedCount == triggersCount)
        {
            OnTriggerGroupPressed();
        }
    }
}
