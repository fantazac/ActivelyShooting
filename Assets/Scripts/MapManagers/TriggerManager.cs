using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private int pressedCount;

    public delegate void OnPressedTriggerHandler(TriggerManager trigger, bool isPressed, GameObject player);
    public event OnPressedTriggerHandler OnPressedTrigger;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if(++pressedCount == 1)
            {
                OnPressedTrigger(this, true, collider.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if(--pressedCount == 0)
            {
                OnPressedTrigger(this, false, collider.gameObject);
            }
        }
    }

    private void Update()
    {
        if (pressedCount > 0 && spriteRenderer.enabled)
        {
            spriteRenderer.enabled = false;
        }
        else if(pressedCount == 0 && !spriteRenderer.enabled)
        {
            spriteRenderer.enabled = true;
        }
    }
}
