using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAbilityManager : PlayerAbilityManager
{
    private FighterMode selectedMode;

    private FighterAbilityManager()
    {
        selectedMode = FighterMode.Swordsman;
    }

    protected override void Awake()
    {
        base.Awake();

        abilities = new Ability[] { gameObject.AddComponent<FighterQ>(), gameObject.AddComponent<FighterE>(), gameObject.AddComponent<FighterLeftClick>(), gameObject.AddComponent<FighterRightClick>() };
    }

    private void OnGUI()
    {
        if (player.PhotonView.isMine)
        {
            GUILayout.Label("");//Ping
            GUILayout.Label("");//Position
            GUILayout.Label(selectedMode + "");
        }
    }

    protected override void UseAbility(int abilityId, Vector3 mousePosition, bool isPressed = false)
    {
        if(abilityId != 1)
        {
            base.UseAbility(abilityId, mousePosition, isPressed);
        }
        else
        {
            Ability ability = abilities[abilityId];
            if (!ability.IsOnCooldown && ability.IsAvailable())
            {
                Vector2 middleOfScreen = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                Vector2 middleOfScenePosition = StaticObjects.PlayerCamera.ScreenToWorldPoint(middleOfScreen);
                ability.UseAbility(middleOfScenePosition, isPressed);
                SendToServer_Ability(abilityId, middleOfScenePosition, isPressed);
            }
        }
    }

    protected override void SwitchWeapon()
    {
        if (selectedMode == FighterMode.Swordsman)
        {
            selectedMode = FighterMode.Tank;
            abilities[2].ChangeType((int)FighterMode.Tank);
        }
        else
        {
            selectedMode = FighterMode.Swordsman;
            abilities[2].ChangeType((int)FighterMode.Swordsman);
        }
    }
}

public enum FighterMode
{
    Swordsman,
    Tank
}
