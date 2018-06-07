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

    protected override void SwitchWeapon()
    {
        if (selectedMode == FighterMode.Swordsman)
        {
            selectedMode = FighterMode.Tank;
            abilities[2].ChangeWeapon((int)FighterMode.Tank);
        }
        else
        {
            selectedMode = FighterMode.Swordsman;
            abilities[2].ChangeWeapon((int)FighterMode.Swordsman);
        }
    }
}

public enum FighterMode
{
    Swordsman,
    Tank
}
