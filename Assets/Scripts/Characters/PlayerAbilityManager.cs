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
            if (!(ability is FighterQ || ability is GunnerQ || ability is MageQ))
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
            Vector2 mousePositionInsideScreen = new Vector2(Mathf.Clamp(mousePosition.x, 0, Screen.width), Mathf.Clamp(mousePosition.y, 0, Screen.height));
            Vector2 sceneMousePosition = StaticObjects.PlayerCamera.ScreenToWorldPoint(mousePositionInsideScreen);
            ability.UseAbility(sceneMousePosition, isPressed);
            SendToServer_Ability(abilityId, sceneMousePosition, isPressed);
        }
    }

    public void UseAbilityWithoutLimitation(int abilityId, Vector3 mousePosition, bool isPressed = false)
    {
        Vector2 mousePositionInsideScreen = new Vector2(Mathf.Clamp(mousePosition.x, 0, Screen.width), Mathf.Clamp(mousePosition.y, 0, Screen.height));
        Vector2 sceneMousePosition = StaticObjects.PlayerCamera.ScreenToWorldPoint(mousePositionInsideScreen);
        abilities[abilityId].UseAbility(sceneMousePosition, isPressed, false);
        SendToServer_Ability(abilityId, sceneMousePosition, isPressed, false);
    }

    public void SetAbilityActive(int abilityId, bool active)
    {
        abilities[abilityId].SetActive(active);
    }

    protected void OnSwitchWeapon()
    {
        SwitchWeapon();
        SendToServer_Switch();
    }

    protected void SendToServer_Ability(int abilityId, Vector3 mousePosition, bool isPressed, bool putOnCooldown = true)
    {
        player.PhotonView.RPC("ReceiveFromServer_Ability", PhotonTargets.Others, abilityId, mousePosition, isPressed, putOnCooldown);
    }

    [PunRPC]
    protected void ReceiveFromServer_Ability(int abilityId, Vector3 mousePosition, bool isPressed, bool putOnCooldown)
    {
        abilities[abilityId].UseAbilityOnNetwork(mousePosition, isPressed, putOnCooldown);
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
