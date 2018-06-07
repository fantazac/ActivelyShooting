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
            player.PlayerInputManager.OnTabPressed += OnSwitchWeapon;
        }
    }

    protected abstract void SwitchWeapon();

    public void SetCooldownReductionForAbilities(float cooldownReduction)
    {
        foreach (Ability ability in abilities)
        {
            if (!(ability is GunnerQ || ability is FighterQ))
            {
                ability.SetCooldownReduction(cooldownReduction);
            }
        }
    }

    public void SetDamageAmplificationForAbilities(float damageAmplification)
    {
        foreach (Ability ability in abilities)
        {
            ability.SetDamageAmplification(damageAmplification);
        }
    }

    protected void UseAbility(int abilityId, Vector3 mousePosition, bool isPressed = false)
    {
        Ability ability = abilities[abilityId];
        if (!ability.IsOnCooldown && ability.IsAvailable())
        {
            Vector2 sceneMousePosition = StaticObjects.PlayerCamera.ScreenToWorldPoint(mousePosition);
            ability.UseAbility(sceneMousePosition, isPressed);
            SendToServer_Ability(abilityId, sceneMousePosition, isPressed);
        }
    }

    protected void OnSwitchWeapon()
    {
        SwitchWeapon();
        SendToServer_Switch();
    }

    protected void SendToServer_Ability(int abilityId, Vector3 mousePosition, bool isPressed)
    {
        player.PhotonView.RPC("ReceiveFromServer_Ability", PhotonTargets.Others, abilityId, mousePosition, isPressed);
    }

    [PunRPC]
    protected void ReceiveFromServer_Ability(int abilityId, Vector3 mousePosition, bool isPressed)
    {
        abilities[abilityId].UseAbilityOnNetwork(mousePosition, isPressed);
    }

    protected void SendToServer_Switch()
    {
        player.PhotonView.RPC("ReceiveFromServer_Switch", PhotonTargets.Others);
    }

    [PunRPC]
    protected void ReceiveFromServer_Switch()
    {
        SwitchWeapon();
    }
}
