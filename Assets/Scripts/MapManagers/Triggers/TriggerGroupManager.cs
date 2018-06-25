using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGroupManager : MonoBehaviour
{
    private TriggerManager[] triggers;

    private int triggersPressedCount;
    private int triggersCount;

    [SerializeField]
    private bool disableOnPressed;

    public int ID { get; set; }

    public delegate void OnTriggerGroupPressedHandler(bool activated, int id);
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
        if (OnTriggerGroupPressed != null)
        {
            if (triggersPressedCount == triggersCount)
            {
                OnTriggerGroupPressed(true, ID);
            }
            else if (!isPressed && triggersPressedCount + 1 == triggersCount)
            {
                OnTriggerGroupPressed(false, ID);
            }
        }
    }

    public bool GetDisableOnPressed()
    {
        return disableOnPressed;
    }
}
