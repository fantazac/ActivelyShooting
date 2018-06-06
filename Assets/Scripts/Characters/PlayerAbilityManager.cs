using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAbilityManager : MonoBehaviour
{
    protected Player player;

    protected bool leftClickIsPressed;
    protected bool rightClickIsPressed;

    protected Ability[] abilities;

    protected virtual void Awake()
    {
        player = GetComponent<Player>();

        if (player.PlayerInputManager)
        {
            player.PlayerInputManager.OnAbilityPressed += UseAbility;
            player.PlayerInputManager.OnTabPressed += SwitchWeapon;
        }
    }

    protected abstract void SwitchWeapon();

    protected void UseAbility(int abilityId, Vector3 mousePosition, bool isPressed = false)
    {
        Ability ability = abilities[abilityId];
        if (!ability.IsOnCooldown)
        {
            ability.UseAbility(mousePosition, isPressed);
            SendToServer_Ability(abilityId, mousePosition, isPressed);
        }
    }

    protected void SendToServer_Ability(int abilityId, Vector3 mousePosition, bool isPressed)
    {
        player.PhotonView.RPC("ReceiveFromServer_Ability", PhotonTargets.Others, abilityId, mousePosition, isPressed);
    }

    [PunRPC]
    private void ReceiveFromServer_QAbility(int abilityId, Vector3 mousePosition, bool isPressed)
    {
        abilities[abilityId].UseAbility(mousePosition, isPressed);
    }
}
