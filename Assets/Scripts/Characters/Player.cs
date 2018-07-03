using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Entity
{
    [SerializeField]
    private BoxCollider2D playerGroundHitbox;

    private float gracePeriodDuration;
    private float gracePeriodDurationRemaining;

    public bool IsInGracePeriod { get; private set; }

    public BoxCollider2D PlayerGroundHitbox { get { return playerGroundHitbox; } }
    public BoxCollider2D PlayerHitbox { get; private set; }

    public PlayerAbilityManager PlayerAbilityManager { get; protected set; }
    public PlayerGroundHitboxManager PlayerGroundHitboxManager { get; private set; }
    public PlayerInputManager PlayerInputManager { get; private set; }
    public PlayerLevelInfoTransmitter PlayerLevelInfoTransmitter { get; private set; }
    public PlayerMovementManager PlayerMovementManager { get; protected set; }
    public PlayerPortalManager PlayerPortalManager { get; private set; }

    public Player[] Party { get; private set; }

    public delegate void OnPartyUpdatedHandler();
    public event OnPartyUpdatedHandler OnPartyUpdated;

    protected Player()
    {
        gracePeriodDuration = 1.5f;
    }

    protected override void Awake()
    {
        base.Awake();

        if (PhotonView.isMine)
        {
            PlayerGroundHitboxManager = gameObject.AddComponent<PlayerGroundHitboxManager>();
            PlayerHitbox = GetComponent<BoxCollider2D>();
            PlayerInputManager = gameObject.AddComponent<PlayerInputManager>();
            PlayerPortalManager = gameObject.AddComponent<PlayerPortalManager>();
        }
        else
        {
            Destroy(EntityRigidBody);
            Destroy(playerGroundHitbox.gameObject);
            playerGroundHitbox = null;
        }

        PlayerLevelInfoTransmitter = gameObject.AddComponent<PlayerLevelInfoTransmitter>();

        if (PhotonView.isMine)
        {
            SendToServer_UpdateParty();
        }
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
        foreach (Player p in Party)
        {
            p.PlayerLevelInfoTransmitter.ResetEvents();
        }
        if(OnPartyUpdated != null)
        {
            OnPartyUpdated();
        }
    }

    public override void ChangeHealthOnServer(bool reduce, float amount, bool isPercent = false)
    {
        base.ChangeHealthOnServer(reduce, amount, isPercent);
        IsInGracePeriod = true;
        StartCoroutine(GracePeriod());
    }

    private IEnumerator GracePeriod()
    {
        gracePeriodDurationRemaining = gracePeriodDuration;

        while (gracePeriodDurationRemaining > 0)
        {
            gracePeriodDurationRemaining -= Time.deltaTime;

            yield return null;
        }

        IsInGracePeriod = false;
    }
}
