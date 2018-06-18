using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private int pressedCount;

    public delegate void OnPressedTriggerHandler(bool isPressed);
    public event OnPressedTriggerHandler OnPressedTrigger;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (++pressedCount == 1)
            {
                OnPressedTrigger(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (--pressedCount == 0)
            {
                OnPressedTrigger(false);
            }
        }
    }

    private void Update()
    {
        if (pressedCount > 0 && spriteRenderer.enabled)
        {
            spriteRenderer.enabled = false;
        }
        else if (pressedCount == 0 && !spriteRenderer.enabled)
        {
            spriteRenderer.enabled = true;
        }
    }
}
