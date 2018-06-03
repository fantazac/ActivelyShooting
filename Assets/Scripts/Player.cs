using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PhotonView PhotonView { get; private set; }

    public PlayerMovement PlayerMovement { get; private set; }
    public PlayerInputManager PlayerInputManager { get; private set; }

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();

        if (PhotonView.isMine)
        {
            PlayerInputManager = gameObject.AddComponent<PlayerInputManager>();
        }
        PlayerMovement = gameObject.AddComponent<PlayerMovement>();
    }
}
