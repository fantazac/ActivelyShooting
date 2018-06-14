using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundHitboxManager : MonoBehaviour
{
    public delegate void OnTouchesFlyingPlatformOrGroundHandler(GameObject platform, float objectYPosition);
    public event OnTouchesFlyingPlatformOrGroundHandler OnTouchesFlyingPlatformOrGround;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (OnTouchesFlyingPlatformOrGround != null &&
            (collider.gameObject.tag == "FlyingPlatform" || collider.gameObject.tag == "MapFloor" || collider.gameObject.tag == "Wall" || collider.gameObject.tag == "Platform"))
        {
            OnTouchesFlyingPlatformOrGround(collider.gameObject.tag == "FlyingPlatform" ? collider.gameObject : null, collider.transform.position.y + (collider.transform.localScale.y * 0.5f));
        }
    }
}
