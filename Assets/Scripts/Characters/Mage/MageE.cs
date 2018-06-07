using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageE : Ability
{
    private MageE()
    {
        baseCooldown = 4;
        cooldown = baseCooldown;
    }

    protected override void UseAbilityEffect(Vector3 mousePosition, bool isPressed)
    {
        RaycastHit2D[] raycasts = Physics2D.RaycastAll(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Structures"));
        Vector3 newPosition = mousePosition;
        foreach (RaycastHit2D raycast in raycasts)
        {
            Collider2D collider = raycast.collider;
            if (collider.gameObject.tag == "Platform" || collider.gameObject.tag == "Floor")
            {
                newPosition = new Vector2(newPosition.x, collider.gameObject.transform.position.y + (collider.gameObject.transform.localScale.y + transform.localScale.y) * 0.5f);
            }
            else if (collider.gameObject.tag == "Ceiling")
            {
                newPosition = new Vector2(newPosition.x, collider.gameObject.transform.position.y - (collider.gameObject.transform.localScale.y + transform.localScale.y) * 0.5f);
            }
            else if (collider.gameObject.tag == "Wall")
            {
                if (transform.position.x > collider.gameObject.transform.position.x)
                {
                    newPosition = new Vector2(collider.gameObject.transform.position.x + (collider.gameObject.transform.localScale.x + transform.localScale.x) * 0.5f, newPosition.y);
                }
                else
                {
                    newPosition = new Vector2(collider.gameObject.transform.position.x - (collider.gameObject.transform.localScale.x + transform.localScale.x) * 0.5f, newPosition.y);
                }
            }
        }
        transform.position = newPosition;
    }
}
