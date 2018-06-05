﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundHitboxManager : MonoBehaviour
{
    public delegate void OnTouchesPlatformOrFloorHandler(GameObject platform, float objectYPosition);
    public event OnTouchesPlatformOrFloorHandler OnTouchesPlatformOrFloor;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (OnTouchesPlatformOrFloor != null && (collider.gameObject.tag == "Platform" || collider.gameObject.tag == "Floor"))
        {
            OnTouchesPlatformOrFloor(collider.gameObject.tag == "Platform" ? collider.gameObject : null, collider.transform.position.y + (collider.transform.localScale.y * 0.5f));
        }
    }
}
