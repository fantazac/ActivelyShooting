using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField]
    private GameObject nextPortal;

    public void TeleportEntityToNextPortal(GameObject entity)
    {
        entity.transform.position = nextPortal.transform.position;
    }
}
