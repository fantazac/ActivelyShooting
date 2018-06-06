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
        }
        else
        {
            selectedMode = FighterMode.Swordsman;
        }
    }
}

public enum FighterMode
{
    Swordsman,
    Tank
}
