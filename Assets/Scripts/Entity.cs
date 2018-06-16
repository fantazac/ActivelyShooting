using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected float damageReduction;
    protected float maxHealth;

    public Health Health { get; private set; }

    public PhotonView PhotonView { get; private set; }
    public Rigidbody2D EntityRigidBody { get; private set; }

    private bool sentConnectionInfoRequest;

    protected virtual void Awake()
    {
        Health = gameObject.AddComponent<Health>();
        Health.SetMaxHealth(maxHealth);

        PhotonView = GetComponent<PhotonView>();
        EntityRigidBody = GetComponent<Rigidbody2D>();

        if (StaticObjects.Player && StaticObjects.Player != this)
        {
            StaticObjects.Player.transform.parent.GetComponentInChildren<HealthBarManager>().SetupHealthBarForEntity(this);
        }
    }

    protected virtual void Start() { }

    public float GetDamageReduction()
    {
        return 1 - Mathf.Clamp(damageReduction, 0, 1);
    }

    public void SetDamageReduction(float amount)
    {
        damageReduction += amount;
    }

    public virtual void ChangeHealthOnServer(bool reduce, float amount, bool isPercent = false)
    {
        SendToServer_ChangeHealth(reduce, amount, isPercent);
    }

    protected void SendToServer_ChangeHealth(bool reduce, float amount, bool isPercent)
    {
        PhotonView.RPC("ReceiveFromServer_ChangeHealth", PhotonTargets.Others, reduce, amount, isPercent);
    }

    [PunRPC]
    protected void ReceiveFromServer_ChangeHealth(bool reduce, float amount, bool isPercent)
    {
        if (reduce)
        {
            Health.ReduceFromServer(amount);
        }
        else
        {
            Health.RestoreFromServer(amount, isPercent);
        }
    }

    private void OnDestroy()
    {
        if (StaticObjects.Player)
        {
            StaticObjects.Player.transform.parent.GetComponentInChildren<HealthBarManager>().RemoveHealthBarOfDeletedEntity(this);
        }
    }

    [PunRPC]
    protected void ReceiveFromServer_ConnectionInfo(Vector3 position, float currentHealth)
    {
        if (sentConnectionInfoRequest)
        {
            sentConnectionInfoRequest = false;
            transform.position = position;
            Health.SetCurrentHealthOnLoad(currentHealth);
        }
    }

    [PunRPC]
    protected void ReceiveFromServer_ConnectionInfoRequest()
    {
        if (PhotonView.isMine || this is Enemy)
        {
            PhotonView.RPC("ReceiveFromServer_ConnectionInfo", PhotonTargets.Others, transform.position, Health.GetCurrentHealth());
        }
    }

    public void SendToServer_ConnectionInfoRequest()
    {
        sentConnectionInfoRequest = true;
        PhotonView.RPC("ReceiveFromServer_ConnectionInfoRequest", PhotonTargets.Others);
    }
}
