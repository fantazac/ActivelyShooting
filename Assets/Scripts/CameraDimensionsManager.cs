using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDimensionsManager : MonoBehaviour
{
    private void Start()
    {
        if (StaticObjects.PlayerCamera)
        {
            StaticObjects.PlayerCamera.GetComponent<CameraManager>().AddNewCameraDimensions(gameObject);
        }
    }
}
