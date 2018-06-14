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

    protected override void UseAbilityEffect(Vector3 mousePosition, bool isPressed, bool forceAbility = false)
    {
        player.PlayerHitbox.isTrigger = false;
        RaycastHit2D[] raycasts = Physics2D.RaycastAll(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Structures"));
        Vector3 newPosition = mousePosition;
        foreach (RaycastHit2D raycast in raycasts)
        {
            Collider2D collider = raycast.collider;
            if (collider.gameObject.tag == "FlyingPlatform" || collider.gameObject.tag == "MapFloor")
            {
                newPosition = new Vector2(newPosition.x, collider.gameObject.transform.position.y + (collider.gameObject.transform.localScale.y + transform.localScale.y) * 0.5f);
            }
            else if (collider.gameObject.tag == "MapCeiling")
            {
                newPosition = new Vector2(newPosition.x, collider.gameObject.transform.position.y - (collider.gameObject.transform.localScale.y + transform.localScale.y) * 0.5f);
            }
            else if (collider.gameObject.tag == "MapWall")
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
            else if (collider.gameObject.tag == "Platform")
            {
                if (newPosition.y < collider.gameObject.transform.position.y)
                {
                    newPosition = new Vector2(newPosition.x, collider.gameObject.transform.position.y - (collider.gameObject.transform.localScale.y + transform.localScale.y) * 0.5f);
                }
                else
                {
                    newPosition = new Vector2(newPosition.x, collider.gameObject.transform.position.y + (collider.gameObject.transform.localScale.y + transform.localScale.y) * 0.5f);
                }
            }
            else if (collider.gameObject.tag == "Wall")
            {
                if (newPosition.x < collider.gameObject.transform.position.x)
                {
                    newPosition = new Vector2(collider.gameObject.transform.position.x - (collider.gameObject.transform.localScale.x + transform.localScale.x) * 0.5f, newPosition.y);
                }
                else
                {
                    newPosition = new Vector2(collider.gameObject.transform.position.x + (collider.gameObject.transform.localScale.x + transform.localScale.x) * 0.5f, newPosition.y);
                }
            }
        }
        transform.position = newPosition;
    }
}
