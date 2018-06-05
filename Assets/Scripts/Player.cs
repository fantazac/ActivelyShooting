using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D playerGroundHitbox;

    public PhotonView PhotonView { get; private set; }

    public BoxCollider2D PlayerGroundHitbox { get { return playerGroundHitbox; } }
    public BoxCollider2D PlayerHitbox { get; private set; }
    public Rigidbody2D PlayerRigidBody { get; private set; }

    public PlayerGroundHitboxManager PlayerGroundHitboxManager { get; private set; }
    public PlayerInputManager PlayerInputManager { get; private set; }
    public PlayerMovement PlayerMovement { get; private set; }

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        PlayerRigidBody = GetComponent<Rigidbody2D>();

        if (PhotonView.isMine)
        {
            PlayerGroundHitboxManager = gameObject.AddComponent<PlayerGroundHitboxManager>();
            PlayerHitbox = GetComponent<BoxCollider2D>();
            PlayerInputManager = gameObject.AddComponent<PlayerInputManager>();
        }
        else
        {
            Destroy(PlayerRigidBody);
            Destroy(playerGroundHitbox.gameObject);
            playerGroundHitbox = null;
        }
        PlayerMovement = gameObject.AddComponent<PlayerMovement>();
    }
}
