using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D playerGroundHitbox;
    [SerializeField]
    private BoxCollider2D playerJumpingHitbox;

    public PhotonView PhotonView { get; private set; }

    public BoxCollider2D PlayerGroundHitbox { get { return playerGroundHitbox; } }
    public BoxCollider2D PlayerHitbox { get; private set; }
    public BoxCollider2D PlayerJumpingHitbox { get { return playerJumpingHitbox; } }
    public Rigidbody2D PlayerRigidBody { get; private set; }

    public PlayerAbilityManager PlayerAbilityManager { get; protected set; }
    public PlayerGroundHitboxManager PlayerGroundHitboxManager { get; private set; }
    public PlayerInputManager PlayerInputManager { get; private set; }
    public PlayerJumpingHitboxManager PlayerJumpingHitboxManager { get; private set; }
    public PlayerMovementManager PlayerMovementManager { get; protected set; }

    public Player[] Party { get; private set; }

    protected virtual void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        PlayerRigidBody = GetComponent<Rigidbody2D>();

        if (PhotonView.isMine)
        {
            PlayerGroundHitboxManager = gameObject.AddComponent<PlayerGroundHitboxManager>();
            PlayerHitbox = GetComponent<BoxCollider2D>();
            PlayerInputManager = gameObject.AddComponent<PlayerInputManager>();
            PlayerJumpingHitboxManager = gameObject.AddComponent<PlayerJumpingHitboxManager>();
        }
        else
        {
            Destroy(PlayerRigidBody);
            Destroy(playerGroundHitbox.gameObject);
            Destroy(playerJumpingHitbox.gameObject);
            playerGroundHitbox = null;
        }
        if (!(this is Gunner))
        {
            PlayerMovementManager = gameObject.AddComponent<PlayerMovementManager>();
        }

        SendToServer_UpdateParty();
        UpdateParty();
    }

    protected void SendToServer_UpdateParty()
    {
        PhotonView.RPC("ReceiveFromServer_UpdateParty", PhotonTargets.Others);
    }

    [PunRPC]
    protected void ReceiveFromServer_UpdateParty()
    {
        foreach (Player p in FindObjectsOfType<Player>())
        {
            p.UpdateParty();
        }
    }

    protected void UpdateParty()
    {
        Party = FindObjectsOfType<Player>();
    }
}
