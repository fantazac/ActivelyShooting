using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelInfoTransmitter : MonoBehaviour
{
    private Player player;

    public delegate void OnChangeEnemyHealthInfoReceivedHandler(int id, bool reduce, float amount, bool isPercent);
    public event OnChangeEnemyHealthInfoReceivedHandler OnChangeEnemyHealthInfoReceived;

    public delegate void OnChangeTowerHealthInfoReceivedHandler(bool reduce, float amount, bool isPercent);
    public event OnChangeTowerHealthInfoReceivedHandler OnChangeTowerHealthInfoReceived;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void ResetEvents()
    {
        OnChangeEnemyHealthInfoReceived = null;
        OnChangeTowerHealthInfoReceived = null;
    }

    public void ChangeEnemyHealthOnServer(int id, bool reduce, float amount, bool isPercent)
    {
        SendToServer_ChangeEnemyHealth(id, reduce, amount, isPercent);
    }

    private void SendToServer_ChangeEnemyHealth(int id, bool reduce, float amount, bool isPercent)
    {
        player.PhotonView.RPC("ReceiveFromServer_ChangeEnemyHealth", PhotonTargets.Others, id, reduce, amount, isPercent);
    }

    [PunRPC]
    private void ReceiveFromServer_ChangeEnemyHealth(int id, bool reduce, float amount, bool isPercent)
    {
        OnChangeEnemyHealthInfoReceived(id, reduce, amount, isPercent);
    }

    public void ChangeTowerHealthOnServer(bool reduce, float amount, bool isPercent)
    {
        SendToServer_ChangeTowerHealth(reduce, amount, isPercent);
    }

    private void SendToServer_ChangeTowerHealth(bool reduce, float amount, bool isPercent)
    {
        player.PhotonView.RPC("ReceiveFromServer_ChangeTowerHealth", PhotonTargets.Others, reduce, amount, isPercent);
    }

    [PunRPC]
    private void ReceiveFromServer_ChangeTowerHealth(bool reduce, float amount, bool isPercent)
    {
        OnChangeTowerHealthInfoReceived(reduce, amount, isPercent);
    }
}
