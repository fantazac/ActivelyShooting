using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Player
{
    private Fighter()
    {
        maxHealth = 300;
    }

    protected override void Awake()
    {
        base.Awake();

        PlayerAbilityManager = gameObject.AddComponent<FighterAbilityManager>();
        PlayerMovementManager = gameObject.AddComponent<FighterMovementManager>();

        if (PhotonView.isMine)
        {
            PhotonNetwork.Instantiate("Enemy", new Vector2(-15, 0), Quaternion.identity, 0);
            PhotonNetwork.Instantiate("Enemy", new Vector2(20, 0), Quaternion.identity, 0);
        }
    }
}
