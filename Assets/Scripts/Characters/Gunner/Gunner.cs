using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : Player
{
    private Gunner()
    {
        maxHealth = 100;
    }

    protected override void Awake()
    {
        base.Awake();

        PlayerAbilityManager = gameObject.AddComponent<GunnerAbilityManager>();
        PlayerMovementManager = gameObject.AddComponent<GunnerMovementManager>();

        if (PhotonView.isMine)
        {
            PhotonNetwork.Instantiate("Enemy", Vector2.right, Quaternion.identity, 0);
            PhotonNetwork.Instantiate("Enemy", Vector2.left, Quaternion.identity, 0);
        }
    }
}
