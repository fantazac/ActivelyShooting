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
        if (!ability.IsOnCooldown)//Currently, means it will take slow off Gunner when the auto is back up
        {
            Vector2 sceneMousePosition = StaticObjects.PlayerCamera.ScreenToWorldPoint(mousePosition);
            ability.UseAbility(sceneMousePosition, isPressed);
            SendToServer_Ability(abilityId, sceneMousePosition, isPressed);
        }
    }

    protected void SendToServer_Ability(int abilityId, Vector3 mousePosition, bool isPressed)
    {
        player.PhotonView.RPC("ReceiveFromServer_Ability", PhotonTargets.Others, abilityId, mousePosition, isPressed);
    }

    [PunRPC]
    protected void ReceiveFromServer_Ability(int abilityId, Vector3 mousePosition, bool isPressed)
    {
        abilities[abilityId].UseAbility(mousePosition, isPressed);
    }
}
