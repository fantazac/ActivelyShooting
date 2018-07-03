using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPortalManager : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Player>().PlayerInputManager.OnMoveUp += GoIntoPortal;
    }

    private void GoIntoPortal()
    {
        foreach (Collider2D portal in Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0, LayerMask.GetMask("Portals")))
        {
            portal.GetComponent<PortalManager>().TeleportEntityToNextPortal(gameObject);
            break;
        }
    }
}
