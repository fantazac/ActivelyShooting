using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected float damageReduction;
    protected float maxHealth;

    public int ID { get; protected set; }

    public Health Health { get; private set; }

    public PhotonView PhotonView { get; private set; }

    protected virtual void Awake()
    {
        Health = gameObject.AddComponent<Health>();
        Health.SetMaxHealth(maxHealth);

        PhotonView = GetComponent<PhotonView>();
    }

    public float GetDamageReduction()
    {
        return 1 - Mathf.Clamp(damageReduction, 0, 1);
    }

    public void SetDamageReduction(float amount)
    {
        damageReduction += amount;
    }

    public void ChangeHealthOnServer(bool reduce, float amount, bool isPercent = false)
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
}
