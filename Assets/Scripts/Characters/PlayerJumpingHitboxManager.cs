using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingHitboxManager : MonoBehaviour
{
    public bool IsTouchingFloorOrPlatform { get; private set; }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Platform" || collider.gameObject.tag == "Floor")
        {
            IsTouchingFloorOrPlatform = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Platform" || collider.gameObject.tag == "Floor")
        {
            IsTouchingFloorOrPlatform = false;
        }
    }
}
